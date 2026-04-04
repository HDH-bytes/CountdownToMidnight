using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    // This will hold whichever weapon is currently active
    public Weapon activeWeapon; 

    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.started && activeWeapon != null)
        {
            // Attack with the current weapon
            activeWeapon.Attack();
        }
    }
}
