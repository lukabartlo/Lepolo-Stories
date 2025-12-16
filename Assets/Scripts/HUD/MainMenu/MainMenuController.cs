using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class MainMenuController : MonoBehaviour {

    public Action acquireCancelInput;
    public void AcquireCancelInput(InputAction.CallbackContext _ctx) {
        if (_ctx.started) {
            acquireCancelInput?.Invoke();
        }
    }
}
