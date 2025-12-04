using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerationSystem {
    
    #region Variables
    
    private BuildingSystem _buildingSystem;
    private Vector2Int _mapSize;
    
    Coroutine mapGenerationCoroutine = null;
    
    #endregion
    
    public MapGenerationSystem(ref BuildingSystem _buildingSystem, Vector2Int _mapSize) {
        this._buildingSystem =  _buildingSystem;
        this._mapSize = _mapSize;
    }
    
    //Generating the first map, not relevant if a save is added
    public bool PreGenerateMap(List<PreGenObjectWrapper> _objectsToPregen) {
        if (_objectsToPregen.Count == 0) return false;
    
        // mapGenerationCoroutine ??= TestBuildManager.Instance.StartChildCoroutine(mapGeneration(_objectsToPregen));
        
        #region Altar Pregen
        if (_objectsToPregen[0].objectType == ObjectType.Building) {
            if (_buildingSystem.TryBuild(_objectsToPregen[0].objectType,
                    new Vector2Int(_mapSize.x / 2, _mapSize.y / 2)) == false) {
                Debug.LogError("Center Autel Generation Failed Atrociously");
            }
            
            _objectsToPregen.RemoveAt(0);
        }
        #endregion
        
        #region Rest of the pregen
        
        foreach (PreGenObjectWrapper obj in _objectsToPregen) {
            for (int i = 0; i < obj.onPreGenAmountToSpawn; i++) {
                Vector2Int _coordsToTryAt = new Vector2Int(Random.Range(0, _mapSize.x-1), Random.Range(0, _mapSize.y-1));
                if (_buildingSystem.TryBuild(obj.objectType, _coordsToTryAt)) {
                    continue;
                }
        
                i--;
            }
        }
        #endregion
    
        return true;
    }
    
    #region Possibilities

    // private IEnumerator mapGeneration(List<PreGenObjectWrapper> _objectsToPregen) {
    //     if (_objectsToPregen[0].objectType == ObjectType.Building) {
    //         if (_buildingSystem.TryBuild(_objectsToPregen[0].objectType,
    //                 new Vector2Int(_mapSize.x / 2, _mapSize.y / 2)) == false) {
    //             Debug.LogError("Center Autel Generation Failed Atrociously");
    //         }
    //     }
    //
    //     if (_objectsToPregen.Count > 1) {
    //         yield return new WaitForSeconds(0.02f);
    //         foreach (PreGenObjectWrapper obj in _objectsToPregen) {
    //             for (int i = 1; i < obj.onPreGenAmountToSpawn; i++) {
    //                 Vector2Int _coordsToTryAt = new Vector2Int(Random.Range(0, _mapSize.x-1), Random.Range(0, _mapSize.y-1));
    //                 if (_buildingSystem.TryBuild(obj.objectType, _coordsToTryAt)) {
    //                     yield return new WaitForSeconds(0.02f);
    //                     continue;
    //                 }
    //
    //                 i--;
    //             }
    //         }
    //     }
    //     
    //     mapGenerationCoroutine = null;
    // }
    
    #endregion
}
