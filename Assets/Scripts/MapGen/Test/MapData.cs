using UnityEngine;
using System.Collections.Generic;

public class MapData : MonoBehaviour
{
    private CellData[,] map;
    private Dictionary<ObjectType, List<GameObject>> allMapObjectsByType;

    MapData(int Width, int Height)
    {
        map =  new CellData[Width,Height];
    }

    private CellState IsSpaceOccupied(int _x, int _y)
    {
        if (_x < 0 || _y < 0 || _x >= map.GetLength(0) || _y >= map.GetLength(1))
            return CellState.Undefined;
        
        return map[_x, _y].cellState;
    }

    public bool TryBuildingOnCell(Vector2Int _buildCoord, ObjectData objectData)
    {
        int _width = _buildCoord.x + objectData.width - objectData.padding;
        int _height = _buildCoord.y + objectData.height - objectData.padding;

        for (int i = 0; _buildCoord.x + i < _width; i++ )
        {
            for (int j = 0;  _buildCoord.y + j < _height; j++)
            {
                // If the cell on the map is full we don't want to build
                if (map[_buildCoord.x + i, _buildCoord.y + j].cellState == CellState.Full) return false;
            
                // If the cell to build is not a padding and the cell on the map is a padding then we don't want to build
                if (!(_buildCoord.x + i < _buildCoord.x + objectData.padding || _buildCoord.y + j < _buildCoord.y + objectData.padding || _buildCoord.x + i >= _width || _buildCoord.y + j >= _height) 
                    &&  map[_buildCoord.x + i, _buildCoord.y + j].cellState == CellState.Padding) return false;
            }
        }
        
        return true;
    }

    public void PlaceObjectOnMap(Vector2Int _buildCoord, ObjectData objectData,ObjectType objectType)
    {
        int _width = _buildCoord.x + objectData.width - objectData.padding;
        int _height = _buildCoord.y + objectData.height - objectData.padding;
        
        List<Vector2Int> cellsToBuild = new List<Vector2Int>();

        for (int i = 0; _buildCoord.x + i < _width; i++ )
        {
            for (int j = 0;  _buildCoord.y + j < _height; j++)
            {
                cellsToBuild.Add(new Vector2Int(_buildCoord.x + i, _buildCoord.y + j));
                
                // If is padding
                if (_buildCoord.x + i < _buildCoord.x + objectData.padding ||
                    _buildCoord.y + j < _buildCoord.y + objectData.padding || 
                    _buildCoord.x + i >= _width ||
                    _buildCoord.y + j >= _height)
                {
                    map[_buildCoord.x + i, _buildCoord.y + j].cellState = CellState.Padding;
                    map[_buildCoord.x + i, _buildCoord.y + j].paddingSecurity++;
                    continue;
                }
                
                // if building
                map[_buildCoord.x + i, _buildCoord.y + j].cellState = CellState.Full;
            }
        }
        
        // Add the connected cells to the building's cell
        foreach (Vector2Int cellToBuild in cellsToBuild)
        {
            if (map[cellToBuild.x, cellToBuild.y].cellState != CellState.Full)
                return;
                
            map[cellToBuild.x, cellToBuild.y].connectedCells = cellsToBuild;
            map[cellToBuild.x, cellToBuild.y].connectedCells.Remove(cellToBuild);
            map[cellToBuild.x, cellToBuild.y].objectType = objectType;
            map[cellToBuild.x, cellToBuild.y].sceneObject = objectData.buildObject;

        }
        
        ////////////////////////////////////////////AJOUTER AU DICTIONNAIRE CORRESPONDANT/////////////////////////////////////
        ///////////////////////////////////////////////AJOUTER AU DICTIONNAIRE CORRESPONDANT/////////////////////////////////////
        ////////////////////////////////////////////AJOUTER AU DICTIONNAIRE CORRESPONDANT/////////////////////////////////////
        ////////////////////////////////////////////AJOUTER AU DICTIONNAIRE CORRESPONDANT/////////////////////////////////////
        ////////////////////////////////////////////AJOUTER AU DICTIONNAIRE CORRESPONDANT/////////////////////////////////////
        ////////////////////////////////////////////AJOUTER AU DICTIONNAIRE CORRESPONDANT/////////////////////////////////////
        ////////////////////////////////////////////AJOUTER AU DICTIONNAIRE CORRESPONDANT/////////////////////////////////////
        
    }

    public bool TryDeleteCell(int _x, int _y)
    {
        if (map[_x, _y].cellState == CellState.Full) return false;
        
        if (!allMapObjectsByType[map[_x, _y].objectType].Contains(map[_x, _y].sceneObject)) return false;

        allMapObjectsByType[map[_x, _y].objectType].Remove(map[_x, _y].sceneObject);
        Destroy(map[_x, _y].sceneObject);
        
        map[_x, _y].cellState = CellState.Empty;
        
        foreach (Vector2Int tileCoord in map[_x, _y].connectedCells)
        {
            if (map[tileCoord.x, tileCoord.y].paddingSecurity > 0) {
                map[tileCoord.x, tileCoord.y].paddingSecurity--;
                
                if (map[tileCoord.x, tileCoord.y].paddingSecurity == 0)
                    map[tileCoord.x, tileCoord.y].cellState = CellState.Empty;
                
                continue;
            }
            
            map[tileCoord.x, tileCoord.y].cellState = CellState.Empty;
        }
        
        ////////////////////////////////////////////SUPPRIMER AU DICTIONNAIRE CORRESPONDANT/////////////////////////////////////
        ///////////////////////////////////////////////SUPPRIMER AU DICTIONNAIRE CORRESPONDANT/////////////////////////////////////
        ////////////////////////////////////////////SUPPRIMER AU DICTIONNAIRE CORRESPONDANT/////////////////////////////////////
        ////////////////////////////////////////////SUPPRIMER AU DICTIONNAIRE CORRESPONDANT/////////////////////////////////////
        ////////////////////////////////////////////SUPPRIMER AU DICTIONNAIRE CORRESPONDANT/////////////////////////////////////
        ////////////////////////////////////////////SUPPRIMER AU DICTIONNAIRE CORRESPONDANT/////////////////////////////////////
        ////////////////////////////////////////////SUPPRIMER AU DICTIONNAIRE CORRESPONDANT/////////////////////////////////////
        
        return true;
    }

    public GameObject GetClosestMapObject(Vector3 position, ObjectType _objectType)
    {
        GameObject objToReturn = null;
        
        if (allMapObjectsByType.ContainsKey(_objectType) && allMapObjectsByType[_objectType].Count > 0)
        {
            float minDistance = Vector3.Distance(position, allMapObjectsByType[_objectType][0].transform.position);
            
            foreach (GameObject obj in allMapObjectsByType[_objectType])
            {
                float distance = 0;
                if (minDistance > (distance = Vector3.Distance(position, obj.transform.position)))
                    minDistance = distance;
            }
        }
        
        return objToReturn;
    }
}
