using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerRotation : Rotator
{    
    private Camera mainCam;

    private void Start()
    {
        mainCam = Camera.main;
    }

    // 1. Made public so the Inspector can see it
    // 2. Changed from InputValue to InputAction.CallbackContext
    public void OnLook(InputAction.CallbackContext context)
    {
        // 3. Read the value from the context instead of InputValue
        Vector2 mouseScreenPosition = context.ReadValue<Vector2>();

        // The rest of your logic remains exactly the same!
        Vector3 mouseWorldPosition = mainCam.ScreenToWorldPoint(mouseScreenPosition);
        mouseWorldPosition.z = 0f; 

        LookAt(mouseWorldPosition);
    }
}