using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Represents a single node in the pathfinding grid.
/// Contains position, cost values, and walkability information.
/// </summary>
public class PathNode
{
    public int X { get; set; }
    public int Y { get; set; }
    
    // GCost: Distance from starting node
    public float GCost { get; set; }
    
    // HCost: Estimated distance to end node (heuristic)
    public float HCost { get; set; }
    
    // FCost: Total cost (GCost + HCost) - used to determine best path
    public float FCost { get { return GCost + HCost; } }
    
    // Reference to the previous node in the path
    public PathNode Parent { get; set; }
    
    // Whether this node can be traversed
    public bool IsWalkable { get; set; }

    /// <summary>
    /// Constructor for PathNode
    /// </summary>
    /// <param name="x">X coordinate in grid</param>
    /// <param name="y">Y coordinate in grid</param>
    /// <param name="isWalkable">Whether the node is walkable</param>
    public PathNode(int x, int y, bool isWalkable = true)
    {
        X = x;
        Y = y;
        IsWalkable = isWalkable;
        Reset();
    }

    /// <summary>
    /// Resets the pathfinding costs and parent reference
    /// </summary>
    public void Reset()
    {
        GCost = 0;
        HCost = 0;
        Parent = null;
    }
}

/// <summary>
/// Manages the grid of PathNodes used for pathfinding.
/// Handles grid creation, neighbor calculation, and world position conversion.
/// </summary>
public class PathGrid
{
    public PathNode[,] Nodes { get; private set; }
    public int Width { get; private set; }
    public int Height { get; private set; }
    public float NodeSize { get; private set; }

    // Direction arrays for finding 8 neighbors (including diagonals)
    private static readonly int[] _directionX = { -1, 0, 1, -1, 1, -1, 0, 1 };
    private static readonly int[] _directionY = { -1, -1, -1, 0, 0, 1, 1, 1 };

    /// <summary>
    /// Creates a new PathGrid with specified dimensions
    /// </summary>
    /// <param name="width">Width of the grid</param>
    /// <param name="height">Height of the grid</param>
    /// <param name="nodeSize">Size of each node in world units</param>
    public PathGrid(int width, int height, float nodeSize = 1f)
    {
        Width = width;
        Height = height;
        NodeSize = nodeSize;
        Nodes = new PathNode[width, height];

        // Initialize all nodes in the grid
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Nodes[x, y] = new PathNode(x, y);
            }
        }
    }

    /// <summary>
    /// Returns the node at the specified grid coordinates
    /// </summary>
    public PathNode GetNode(int x, int y)
    {
        if (IsValidCoordinate(x, y))
            return Nodes[x, y];
        return null;
    }

    /// <summary>
    /// Sets the walkability state of a node at the given coordinates
    /// </summary>
    public void SetWalkable(int x, int y, bool walkable)
    {
        if (IsValidCoordinate(x, y))
            Nodes[x, y].IsWalkable = walkable;
    }

    /// <summary>
    /// Converts grid coordinates to world position
    /// </summary>
    public Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x * NodeSize, 0, y * NodeSize);
    }

    /// <summary>
    /// Returns all walkable neighbors of a given node (up to 8 directions)
    /// </summary>
    public List<PathNode> GetNeighbors(PathNode node)
    {
        List<PathNode> neighbors = new List<PathNode>(8);

        for (int i = 0; i < 8; i++)
        {
            int newX = node.X + _directionX[i];
            int newY = node.Y + _directionY[i];
            PathNode neighbor = GetNode(newX, newY);

            if (neighbor != null && neighbor.IsWalkable)
                neighbors.Add(neighbor);
        }

        return neighbors;
    }

    /// <summary>
    /// Resets all nodes in the grid (clears pathfinding data)
    /// </summary>
    public void Reset()
    {
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                Nodes[x, y].Reset();
            }
        }
    }

    /// <summary>
    /// Checks if the given coordinates are within grid bounds
    /// </summary>
    private bool IsValidCoordinate(int x, int y)
    {
        return x >= 0 && x < Width && y >= 0 && y < Height;
    }
}

/// <summary>
/// A* Pathfinding.
/// Handles grid-based pathfinding with support for obstacles and diagonal movement.
/// </summary>
public class AStarPathfinding : MonoBehaviour
{
    [Header("Grid Settings")]
    [SerializeField] private int _gridWidth = 50;
    [SerializeField] private int _gridHeight = 50;
    [SerializeField] private float _nodeSize = 1f;
    
    [Header("Visualisation")]
    [SerializeField] private bool _drawGizmos = true;
    [SerializeField] private bool _drawGrid = true;
    [SerializeField] private bool _drawPath = true;
    [SerializeField] private Color _walkableColor = Color.white;
    [SerializeField] private Color _obstacleColor = Color.red;
    [SerializeField] private Color _pathColor = Color.green;
    
    [Header("Obstacles (Example)")]
    [SerializeField] private bool _createExampleObstacles = true;
    
    [Header("Pathfinding Test (Visualisation)")]
    [SerializeField] private bool _enablePathTest = true;
    [SerializeField] private Vector2Int _testStartPos = new Vector2Int(5, 5);
    [SerializeField] private Vector2Int _testEndPos = new Vector2Int(45, 45);

    // The grid containing all pathfinding nodes
    private PathGrid _grid;
    
    // The last calculated path (used for visualization)
    private List<PathNode> _currentPath;
    
    // Cost constants for pathfinding (diagonal movement costs more than straight)
    private const float _diagonalCost = 1.414f;
    private const float _straightCost = 1f;
    
    private void Awake()
    { 
        InitializeGrid();
    }
    
    private void Start()
    {
        if (_enablePathTest && _grid != null)
        {
            _currentPath = FindPath(_testStartPos.x, _testStartPos.y, _testEndPos.x, _testEndPos.y);
        }
    }
    
    private void InitializeGrid()
    {
        _grid = new PathGrid(_gridWidth, _gridHeight, _nodeSize);

        // Create a horizontal wall as an example obstacle
        if (_createExampleObstacles)
        {
            for (int i = 15; i < 35; i++)
            {
                _grid.SetWalkable(i, 25, false);
            }
        }
    }

    /// <summary>
    /// Creates a new grid with custom dimensions
    /// </summary>
    /// <param name="width">Grid width</param>
    /// <param name="height">Grid height</param>
    /// <param name="size">Size of each node in world units</param>
    public void CreateGrid(int width, int height, float size = 1f)
    {
        _gridWidth = width;
        _gridHeight = height;
        _nodeSize = size;
        InitializeGrid();
    }

    /// <summary>
    /// Sets whether a specific grid cell is walkable or not
    /// </summary>
    /// <param name="x">X coordinate in grid</param>
    /// <param name="y">Y coordinate in grid</param>
    /// <param name="walkable">Whether the cell should be walkable</param>
    public void SetWalkable(int x, int y, bool walkable)
    {
        if (_grid != null)
            _grid.SetWalkable(x, y, walkable);
    }

    /// <summary>
    /// Returns the current PathGrid instance
    /// </summary>
    public PathGrid GetGrid()
    {
        return _grid;
    }

    /// <summary>
    /// Finds a path between two world positions
    /// </summary>
    /// <param name="startWorldPos">Starting position in world space</param>
    /// <param name="endWorldPos">Target position in world space</param>
    /// <returns>List of PathNodes representing the path, or empty list if no path found</returns>
    public List<PathNode> FindPath(Vector3 startWorldPos, Vector3 endWorldPos)
    {
        // Convert world positions to grid coordinates
        int startX = Mathf.RoundToInt(startWorldPos.x / _nodeSize);
        int startY = Mathf.RoundToInt(startWorldPos.z / _nodeSize);
        int endX = Mathf.RoundToInt(endWorldPos.x / _nodeSize);
        int endY = Mathf.RoundToInt(endWorldPos.z / _nodeSize);

        return FindPath(startX, startY, endX, endY);
    }

    /// <summary>
    /// Finds a path between two grid coordinates using A* algorithm
    /// </summary>
    /// <param name="startX">Starting X coordinate</param>
    /// <param name="startY">Starting Y coordinate</param>
    /// <param name="endX">Target X coordinate</param>
    /// <param name="endY">Target Y coordinate</param>
    /// <returns>List of PathNodes representing the path, or empty list if no path found</returns>
    public List<PathNode> FindPath(int startX, int startY, int endX, int endY)
    {
        // Check if grid exists.
        if (_grid == null)
        {
            return new List<PathNode>();
        }

        // Get start and end nodes
        PathNode startNode = _grid.GetNode(startX, startY);
        PathNode endNode = _grid.GetNode(endX, endY); 
        
        // Check if nodes exists
        if (startNode == null || endNode == null)
        {
            return new List<PathNode>();
        }

        // Check if target is walkable
        if (!endNode.IsWalkable)
        {
            return new List<PathNode>();
        }

        // Reset all node costs from previous pathfinding
        _grid.Reset();

        // A* algorithm: openList contains nodes to evaluate, closedList contains evaluated nodes
        List<PathNode> openList = new List<PathNode>();
        HashSet<PathNode> closedList = new HashSet<PathNode>();

        openList.Add(startNode);

        // Main A* loop - continue until all reachable nodes are evaluated
        while (openList.Count > 0)
        {
            // Get the node with lowest F cost (most promising path)
            PathNode currentNode = GetLowestFCostNode(openList);
            openList.Remove(currentNode);
            closedList.Add(currentNode);

            // If we reached the target, retrace and return the path
            if (currentNode == endNode)
            {
                _currentPath = RetracePath(startNode, endNode);
                return _currentPath;
            }

            // Evaluate all neighbors of the current node
            List<PathNode> neighbors = _grid.GetNeighbors(currentNode);

            foreach (PathNode neighbor in neighbors)
            {
                // Skip already evaluated nodes
                if (closedList.Contains(neighbor))
                    continue;

                // Calculate the cost to reach this neighbor through current node
                float tentativeGCost = currentNode.GCost + GetDistance(currentNode, neighbor);

                // If this neighbor hasn't been evaluated yet
                if (!openList.Contains(neighbor))
                {
                    neighbor.GCost = tentativeGCost;
                    neighbor.HCost = GetHeuristic(neighbor, endNode);
                    neighbor.Parent = currentNode;
                    openList.Add(neighbor);
                }
                // If we found a better path to this neighbor
                else if (tentativeGCost < neighbor.GCost)
                {
                    neighbor.GCost = tentativeGCost;
                    neighbor.Parent = currentNode;
                }
            }
        }

        // No path found
        return new List<PathNode>();
    }

    /// <summary>
    /// Finds the node with the lowest FCost from a list of nodes.
    /// If multiple nodes have the same FCost, prefer the one with lower HCost.
    /// </summary>
    private PathNode GetLowestFCostNode(List<PathNode> nodes)
    {
        PathNode lowestNode = nodes[0];
        for (int i = 1; i < nodes.Count; i++)
        {
            if (nodes[i].FCost < lowestNode.FCost || 
                (nodes[i].FCost == lowestNode.FCost && nodes[i].HCost < lowestNode.HCost))
            {
                lowestNode = nodes[i];
            }
        }
        return lowestNode;
    }

    /// <summary>
    /// Calculates the actual distance between two nodes.
    /// Uses diagonal and straight costs for accurate pathfinding.
    /// </summary>
    private float GetDistance(PathNode a, PathNode b)
    {
        int dx = Mathf.Abs(a.X - b.X);
        int dy = Mathf.Abs(a.Y - b.Y);

        // Calculate mixed diagonal and straight movement
        if (dx > dy)
            return _diagonalCost * dy + _straightCost * (dx - dy);
        return _straightCost * dx + _straightCost * (dy - dx);
    }

    /// <summary>
    /// Calculates the heuristic (estimated distance) between two nodes.
    /// </summary>
    private float GetHeuristic(PathNode a, PathNode b)
    {
        int dx = Mathf.Abs(a.X - b.X);
        int dy = Mathf.Abs(a.Y - b.Y);
        return _straightCost * (dx + dy);
    }

    /// <summary>
    /// Reconstructs the final path by following parent references from end to start.
    /// Returns the path in correct order (start to end).
    /// </summary>
    private List<PathNode> RetracePath(PathNode startNode, PathNode endNode)
    {
        List<PathNode> path = new List<PathNode>();
        PathNode currentNode = endNode;

        // Follow parent references backwards
        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.Parent;
        }

        // Reverse to get path from start to end
        path.Reverse();
        return path;
    }

    /// <summary>
    /// Converts a path of PathNodes to a list of world positions
    /// </summary>
    /// <param name="path">The path to convert</param>
    /// <returns>List of world space Vector3 positions</returns>
    public List<Vector3> GetWorldPath(List<PathNode> path)
    {
        List<Vector3> worldPath = new List<Vector3>();
        foreach (PathNode node in path)
        {
            worldPath.Add(_grid.GetWorldPosition(node.X, node.Y));
        }
        return worldPath;
    }

    private void OnDrawGizmos()
    {
        if (!_drawGizmos || _grid == null) 
            return;

        if (_drawGrid)
            DrawGridGizmos();

        if (_drawPath && _currentPath != null && _currentPath.Count > 0)
            DrawPathGizmos();
        
        if (_enablePathTest)
            DrawTestPoints();
    }
    
    private void DrawGridGizmos()
    {
        for (int x = 0; x < _grid.Width; x++)
        {
            for (int y = 0; y < _grid.Height; y++)
            {
                Vector3 pos = _grid.GetWorldPosition(x, y);
                float size = _grid.NodeSize * 0.4f;

                // Color nodes based on walkability
                Gizmos.color = _grid.Nodes[x, y].IsWalkable ? _walkableColor : _obstacleColor;
                Gizmos.DrawCube(pos, new Vector3(size, 0.1f, size));
            }
        }
    }

    /// <summary>
    /// Draws the calculated path as connected lines and spheres
    /// </summary>
    private void DrawPathGizmos()
    {
        Gizmos.color = _pathColor;
        
        // Draw lines between path nodes
        for (int i = 0; i < _currentPath.Count - 1; i++)
        {
            Vector3 pos1 = _grid.GetWorldPosition(_currentPath[i].X, _currentPath[i].Y);
            Vector3 pos2 = _grid.GetWorldPosition(_currentPath[i + 1].X, _currentPath[i + 1].Y);
            Gizmos.DrawLine(pos1 + Vector3.up * 0.2f, pos2 + Vector3.up * 0.2f);
            Gizmos.DrawSphere(pos1 + Vector3.up * 0.2f, 0.2f);
        }

        // Draw the last node with a larger sphere
        if (_currentPath.Count > 0)
        {
            PathNode lastNode = _currentPath[_currentPath.Count - 1];
            Vector3 lastPos = _grid.GetWorldPosition(lastNode.X, lastNode.Y);
            Gizmos.DrawSphere(lastPos + Vector3.up * 0.2f, 0.3f);
        }
    }
    
    private void DrawTestPoints()
    {
        // Starting point (blue)
        Vector3 startPos = _grid.GetWorldPosition(_testStartPos.x, _testStartPos.y);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(startPos + Vector3.up * 0.5f, 0.5f);
        Gizmos.DrawLine(startPos, startPos + Vector3.up * 0.5f);
        
        // Ending point (cyan)
        Vector3 endPos = _grid.GetWorldPosition(_testEndPos.x, _testEndPos.y);
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(endPos + Vector3.up * 0.5f, 0.5f);
        Gizmos.DrawLine(endPos, endPos + Vector3.up * 0.5f);
    }
}

