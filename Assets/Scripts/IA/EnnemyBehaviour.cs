using TMPro;
using UnityEngine;

public class EnnemyBehaviour : MonoBehaviour
{

    private Vector3 ennemyPos = new Vector3(1, 0, 0);
    [SerializeField] private float speed = 20f;

    [SerializeField] private int currentMadness = 50;
    [SerializeField] private int maxMadness = 100;
    [SerializeField] private int minMadness = 0;

    [SerializeField] private TextMeshProUGUI TMPcurrentMadness;

    private int i = 0;

    void Start()
    {
        ennemyPos = new Vector3(1, 0, 0);
    }

    void Update()
    {
        transform.position = transform.position + ennemyPos * speed * Time.deltaTime;
        TMPcurrentMadness.text = Mathf.Round(currentMadness) + "/" + maxMadness;

        i++;
        if (i >= 180 && i < 360) {
            ennemyPos = new Vector3(-1, 0, 0);
        }
        if (i >= 360) {
            ennemyPos = new Vector3(1, 0, 0);
            i = 0;
        }

        if (currentMadness < minMadness)
        {
            currentMadness = minMadness;
        }
        if (currentMadness > maxMadness)
        {
            currentMadness = maxMadness;
        }
    }
}
