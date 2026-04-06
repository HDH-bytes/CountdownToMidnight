using UnityEngine;
using UnityEngine.SceneManagement;

public class ChooseCharacterController : MonoBehaviour
{
    public void ChooseCharacter1()
    {
        PlayerPrefs.SetInt("SelectedCharacter", 0);
        PlayerPrefs.Save();
        StartNewGame();
    }

    public void ChooseCharacter2()
    {
        PlayerPrefs.SetInt("SelectedCharacter", 1);
        PlayerPrefs.Save();
        StartNewGame();
    }

    public void ChooseCharacter3()
    {
        PlayerPrefs.SetInt("SelectedCharacter", 2);
        PlayerPrefs.Save();
        StartNewGame();
    }

    // Reset hearts to 10 whenever the player starts a fresh game from the character screen
    private void StartNewGame()
    {
        EnsureLifeManager();
        LifeManager.Instance.ResetLives();
        SceneManager.LoadScene("Level0");
    }

    private static void EnsureLifeManager()
    {
        if (LifeManager.Instance == null)
            new GameObject("LifeManager").AddComponent<LifeManager>();
    }
}