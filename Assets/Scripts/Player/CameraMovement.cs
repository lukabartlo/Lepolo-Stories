using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMovement : MonoBehaviour
{

    public float camSpeed = 10;
    public float zoomSpeed = 10;
    private Vector2 moveInput;
    private Vector2 zoomInput;
    [SerializeField] Transform transformCam;
    private InputSystem_Actions cameraControl;
    private InputAction zoomAction;


    public float panSpeed = 6;
    private Camera _camera;

    private void Awake()
    {
        _camera = GetComponentInChildren<Camera>();
    }

    void Update()
    {
        //Vector2 panPosition = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        //transform.position += Quaternion.Euler(0, _camera.transform.eulerAngles.y, 0)
        //                    * new Vector3(panPosition.x, 0, panPosition.y)
        //                    * (panSpeed * Time.deltaTime);

        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y);
        transformCam.position += Quaternion.Euler(0, _camera.transform.eulerAngles.y, 0) * move * camSpeed * Time.deltaTime;

        Vector3 zoom = new Vector3(0, zoomInput.x, zoomInput.y);
        transformCam.position += Quaternion.Euler(0, _camera.transform.eulerAngles.y, 0) * zoom * zoomSpeed * Time.deltaTime;
         

    }

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {

    }

    public void moveCam(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void zoomCam(InputAction.CallbackContext context)
    {
        zoomInput = context.ReadValue<Vector2>();
    }
}