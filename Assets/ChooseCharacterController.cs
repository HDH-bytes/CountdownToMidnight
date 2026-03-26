using UnityEngine;
using UnityEngine.SceneManagement;

public class ChooseCharacterController : MonoBehaviour
{
    public void ChooseCharacter1()
    {
        PlayerPrefs.SetInt("SelectedCharacter", 0);
        PlayerPrefs.Save();
        SceneManager.LoadScene("demo");
    }

    public void ChooseCharacter2()
    {
        PlayerPrefs.SetInt("SelectedCharacter", 1);
        PlayerPrefs.Save();
        SceneManager.LoadScene("demo");
    }
}