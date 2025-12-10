using UnityEngine;

public class AoeEffectFollowMouse : MonoBehaviour
{

    private Camera camera;
    [SerializeField] GameObject aoeFeedbackObject;

    private void Update()
    {
        FollowMousePosition();
    }

    private void FollowMousePosition()
    {
        transform.position = GetPositionFromMouse();
    }

    private Vector2 GetPositionFromMouse()
    {
        return camera.ScreenToWorldPoint(Input.mousePosition);
    }
}
