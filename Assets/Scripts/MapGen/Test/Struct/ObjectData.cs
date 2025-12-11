using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public struct ObjectData {
    public List<BuildingCells> cellsOffsetFromOrigin;
    public List<GameObject> buildObjects;
    [HideInInspector]
    public GameObject buildObject;
}