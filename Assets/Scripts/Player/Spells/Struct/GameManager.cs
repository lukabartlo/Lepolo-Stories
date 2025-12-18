using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour 
{
    public float currentMana { get; set; }
    [SerializeField] private float minMana = 0f;
    [SerializeField] private float maxMana = 100f;

    public static GameManager Instance;

    private void Awake()
    {
        Instance = this;
        currentMana = maxMana/2;
    }

    private void Update()
    {
        if (currentMana < minMana) {
            currentMana = minMana;
        }
        if (currentMana > maxMana) {
            currentMana = maxMana;
        }
        
        InGameHUD.Instance.SetMana(currentMana, maxMana);
    }
}
