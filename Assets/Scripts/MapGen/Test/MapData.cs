using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;
using System;
using Object = UnityEngine.Object;

public class MapData {
    
    #region Variables
    
    private CellData[,] _map;
    private Dictionary<ObjectType, List<GameObject>> _allMapObjectsByType =  new Dictionary<ObjectType, List<GameObject>>();
    private float _mapHeight;
    private Transform _objectsParent;
    public bool isMapGenerated = false;
    public AStarPathfinding pathfinding;
    private bool drawGizmo = false;
    
    #endregion

    public MapData(int _sizeX, int _sizeY, float _mapHeight, Transform _objectsParent) {
        _map =  new CellData[_sizeX , _sizeY];
        this._mapHeight = _mapHeight;
        this._objectsParent = _objectsParent;
        isMapGenerated = true;

        pathfinding = this._objectsParent.AddComponent<AStarPathfinding>();
        pathfinding.CreateGrid(_sizeX, _sizeY);
        pathfinding.SetMapDataRef(this);

    }
    
    
    #region Placing On Cells
    
    public bool TryBuildingOnCell(Vector2Int _buildCoord, ObjectData _objectData) {
        foreach (BuildingCells cell in _objectData.cellsOffsetFromOrigin) {
            Vector2Int coordsAfterOffset = _buildCoord + cell.offsetFromOrigin;
            
            if (!IsCoordInMap(coordsAfterOffset.x, coordsAfterOffset.y)) 
                return false;
            if (_map[coordsAfterOffset.x, coordsAfterOffset.y].cellState == CellState.Full) 
                return false;
            if (cell.cellState == CellState.Full && _map[coordsAfterOffset.x, coordsAfterOffset.y].cellState == CellState.Padding) 
                return false;
        }
        
        return true;
    }
    
    public void PlaceObjectOnMap(Vector2Int _buildCoord, ObjectData _objectData, ObjectType _objectType) {
        List<Vector2Int> _cellsToBuild = new List<Vector2Int>();
        Vector2Int _coordCellToBuild = Vector2Int.zero;

        foreach (BuildingCells cell in _objectData.cellsOffsetFromOrigin) {
            _coordCellToBuild.x = _buildCoord.x + cell.offsetFromOrigin.x;
            _coordCellToBuild.y = _buildCoord.y + cell.offsetFromOrigin.y;
            _cellsToBuild.Add(_coordCellToBuild);
            
            _map[_coordCellToBuild.x,  _coordCellToBuild.y].cellState = cell.cellState;
            
            if (cell.cellState == CellState.Full)
                pathfinding.SetWalkable(_coordCellToBuild.x,  _coordCellToBuild.y, false);
            
            if (cell.cellState == CellState.Padding)
                _map[_coordCellToBuild.x, _coordCellToBuild.y].paddingSecurity++;
        }
        
        for (int i = 0; i < _cellsToBuild.Count; i++) {
            ref CellData _cellOnGrid = ref _map[_cellsToBuild[i].x, _cellsToBuild[i].y];
            _cellOnGrid.connectedCells = new List<Vector2Int>();
            
            if (_cellOnGrid.cellState != CellState.Full)
                continue;
            
            for (int j = 0; j < _cellsToBuild.Count; j++) {
                if(_cellsToBuild[j] != _cellsToBuild[i])
                    _cellOnGrid.connectedCells.Add(_cellsToBuild[j]);
            }
            _cellOnGrid.objectType = _objectType;
            _cellOnGrid.sceneObject = _objectData.buildObject;
            _cellOnGrid.sceneObject.transform.position = new Vector3(_buildCoord.x + 0.5f, _mapHeight, _buildCoord.y + 0.5f);
        }
        
        if (_allMapObjectsByType.ContainsKey(_objectType)) {
            _allMapObjectsByType[_objectType].Add(_objectData.buildObject);
        } else {
            _allMapObjectsByType.Add(_objectType, new List<GameObject> { _objectData.buildObject });
        }
        
        
        // for (int i = 0; _buildCoord.x + i < _widthX; i++ ) {
        //     for (int j = 0;  _buildCoord.y + j < _widthY; j++) {
        //         
        //         _coordCellToBuild.x = _buildCoord.x + i;
        //         _coordCellToBuild.y = _buildCoord.y + j;
        //         _cellsToBuild.Add(_coordCellToBuild);
        //         
        //         // If is padding
        //         if (_buildCoord.x + i < _buildCoord.x + _objectData.padding ||
        //             _buildCoord.y + j < _buildCoord.y + _objectData.padding || 
        //             _buildCoord.x + i >= _widthX  - _objectData.padding ||
        //             _buildCoord.y + j >= _widthY  - _objectData.padding)
        //         {
        //             _map[_buildCoord.x + i, _buildCoord.y + j].cellState = CellState.Padding;
        //             _map[_buildCoord.x + i, _buildCoord.y + j].paddingSecurity++;
        //             continue;
        //         }
        //         
        //         // if building
        //         _map[_buildCoord.x + i, _buildCoord.y + j].cellState = CellState.Full;
        //     }
        // }
        //
        // // Add the connected cells to the building's cell
        // for (int i = 0; i < _cellsToBuild.Count; i++) {
        //     ref CellData _cellOnGrid = ref _map[_cellsToBuild[i].x, _cellsToBuild[i].y];
        //     _cellOnGrid.connectedCells = new List<Vector2Int>();
        //     
        //     if (_cellOnGrid.cellState != CellState.Full)
        //         continue;
        //     
        //     for (int j = 0; j < _cellsToBuild.Count; j++) {
        //         if(_cellsToBuild[j] != _cellsToBuild[i])
        //             _cellOnGrid.connectedCells.Add(_cellsToBuild[j]);
        //     }
        //     _cellOnGrid.objectType = _objectType;
        //     _cellOnGrid.sceneObject = _objectData.buildObject;
        //     _cellOnGrid.sceneObject.transform.position = new Vector3(_buildCoord.x + (float)_objectData.widthX / 2, _mapHeight, _buildCoord.y + (float)_objectData.widthY / 2);
        //     
        // }
        //
        // if (_allMapObjectsByType.ContainsKey(_objectType)) {
        //     _allMapObjectsByType[_objectType].Add(_objectData.buildObject);
        // } else {
        //     _allMapObjectsByType.Add(_objectType, new List<GameObject> { _objectData.buildObject });
        // }
    }
    
    #endregion 

    #region Deleting On Cells
    
    public bool TryDeleteCell(int _x, int _y)
    {
        if (!IsCoordInMap(_x, _y)) return false;
        if (_map[_x, _y].cellState != CellState.Full) return false;
        if (!_allMapObjectsByType[_map[_x, _y].objectType].Contains(_map[_x, _y].sceneObject)) return false;

        if(_allMapObjectsByType.ContainsKey(_map[_x, _y].objectType) && _allMapObjectsByType[_map[_x, _y].objectType].Count != 0)
            _allMapObjectsByType[_map[_x, _y].objectType].Remove(_map[_x, _y].sceneObject);
        
        Object.Destroy(_map[_x, _y].sceneObject);
        
        _map[_x, _y].cellState = CellState.Empty;
        pathfinding.SetWalkable(_x, _y, true);
        
        foreach (Vector2Int _tileCoord in _map[_x, _y].connectedCells)
        {
            if (_map[_tileCoord.x, _tileCoord.y].paddingSecurity > 0) {
                _map[_tileCoord.x, _tileCoord.y].paddingSecurity--;

                if (_map[_tileCoord.x, _tileCoord.y].paddingSecurity == 0)
                {
                    _map[_tileCoord.x, _tileCoord.y].cellState = CellState.Empty;
                    pathfinding.SetWalkable(_tileCoord.x, _tileCoord.y, true);
                }
                
                continue;
            }
            
            _map[_tileCoord.x, _tileCoord.y].cellState = CellState.Empty;
            pathfinding.SetWalkable(_tileCoord.x, _tileCoord.y, true);
        }
        return true;
    }

    public void DeleteMap() {
        for(int i = _objectsParent.childCount - 1; i >= 0; i--) {
            Object.Destroy(_objectsParent.GetChild(i).gameObject);
        }

        _map = new CellData[_map.GetLength(0), _map.GetLength(1)];
    }
    
    #endregion

    public CellData GetCellData(int _x, int _y) {
        if(IsCoordInMap(_x, _y)) return _map[_x, _y];

        return default;
    }

    public List<Vector2Int> GetConnectedCellsFull(int _x, int _y)
    {
        List<Vector2Int> returnList = new List<Vector2Int>();
        
        if (IsCoordInMap(_x, _y))
        {
            foreach (Vector2Int pos in _map[_x, _y].connectedCells)
            {
                if (GetCellData(pos.x, pos.y).cellState == CellState.Full)
                    returnList.Add(new Vector2Int(pos.x, pos.y));
            }
        }
        
        return returnList;
    }
    
    public void OnDrawGizmos()
    {
        if (!drawGizmo) return;
        if (!isMapGenerated) return;
        for (int i = 0; i < _map.GetLength(0); i++) {
            for (int j = 0; j < _map.GetLength(1); j++) {
                Color _color = Color.white;
                
                switch (_map[i, j].cellState) {
                    case CellState.Empty:
                        _color = Color.green;
                        break;
                    case CellState.Full:
                        _color = Color.red;
                        break;
                    case CellState.Padding:
                        _color = Color.yellow;
                        break;
                }
                Gizmos.color =  _color;
                Gizmos.DrawWireCube(new Vector3(i + 0.5f, 0, j + 0.5f),Vector3.one);
            }
        }
    }

    #region Checks
    
    private CellState IsSpaceOccupied(int _x, int _y) {
        if (!IsCoordInMap(_x, _y))
            return CellState.Undefined;
        
        return _map[_x, _y].cellState;
    }

    private bool IsCoordInMap(int _x, int _y) {
        if (_x < 0 || _y < 0 || _x > _map.GetLength(0) -1 || _y > _map.GetLength(1) -1)
            return false;
        return true;
    }
    
    public GameObject GetClosestMapObject(Vector3 _position, ObjectType _objectType)
    {
        GameObject _objToReturn = null;
        
        if (_allMapObjectsByType.ContainsKey(_objectType) && _allMapObjectsByType[_objectType].Count > 0)
        {
            float _minDistance = Vector3.Distance(_position, _allMapObjectsByType[_objectType][0].transform.position);
            _objToReturn = _allMapObjectsByType[_objectType][0];
            
            foreach (GameObject _obj in _allMapObjectsByType[_objectType])
            {
                float _distance = 0;
                if (_minDistance > (_distance = Vector3.Distance(_position, _obj.transform.position))) {
                    _minDistance = _distance;
                    _objToReturn = _obj;
                }
            }
        }
        return _objToReturn;
    }
    
    #endregion
}
