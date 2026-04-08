using UnityEngine;

public class WeaponSwitcher : MonoBehaviour
{
    public int selectedWeaponIndex = 0;
    
    // NEW: We need to talk to your PlayerAttack script
    public PlayerAttack playerAttackScript;

    void Start()
    {
        // If you didn't manually assign the script in the Inspector, find it on the parent (the Player)
        if (playerAttackScript == null)
        {
            playerAttackScript = GetComponentInParent<PlayerAttack>();
        }

        SelectWeapon(); 
    }

    void Update()
    {
        int previousSelectedWeapon = selectedWeaponIndex;

        // Number Keys
        if (Input.GetKeyDown(KeyCode.Alpha1) && transform.childCount >= 1)
            selectedWeaponIndex = 0;
        if (Input.GetKeyDown(KeyCode.Alpha2) && transform.childCount >= 2)
            selectedWeaponIndex = 1;
        if (Input.GetKeyDown(KeyCode.Alpha3) && transform.childCount >= 3)
            selectedWeaponIndex = 2;
        if (Input.GetKeyDown(KeyCode.Alpha4) && transform.childCount >= 4)
            selectedWeaponIndex = 3;

        // Mouse Scroll Wheel
        if (Input.GetAxis("Mouse ScrollWheel") > 0f) 
        {
            if (selectedWeaponIndex >= transform.childCount - 1)
                selectedWeaponIndex = 0; 
            else
                selectedWeaponIndex++;
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0f) 
        {
            if (selectedWeaponIndex <= 0)
                selectedWeaponIndex = transform.childCount - 1; 
            else
                selectedWeaponIndex--;
        }

        if (previousSelectedWeapon != selectedWeaponIndex)
        {
            SelectWeapon();
        }
    }

    void SelectWeapon()
    {
        int i = 0;
        foreach (Transform weapon in transform)
        {
            if (i == selectedWeaponIndex)
            {
                weapon.gameObject.SetActive(true);  // Turn ON the weapon
                
                // NEW: Grab the specific Weapon component (Melee or Gun) from this object
                // and assign it to the PlayerAttack's active slot!
                if (playerAttackScript != null)
                {
                    playerAttackScript.activeWeapon = weapon.GetComponent<Weapon>();
                }
            }
            else
            {
                weapon.gameObject.SetActive(false); // Turn OFF all others
            }
            i++;
        }
    }
}