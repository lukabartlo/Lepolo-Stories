using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public float currentMana { get; set; }
    [SerializeField] private float minMana = 0f;
    [SerializeField] private float maxMana = 10f;
    [SerializeField] private float coef = 0.6f;



    [SerializeField] private TextMeshProUGUI TMPcurrentMana;

    private void Awake()
    {
        currentMana = minMana;
    }

    private void Update()
    {
        TMPcurrentMana.text = Mathf.Round(currentMana) + "/" + maxMana;

        currentMana += coef * Time.deltaTime;

        if (currentMana < minMana) {
            currentMana = minMana;
        }
        if (currentMana > maxMana) {
            currentMana = maxMana;
        }
    }
}
