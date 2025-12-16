using System;
using UnityEngine;

[Serializable]
public struct MapSaveWrapper {
    public ObjectType savedObject;
    public Vector2Int savedObjectCoords;
}