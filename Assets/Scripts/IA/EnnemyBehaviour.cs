using UnityEngine;

public class EnnemyBehaviour : MonoBehaviour
{

    private Vector3 ennemyPos = new Vector3(1, 0, 0);
    [SerializeField] private float speed = 20f;

    private int i = 0;

    void Start()
    {
        ennemyPos = new Vector3(1, 0, 0);
    }

    void Update()
    {
        transform.position = transform.position + ennemyPos * speed * Time.deltaTime;

        i++;
        if (i >= 180 && i < 360) {
            ennemyPos = new Vector3(-1, 0, 0);
        }
        if (i >= 360) {
            ennemyPos = new Vector3(1, 0, 0);
            i = 0;
        }
    }
}
