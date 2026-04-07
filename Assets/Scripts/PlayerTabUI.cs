using UnityEngine;
using TMPro; // You need this for TextMeshPro!

public class PlayerTabUI : MonoBehaviour
{
    public TextMeshProUGUI playerListText;

    // This built-in function runs automatically the exact moment 
    // your TabController opens this specific page!
    void OnEnable()
    {
        RefreshPlayerList();
    }

    void RefreshPlayerList()
    {
        // 1. Clear the old text and set up a title
        if (playerListText == null) return;
        playerListText.text = "<b>PLAYERS</b>\n\n";

        // 2. Find the player in your scene
        // Make sure your Player GameObject actually has the tag "Player" in the Inspector!
        GameObject[] allPlayers = GameObject.FindGameObjectsWithTag("Player");

        // 3. Loop through and grab their info
        foreach (GameObject p in allPlayers)
        {
            PlayerStats stats = p.GetComponent<PlayerStats>();
            
            if (stats != null)
            {
                // This writes: "Justin - (Ninja)" onto your screen
                playerListText.text += stats.playerName + " - (" + stats.characterName + ")\n";
            }
        }
    }
}

