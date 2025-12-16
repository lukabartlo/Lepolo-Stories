using UnityEngine;
using UnityEngine.InputSystem;

public class UIController : MonoBehaviour {
    public void OnEscapeKey(InputAction.CallbackContext _ctx) {
        UIManager.Instance.HandleEscape();
    }
}
