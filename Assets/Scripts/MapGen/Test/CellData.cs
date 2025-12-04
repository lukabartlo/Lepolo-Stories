using UnityEngine;
using System.Collections.Generic;

public struct CellData {
    public CellState cellState;
    public int paddingSecurity;
    public ObjectType objectType;
    public GameObject sceneObject;
    public List<Vector2Int> connectedCells;
}