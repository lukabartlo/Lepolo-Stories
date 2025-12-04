using TMPro;
using UnityEngine;

public class TestBuildManagerWindow : MonoBehaviour
{
    public TextMeshProUGUI stateText;
    public Transform textParent;
    public GameObject textPrefab;
    
    public void UpdateDisplay(CellData _cellData) {
        stateText.text = _cellData.cellState.ToString();

        for (int i = textParent.childCount - 1; i >= 0; i--) {
            Destroy(textParent.GetChild(i).gameObject);
        }

        foreach (Vector2Int vector in _cellData.connectedCells) {
            Instantiate(textPrefab, textParent).GetComponent<TextMeshProUGUI>().text = vector.ToString();
        }
    }
}
