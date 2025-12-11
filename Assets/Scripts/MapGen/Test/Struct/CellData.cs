using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public struct CellData {
    public CellState cellState;
    public int paddingSecurity;
    public ObjectType objectType;
    public GameObject sceneObject;
    public List<Vector2Int> connectedCells;

    public bool isOrigin;
}