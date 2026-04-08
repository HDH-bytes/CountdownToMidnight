using UnityEngine;
using UnityEngine.SceneManagement;

public class WeaponSwitcher : MonoBehaviour
{
    public int selectedWeaponIndex = 0;
    public PlayerAttack playerAttackScript;
    private int currentLevelIndex;

    void Start()
    {
        if (playerAttackScript == null)
            playerAttackScript = GetComponentInParent<PlayerAttack>();

        // Grab the Build Index of the current active scene
        currentLevelIndex = SceneManager.GetActiveScene().buildIndex;

        SelectWeapon(); 
    }

    void Update()
    {
        int previousSelectedWeapon = selectedWeaponIndex;

        // Number Keys
        if (Input.GetKeyDown(KeyCode.Alpha1)) TrySelectWeapon(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) TrySelectWeapon(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) TrySelectWeapon(2);
        if (Input.GetKeyDown(KeyCode.Alpha4)) TrySelectWeapon(3);

        // Mouse Scroll Wheel
        if (Input.GetAxis("Mouse ScrollWheel") > 0f) 
            ScrollWeapon(1);
        if (Input.GetAxis("Mouse ScrollWheel") < 0f) 
            ScrollWeapon(-1);

        if (previousSelectedWeapon != selectedWeaponIndex)
        {
            SelectWeapon();
        }
    }

    void TrySelectWeapon(int index)
    {
        if (index >= transform.childCount) return; 

        Weapon weaponToCheck = transform.GetChild(index).GetComponent<Weapon>();
        
        if (weaponToCheck != null && currentLevelIndex >= weaponToCheck.unlockSceneIndex)
        {
            selectedWeaponIndex = index;
        }
    }

    void ScrollWeapon(int direction)
    {
        int checkIndex = selectedWeaponIndex;

        for (int i = 0; i < transform.childCount; i++)
        {
            checkIndex += direction;

            if (checkIndex >= transform.childCount) checkIndex = 0;
            if (checkIndex < 0) checkIndex = transform.childCount - 1;

            Weapon weaponToCheck = transform.GetChild(checkIndex).GetComponent<Weapon>();
            
            if (weaponToCheck != null && currentLevelIndex >= weaponToCheck.unlockSceneIndex)
            {
                selectedWeaponIndex = checkIndex;
                return; 
            }
        }
    }

    void SelectWeapon()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform weaponTransform = transform.GetChild(i);

            if (i == selectedWeaponIndex)
            {
                weaponTransform.gameObject.SetActive(true);  
                
                if (playerAttackScript != null)
                {
                    playerAttackScript.activeWeapon = weaponTransform.GetComponent<Weapon>();
                }
            }
            else
            {
                weaponTransform.gameObject.SetActive(false); 
            }
        }
    }
}