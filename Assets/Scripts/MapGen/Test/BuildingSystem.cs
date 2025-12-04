using UnityEngine;

public class BuildingSystem : MonoBehaviour
{
    private MapData mapData;

    BuildingSystem(MapData _mapData)
    {
        mapData = _mapData;
    }

    // public bool TryBuild(ObjectType _objectToBuild, Vector2Int _coordsToBuildAt)
    // {
    //     //ObjectData objectData = ProgressionSystem.AllPlacableObjects[_objectToBuild];
    //     // recup object data
    //     // check si il peut créer à l'endroit
    //     
    //     // build = Instantiate()
    //     // objectData.buildObject = build;
    //     
    //     //mapData.PlaceObjectOnMap(_coordsToBuildAt, objectData, _objectToBuild);
    // }
}