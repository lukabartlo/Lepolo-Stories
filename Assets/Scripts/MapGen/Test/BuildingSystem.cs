using UnityEngine;
using Object = UnityEngine.Object;

public class BuildingSystem {
    
    #region Variables
    
    private MapData _mapData;
    private TestBuildProgression _progressionSystem;
    private Transform _buildParents;
    
    // public Vector3 mapSize;

    // private Dictionary<ObjectType, ObjectData> _allPlacableObjects;
    // public List<ObjectWrapper> objects = new();
    
    #endregion

    public BuildingSystem(ref MapData _mapData, ref TestBuildProgression _progressionSystem, ref Transform _buildParents) {
        this._mapData = _mapData;
        this._progressionSystem = _progressionSystem;
        this._buildParents = _buildParents;
    }
    
    public bool TryBuild(ObjectType _objectToBuild, Vector2Int _coordsToBuildAt) {
        ObjectData objectData = _progressionSystem.GetObjectByType(_objectToBuild);
        if (objectData.buildObject == null) return false;
        
        Vector2Int _realCoordsToStartBuildingAt = _coordsToBuildAt - new Vector2Int(objectData.widthX / 2, objectData.widthY / 2);
        
        if (!_mapData.TryBuildingOnCell(_realCoordsToStartBuildingAt, objectData)) return false;
        
        objectData.buildObject = Object.Instantiate(objectData.buildObject, _buildParents);
        _mapData.PlaceObjectOnMap(_realCoordsToStartBuildingAt, ref objectData, _objectToBuild);
        return true;
    }
}