using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class StartMenuController : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMP_InputField nameInputField;

    [Header("Scene")]
    [SerializeField] private string gameSceneName = "SampleScene";

    private const string PlayerNameKey = "PLAYER_NAME";

    private void Start()
    {
        // Load saved name if there is one
        if (PlayerPrefs.HasKey(PlayerNameKey))
        {
            nameInputField.text = PlayerPrefs.GetString(PlayerNameKey);
        }
        else
        {
            nameInputField.text = "Player";
        }
    }

    public void OnStartClick()
    {
        SavePlayerName();
        SceneManager.LoadScene(gameSceneName);
    }

    public void OnExitClick()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }

    public void OnNameChanged()
    {
        SavePlayerName();
    }

    private void SavePlayerName()
    {
        string playerName = nameInputField.text.Trim();

        if (string.IsNullOrEmpty(playerName))
        {
            playerName = "Player";
        }

        PlayerPrefs.SetString(PlayerNameKey, playerName);
        PlayerPrefs.Save();
    }

    public static string GetPlayerName()
    {
        return PlayerPrefs.GetString(PlayerNameKey, "Player");
    }
}