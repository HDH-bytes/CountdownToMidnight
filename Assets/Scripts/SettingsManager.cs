using UnityEngine;
using UnityEngine.UI; // Needed to talk to the Checkbox/Toggle!

public class SettingsManager : MonoBehaviour
{
    [Header("Audio Setup")]
    public AudioSource backgroundMusic; // Drag your music source here
    public Toggle musicToggle;          // Drag your UI Toggle here

    void Start()
    {
        // 1. Load the saved preference. We use 1 for ON and 0 for OFF. 
        // We set the default to 1 (Music ON) for first-time players.
        int savedMusicState = PlayerPrefs.GetInt("MusicPreference", 1);
        
        // 2. Update the visual Checkbox to match the saved data
        if (savedMusicState == 1)
            musicToggle.isOn = true;
        else
            musicToggle.isOn = false;

        // 3. Actually apply the mute/unmute to the AudioSource
        ApplyMusicSettings(musicToggle.isOn);
    }

    // This is the function the Checkbox will call every time the player clicks it
    public void OnToggleClicked()
    {
        ApplyMusicSettings(musicToggle.isOn);
    }

    private void ApplyMusicSettings(bool isMusicOn)
    {
        if (isMusicOn)
        {
            backgroundMusic.mute = false; // Unmute the music
            PlayerPrefs.SetInt("MusicPreference", 1); // Save it as ON
        }
        else
        {
            backgroundMusic.mute = true; // Mute the music
            PlayerPrefs.SetInt("MusicPreference", 0); // Save it as OFF
        }
        
        // Force Unity to save the PlayerPrefs immediately
        PlayerPrefs.Save(); 
    }
}
