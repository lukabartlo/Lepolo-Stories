using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerationSystem {
    
    #region Variables
    
    private BuildingSystem _buildingSystem;
    private Vector2Int _mapSize;

    private int _firstGenChatpelleOffset = 13;
    
    Coroutine mapGenerationCoroutine = null;
    
    #endregion
    
    public MapGenerationSystem(ref BuildingSystem _buildingSystem, Vector2Int _mapSize) {
        this._buildingSystem =  _buildingSystem;
        this._mapSize = _mapSize;
    }
    
    //Generating the first map, not relevant if a save is added
    public bool RandomMapGeneration(List<PreGenObjectWrapper> _objectsToPregen) {
        if (_objectsToPregen.Count <= 0) return false;
    
        // mapGenerationCoroutine ??= TestBuildManager.Instance.StartChildCoroutine(mapGeneration(_objectsToPregen));
        
        #region Static Pregen
        if (_objectsToPregen[0].objectType == ObjectType.Altar) {
            if (_buildingSystem.TryBuild(_objectsToPregen[0].objectType,
                    new Vector2Int(_mapSize.x / 2, _mapSize.y / 2)) == false) {
                Debug.LogError("Center Autel Generation Failed Atrociously");
            }
        }
        
        if (_objectsToPregen.Count <= 1) return false;
        
        if (_objectsToPregen[1].objectType == ObjectType.Chatpelle) {
            if (_buildingSystem.TryBuild(_objectsToPregen[1].objectType,
                    new Vector2Int(_mapSize.x / 2, _mapSize.y / 2 + _firstGenChatpelleOffset)) == false) {
                Debug.LogError("Center Autel Generation Failed Atrociously");
            }
        }
        #endregion
        
        #region Rest of the pregen
        
        foreach (PreGenObjectWrapper obj in _objectsToPregen) {
            for (int i = 2; i < obj.onPreGenAmountToSpawn; i++) {
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

    public bool SavedMapGeneration(List<MapSaveWrapper> _objectsToGenerate) {
        foreach (MapSaveWrapper obj in _objectsToGenerate) {
            if (!_buildingSystem.TryBuild(obj.savedObject, obj.savedObjectCoords)) {
                Debug.LogError("Map Generation Failed Atrociously");
                return false;
            }
        }
        
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
