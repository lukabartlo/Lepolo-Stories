using UnityEngine;
using UnityEngine.InputSystem;

public class UIController : MonoBehaviour {
    public void OnEscapeKey(InputAction.CallbackContext _ctx) {
        UIManager.Instance.HandleEscape();
    }

    public void EnableInputs(bool _enable) {
        TryGetComponent(out PlayerInput playerInput);
        if (playerInput != null) {
            playerInput.enabled = _enable;
        }
    }
}
