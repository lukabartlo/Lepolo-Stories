using UnityEngine;

public class CameraZoom_2 : MonoBehaviour
{

    private Camera _camera;

    private void Awake()
    {
        //_camera = GetComponentInChildren<Camera>();
    }

    void Update()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            GetComponent<Transform>().position = new Vector3(transform.position.x,
                                                             transform.position.y - 0.3f * Time.deltaTime,
                                                             transform.position.z); 
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            GetComponent<Transform>().position = new Vector3(transform.position.x,
                                                             transform.position.y + 0.3f * Time.deltaTime,
                                                             transform.position.z);
        }
    }
}