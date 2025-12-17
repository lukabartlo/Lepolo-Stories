using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraTestPerso : MonoBehaviour
{
    private Transform _cameraPivot;

    [Header("Movements")]
    private Vector2 _cameraInput;
    private Vector3 _cameraDesiredDirection = Vector3.zero;
    [SerializeField] private float moveSpeed;

    [Header("Zoom")]
    [SerializeField] private float zoomMin;
    [SerializeField] private float zoomMax;
    [SerializeField] private float zoomSpeed;
    [SerializeField] private float zoomStep;
    private Coroutine _zoomCoroutine;
    private float _zoomDirection;
    private Vector3 _desiredLocalPosition;
    private Vector2 _bounds;

    void Start() {
        _cameraPivot = transform.parent;
        _desiredLocalPosition = new Vector3(0, 10, -15);
        _bounds = TestBuildManager.Instance.MapSize;
    }
    private void Update()
    {
        CameraZoom();
        if (_cameraInput == Vector2.zero) return;
        CameraMove();
    }


    private void CameraMove()
    {
        _cameraDesiredDirection.x = _cameraInput.x;
        _cameraDesiredDirection.z = _cameraInput.y;

        Vector3 _cameraDesiredPosition = _cameraPivot.position + _cameraDesiredDirection;


        _cameraDesiredPosition.x = Mathf.Clamp(_cameraDesiredPosition.x, 0, _bounds.x);

    
        Transform cam = transform; 

        float cameraLocalZ = cam.localPosition.z; 
        float zoomOffsetZ = -cameraLocalZ * 0.65f; 

        float minZ = zoomOffsetZ;
        float maxZ = _bounds.y ;

        _cameraDesiredPosition.z = Mathf.Clamp(_cameraDesiredPosition.z,minZ,maxZ);

        _cameraPivot.position = Vector3.Lerp(_cameraPivot.position,_cameraDesiredPosition,moveSpeed * Time.fixedDeltaTime);
    }

    private void CameraZoom()
    {
        if (_zoomDirection != 0)
        {
            float _cameraDistanceAfterZoom = Mathf.Clamp(
                Vector3.Distance(_cameraPivot.position, transform.position + transform.forward * (_zoomDirection * zoomStep)),
                zoomMin, zoomMax);

            _desiredLocalPosition = transform.forward * -_cameraDistanceAfterZoom;
        }
        CameraMove();
        transform.localPosition = Vector3.Lerp(transform.localPosition, _desiredLocalPosition, zoomSpeed * Time.deltaTime);
        
    }

    public void AcquireCameraMoveInputs(InputAction.CallbackContext _ctx)
    {
        _cameraInput = _ctx.ReadValue<Vector2>().normalized;
    }

    public void AcquireCameraZoomInputs(InputAction.CallbackContext _ctx)
    {
        _zoomDirection = _ctx.ReadValue<float>();
    }
}