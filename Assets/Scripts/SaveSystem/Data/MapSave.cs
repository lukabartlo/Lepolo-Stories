using System.Collections.Generic;
using UnityEngine;

public class MapSave : GameData {
    public List<MapSaveWrapper> _savedObjects;
    
    public List<MapSaveWrapper> SavedObjects => _savedObjects;

    public MapSave(MapData _mapData) {
        _savedObjects = new List<MapSaveWrapper>();
        
        for (int i = 0; i < _mapData.Map.GetLength(0); i++) {
            for (int j = 0; j < _mapData.Map.GetLength(1); j++) {
                if (!_mapData.Map[i, j].isOrigin) continue;
                
                _savedObjects.Add(new MapSaveWrapper {
                        savedObject = _mapData.Map[i, j].objectType,
                        savedObjectCoords = new Vector2Int(i, j),
                    });
            }
        }
    }
}
