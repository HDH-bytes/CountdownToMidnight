using UnityEngine;
using UnityEngine.SceneManagement;

public class levelManager : MonoBehaviour
{
    // Level 0 = 25 XP, Level 1 = 75 XP, Level 2 = 125 XP ...  (25 + index * 50)
    public static int GetXPForLevel(int buildIndex)
    {
        return 25 + buildIndex * 50;
    }

    // Call this if you trigger completion from a GameObject with this component
    public void CompleteLevel()
    {
        int index = SceneManager.GetActiveScene().buildIndex;
        ScoreManager.Instance.AddXP(GetXPForLevel(index));
    }
}