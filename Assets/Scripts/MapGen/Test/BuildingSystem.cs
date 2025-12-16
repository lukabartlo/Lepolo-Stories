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
        if (objectData.buildObjects.Count <= 0) return false;
        
        if (!_mapData.TryBuildingOnCell(_coordsToBuildAt, objectData)) return false;
        
        objectData.buildObject = Object.Instantiate(
            objectData.buildObjects[Random.Range(0, objectData.buildObjects.Count)],
            _buildParents);
        
        _mapData.PlaceObjectOnMap(_coordsToBuildAt, objectData, _objectToBuild);
        return true;
    }
}