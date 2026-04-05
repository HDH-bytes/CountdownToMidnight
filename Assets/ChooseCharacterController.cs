using UnityEngine;
using UnityEngine.SceneManagement;

public class ChooseCharacterController : MonoBehaviour
{
    public void ChooseCharacter1()
    {
        PlayerPrefs.SetInt("SelectedCharacter", 0);
        PlayerPrefs.Save();
        SceneManager.LoadScene("Level1");
    }

    public void ChooseCharacter2()
    {
        PlayerPrefs.SetInt("SelectedCharacter", 1);
        PlayerPrefs.Save();
        SceneManager.LoadScene("Level1");
    }

    public void ChooseCharacter3()
    {
        PlayerPrefs.SetInt("SelectedCharacter", 2);
        PlayerPrefs.Save();
        SceneManager.LoadScene("Level1");
    }
}