using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class ChooseCharacterController : MonoBehaviour
{
    [Header("Character Data")]
    public Sprite[] characterSprites;
    public string[] characterNames;

    [Header("UI")]
    public Image characterPreviewImage;
    public TMP_Text characterNameText;
    public TMP_Text playerNameText;

    private int currentCharacterIndex = 0;

    private void Start()
    {
        if (characterSprites == null || characterSprites.Length == 0)
        {
            Debug.LogError("No character sprites assigned.");
            return;
        }

        currentCharacterIndex = PlayerPrefs.GetInt("SelectedCharacter", 0);

        if (currentCharacterIndex >= characterSprites.Length)
        {
            currentCharacterIndex = 0;
        }

        string playerName = PlayerPrefs.GetString("SavedPlayerName", "Player1");

        if (playerNameText != null)
        {
            playerNameText.text = "Player: " + playerName;
        }

        UpdateCharacterUI();
    }

    public void NextCharacter()
    {
        currentCharacterIndex++;

        if (currentCharacterIndex >= characterSprites.Length)
        {
            currentCharacterIndex = 0;
        }

        UpdateCharacterUI();
    }

    public void PreviousCharacter()
    {
        currentCharacterIndex--;

        if (currentCharacterIndex < 0)
        {
            currentCharacterIndex = characterSprites.Length - 1;
        }

        UpdateCharacterUI();
    }

    public void ConfirmCharacter()
    {
        PlayerPrefs.SetInt("SelectedCharacter", currentCharacterIndex);
        PlayerPrefs.Save();

        SceneManager.LoadScene("demo");
    }

    private void UpdateCharacterUI()
    {
        if (characterPreviewImage != null)
        {
            characterPreviewImage.sprite = characterSprites[currentCharacterIndex];
        }

        if (characterNameText != null)
        {
            if (characterNames != null && currentCharacterIndex < characterNames.Length)
            {
                characterNameText.text = characterNames[currentCharacterIndex];
            }
            else
            {
                characterNameText.text = "Character " + (currentCharacterIndex + 1);
            }
        }
    }
}