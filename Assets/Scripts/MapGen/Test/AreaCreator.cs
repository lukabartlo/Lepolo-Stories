using System.Collections.Generic;
using UnityEngine;

public class AreaCreator : MonoBehaviour {
    public GameObject objectPrefab;
    private GameObject _previousObject;
    
    public List<BuildingCells> cellsOffsetFromOrigin;

    public void UpdateVisuals() {
        if (_previousObject != null) {
            if (_previousObject != objectPrefab) {
                DestroyImmediate(_previousObject);
            }
        }

        GameObject _object = null;
        if (objectPrefab != null) {
            _object = Instantiate(objectPrefab, transform);
            _object.transform.position +=  new Vector3(0.5f, 0, 0.5f);
        }
        _previousObject = _object;
    }

    private void OnDrawGizmos() {
        
        foreach (BuildingCells cell in cellsOffsetFromOrigin) {
            Color _color = Color.white;
                
            switch (cell.cellState) {
                case CellState.Full:
                    _color = Color.red;
                    break;
                case CellState.Padding:
                    _color = Color.yellow;
                    break;
            }
            
            Gizmos.color =  _color;
            Gizmos.DrawCube(new Vector3(cell.offsetFromOrigin.x + 0.5f, 0, cell.offsetFromOrigin.y + 0.5f),Vector3.one);
        }
    }
}