using UnityEngine;

public class PlayerStats : Character
{
    [Header("Identity")]
    public string playerName;
    public string characterName; 

    void Start()
    {
        
        playerName = PlayerPrefs.GetString("SavedPlayerName", "Player 1");

    
        int characterIndex = PlayerPrefs.GetInt("SelectedCharacter", 0);

        switch (characterIndex)
        {
            case 0:
                characterName = "Normal Guy"; 
                break;
            case 1:
                characterName = "Ninja";  
                break;
            case 2:
                characterName = "Strong Woman"; 
                break;
            default:
                characterName = "Unknown Class";
                break;
        }
    }
}
