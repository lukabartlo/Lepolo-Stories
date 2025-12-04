using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public class MapData {
    
    #region Variables
    
    private CellData[,] _map;
    private Dictionary<ObjectType, List<GameObject>> _allMapObjectsByType =  new Dictionary<ObjectType, List<GameObject>>();
    private float _mapHeight;
    private Transform _objectsParent;
    public bool isMapGenerated  = false;
    
    #endregion

    public MapData(int _sizeX, int _sizeY, float _mapHeight, Transform _objectsParent, GameObject _squarePrefab) {
        _map =  new CellData[_sizeX , _sizeY];
        this._mapHeight = _mapHeight;
        this._objectsParent = _objectsParent;
        isMapGenerated = true;

    }
    
    #region Placing On Cells
    
    public bool TryBuildingOnCell(Vector2Int _buildCoord, ObjectData _objectData)
    {
        int _widthX = _buildCoord.x + _objectData.widthX - _objectData.padding;
        int _widthY = _buildCoord.y + _objectData.widthY - _objectData.padding;

        for (int i = 0; _buildCoord.x + i < _widthX; i++ )
        {
            for (int j = 0;  _buildCoord.y + j < _widthY; j++)
            {
                if (!IsCoordInMap(_buildCoord.x + i, _buildCoord.y + j)) return false;
                
                // If the cell on the map is full we don't want to build
                if (_map[_buildCoord.x + i, _buildCoord.y + j].cellState == CellState.Full) {
                    return false;
                }
            
                // If the cell to build is not a padding and the cell on the map is a padding then we don't want to build
                if (!(_buildCoord.x + i < _buildCoord.x + _objectData.padding || _buildCoord.y + j < _buildCoord.y + _objectData.padding || _buildCoord.x + i >= _widthX || _buildCoord.y + j >= _widthY) 
                    &&  _map[_buildCoord.x + i, _buildCoord.y + j].cellState == CellState.Padding) return false;
            }
        }
        
        return true;
    }
    
    public void PlaceObjectOnMap(Vector2Int _buildCoord, ref ObjectData _objectData, ObjectType _objectType)
    {
        int _widthX = _buildCoord.x + _objectData.widthX;
        int _widthY = _buildCoord.y + _objectData.widthY;
        
        List<Vector2Int> _cellsToBuild = new List<Vector2Int>();

        Vector2Int _coordCellToBuild = Vector2Int.zero;
        for (int i = 0; _buildCoord.x + i < _widthX; i++ ) {
            for (int j = 0;  _buildCoord.y + j < _widthY; j++) {
                
                _coordCellToBuild.x = _buildCoord.x + i;
                _coordCellToBuild.y = _buildCoord.y + j;
                _cellsToBuild.Add(_coordCellToBuild);
                
                // If is padding
                if (_buildCoord.x + i < _buildCoord.x + _objectData.padding ||
                    _buildCoord.y + j < _buildCoord.y + _objectData.padding || 
                    _buildCoord.x + i >= _widthX  - _objectData.padding ||
                    _buildCoord.y + j >= _widthY  - _objectData.padding)
                {
                    _map[_buildCoord.x + i, _buildCoord.y + j].cellState = CellState.Padding;
                    _map[_buildCoord.x + i, _buildCoord.y + j].paddingSecurity++;
                    continue;
                }
                
                // if building
                _map[_buildCoord.x + i, _buildCoord.y + j].cellState = CellState.Full;
            }
        }

        // Add the connected cells to the building's cell
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
            _cellOnGrid.sceneObject.transform.position = new Vector3(_buildCoord.x + (float)_objectData.widthX / 2, _mapHeight, _buildCoord.y + (float)_objectData.widthY / 2);
            
        }

        if (_allMapObjectsByType.ContainsKey(_objectType)) {
            _allMapObjectsByType[_objectType].Add(_objectData.buildObject);
        } else {
            _allMapObjectsByType.Add(_objectType, new List<GameObject> { _objectData.buildObject });
        }
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
        
        foreach (Vector2Int _tileCoord in _map[_x, _y].connectedCells)
        {
            if (_map[_tileCoord.x, _tileCoord.y].paddingSecurity > 0) {
                _map[_tileCoord.x, _tileCoord.y].paddingSecurity--;
                
                if (_map[_tileCoord.x, _tileCoord.y].paddingSecurity == 0)
                    _map[_tileCoord.x, _tileCoord.y].cellState = CellState.Empty;
                
                continue;
            }
            
            _map[_tileCoord.x, _tileCoord.y].cellState = CellState.Empty;
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
    public void OnDrawGizmos()
    {
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
