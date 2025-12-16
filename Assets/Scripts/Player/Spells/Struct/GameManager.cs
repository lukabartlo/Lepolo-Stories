using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour 
{
    public float currentMana { get; set; }
    [SerializeField] private float minMana = 0f;
    [SerializeField] private float maxMana = 100f;
    //[SerializeField] private float coef = 0.6f;

    public static GameManager Instance;

    [SerializeField] private TextMeshProUGUI TMPcurrentMana;

    private void Awake()
    {
        Instance = this;
        currentMana = minMana;
    }

    private void Update()
    {
        TMPcurrentMana.text = Mathf.Round(currentMana) + "/" + maxMana;

        //currentMana += coef * Time.deltaTime;

        if (currentMana < minMana) {
            currentMana = minMana;
        }
        if (currentMana > maxMana) {
            currentMana = maxMana;
        }
    }
}
