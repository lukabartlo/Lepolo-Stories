using UnityEngine;
using System.Collections.Generic;


public class PathNode
{
    public int X { get; set; }
    public int Y { get; set; }
    public float GCost { get; set; }
    public float HCost { get; set; }
    public float FCost { get { return GCost + HCost; } }
    public PathNode Parent { get; set; }
    public bool IsWalkable { get; set; }

    public PathNode(int x, int y, bool isWalkable = true)
    {
        X = x;
        Y = y;
        IsWalkable = isWalkable;
        Reset();
    }

    public void Reset()
    {
        GCost = 0;
        HCost = 0;
        Parent = null;
    }
}

public class PathGrid
{
    public PathNode[,] Nodes { get; private set; }
    public int Width { get; private set; }
    public int Height { get; private set; }
    public float NodeSize { get; private set; }

    private static readonly int[] DirectionX = { -1, 0, 1, -1, 1, -1, 0, 1 };
    private static readonly int[] DirectionY = { -1, -1, -1, 0, 0, 1, 1, 1 };

    public PathGrid(int width, int height, float nodeSize = 1f)
    {
        Width = width;
        Height = height;
        NodeSize = nodeSize;
        Nodes = new PathNode[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Nodes[x, y] = new PathNode(x, y);
            }
        }
    }

    public PathNode GetNode(int x, int y)
    {
        if (IsValidCoordinate(x, y))
            return Nodes[x, y];
        return null;
    }

    public void SetWalkable(int x, int y, bool walkable)
    {
        if (IsValidCoordinate(x, y))
            Nodes[x, y].IsWalkable = walkable;
    }

    public Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x * NodeSize, 0, y * NodeSize);
    }

    public List<PathNode> GetNeighbors(PathNode node)
    {
        List<PathNode> neighbors = new List<PathNode>(8);

        for (int i = 0; i < 8; i++)
        {
            int newX = node.X + DirectionX[i];
            int newY = node.Y + DirectionY[i];
            PathNode neighbor = GetNode(newX, newY);

            if (neighbor != null && neighbor.IsWalkable)
                neighbors.Add(neighbor);
        }

        return neighbors;
    }

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

    private bool IsValidCoordinate(int x, int y)
    {
        return x >= 0 && x < Width && y >= 0 && y < Height;
    }
}

public class AStarPathfinding : MonoBehaviour
{
    [Header("Grid Settings")]
    [SerializeField] private int gridWidth = 50;
    [SerializeField] private int gridHeight = 50;
    [SerializeField] private float nodeSize = 1f;
    
    [Header("Visualisation")]
    [SerializeField] private bool drawGizmos = true;
    [SerializeField] private bool drawGrid = true;
    [SerializeField] private bool drawPath = true;
    [SerializeField] private Color walkableColor = Color.white;
    [SerializeField] private Color obstacleColor = Color.red;
    [SerializeField] private Color pathColor = Color.green;
    
    [Header("Obstacles (Example)")]
    [SerializeField] private bool createExampleObstacles = true;
    
    [Header("Pathfinding Test (Visualisation)")]
    [SerializeField] private bool enablePathTest = true;
    [SerializeField] private Vector2Int testStartPos = new Vector2Int(5, 5);
    [SerializeField] private Vector2Int testEndPos = new Vector2Int(45, 45);

    private PathGrid grid;
    private List<PathNode> currentPath;
    
    private const float DIAGONAL_COST = 1.414f;
    private const float STRAIGHT_COST = 1f;

    private void Awake()
    { 
        InitializeGrid();
    }

    private void Start()
    {
        if (enablePathTest && grid != null)
        {
            currentPath = FindPath(testStartPos.x, testStartPos.y, testEndPos.x, testEndPos.y);
        }
    }

    private void InitializeGrid()
    {
        grid = new PathGrid(gridWidth, gridHeight, nodeSize);

        if (createExampleObstacles)
        {
            for (int i = 15; i < 35; i++)
            {
                grid.SetWalkable(i, 25, false);
            }
        }
    }

    public void CreateGrid(int width, int height, float size = 1f)
    {
        gridWidth = width;
        gridHeight = height;
        nodeSize = size;
        InitializeGrid();
    }

    public void SetWalkable(int x, int y, bool walkable)
    {
        if (grid != null)
            grid.SetWalkable(x, y, walkable);
    }

    public PathGrid GetGrid()
    {
        return grid;
    }

    public List<PathNode> FindPath(Vector3 startWorldPos, Vector3 endWorldPos)
    {
        int startX = Mathf.RoundToInt(startWorldPos.x / nodeSize);
        int startY = Mathf.RoundToInt(startWorldPos.z / nodeSize);
        int endX = Mathf.RoundToInt(endWorldPos.x / nodeSize);
        int endY = Mathf.RoundToInt(endWorldPos.z / nodeSize);

        return FindPath(startX, startY, endX, endY);
    }

    public List<PathNode> FindPath(int startX, int startY, int endX, int endY)
    {
        if (grid == null)
        {
            return new List<PathNode>();
        }

        PathNode startNode = grid.GetNode(startX, startY);
        PathNode endNode = grid.GetNode(endX, endY); 
        
        if (startNode == null || endNode == null)
        {
            return new List<PathNode>();
        }

        if (!endNode.IsWalkable)
        {
            return new List<PathNode>();
        }

        grid.Reset();

        List<PathNode> openList = new List<PathNode>();
        HashSet<PathNode> closedList = new HashSet<PathNode>();

        openList.Add(startNode);

        while (openList.Count > 0)
        {
            PathNode currentNode = GetLowestFCostNode(openList);
            openList.Remove(currentNode);
            closedList.Add(currentNode);

            if (currentNode == endNode)
            {
                currentPath = RetracePath(startNode, endNode);
                return currentPath;
            }

            List<PathNode> neighbors = grid.GetNeighbors(currentNode);

            foreach (PathNode neighbor in neighbors)
            {
                if (closedList.Contains(neighbor))
                    continue;

                float tentativeGCost = currentNode.GCost + GetDistance(currentNode, neighbor);

                if (!openList.Contains(neighbor))
                {
                    neighbor.GCost = tentativeGCost;
                    neighbor.HCost = GetHeuristic(neighbor, endNode);
                    neighbor.Parent = currentNode;
                    openList.Add(neighbor);
                }
                else if (tentativeGCost < neighbor.GCost)
                {
                    neighbor.GCost = tentativeGCost;
                    neighbor.Parent = currentNode;
                }
            }
        }

        return new List<PathNode>();
    }

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

    private float GetDistance(PathNode a, PathNode b)
    {
        int dx = Mathf.Abs(a.X - b.X);
        int dy = Mathf.Abs(a.Y - b.Y);

        if (dx > dy)
            return DIAGONAL_COST * dy + STRAIGHT_COST * (dx - dy);
        return DIAGONAL_COST * dx + STRAIGHT_COST * (dy - dx);
    }

    private float GetHeuristic(PathNode a, PathNode b)
    {
        int dx = Mathf.Abs(a.X - b.X);
        int dy = Mathf.Abs(a.Y - b.Y);
        return STRAIGHT_COST * (dx + dy);
    }

    private List<PathNode> RetracePath(PathNode startNode, PathNode endNode)
    {
        List<PathNode> path = new List<PathNode>();
        PathNode currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.Parent;
        }

        path.Reverse();
        return path;
    }

    public List<Vector3> GetWorldPath(List<PathNode> path)
    {
        List<Vector3> worldPath = new List<Vector3>();
        foreach (PathNode node in path)
        {
            worldPath.Add(grid.GetWorldPosition(node.X, node.Y));
        }
        return worldPath;
    }

    private void OnDrawGizmos()
    {
        if (!drawGizmos || grid == null) 
            return;

        if (drawGrid)
            DrawGridGizmos();

        if (drawPath && currentPath != null && currentPath.Count > 0)
            DrawPathGizmos();
        
        if (enablePathTest)
            DrawTestPoints();
    }

    private void DrawGridGizmos()
    {
        for (int x = 0; x < grid.Width; x++)
        {
            for (int y = 0; y < grid.Height; y++)
            {
                Vector3 pos = grid.GetWorldPosition(x, y);
                float size = grid.NodeSize * 0.4f;

                Gizmos.color = grid.Nodes[x, y].IsWalkable ? walkableColor : obstacleColor;
                Gizmos.DrawCube(pos, new Vector3(size, 0.1f, size));
            }
        }
    }

    private void DrawPathGizmos()
    {
        Gizmos.color = pathColor;
        
        for (int i = 0; i < currentPath.Count - 1; i++)
        {
            Vector3 pos1 = grid.GetWorldPosition(currentPath[i].X, currentPath[i].Y);
            Vector3 pos2 = grid.GetWorldPosition(currentPath[i + 1].X, currentPath[i + 1].Y);
            Gizmos.DrawLine(pos1 + Vector3.up * 0.2f, pos2 + Vector3.up * 0.2f);
            Gizmos.DrawSphere(pos1 + Vector3.up * 0.2f, 0.2f);
        }

        if (currentPath.Count > 0)
        {
            PathNode lastNode = currentPath[currentPath.Count - 1];
            Vector3 lastPos = grid.GetWorldPosition(lastNode.X, lastNode.Y);
            Gizmos.DrawSphere(lastPos + Vector3.up * 0.2f, 0.3f);
        }
    }

    private void DrawTestPoints()
    {
        // Starting point (blue)
        Vector3 startPos = grid.GetWorldPosition(testStartPos.x, testStartPos.y);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(startPos + Vector3.up * 0.5f, 0.5f);
        Gizmos.DrawLine(startPos, startPos + Vector3.up * 0.5f);
        
        // Ending point (cyan)
        Vector3 endPos = grid.GetWorldPosition(testEndPos.x, testEndPos.y);
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(endPos + Vector3.up * 0.5f, 0.5f);
        Gizmos.DrawLine(endPos, endPos + Vector3.up * 0.5f);
    }
}

