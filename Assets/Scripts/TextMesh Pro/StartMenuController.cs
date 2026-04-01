using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; // Required for TextMeshPro UI elements

public class StartMenuController : MonoBehaviour
{
    [Header("UI Elements")]
   
    public TMP_InputField nameInputField; 

    public void OnStartClick()
    {
        
        string playerName = nameInputField.text;

        
        if (string.IsNullOrWhiteSpace(playerName))
        {
            playerName = "Player1";
        }

        
        PlayerPrefs.SetString("SavedPlayerName", playerName);
        PlayerPrefs.Save();

        
        SceneManager.LoadScene("ChooseCharacter");
    }

    public void OnExitClick()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
}