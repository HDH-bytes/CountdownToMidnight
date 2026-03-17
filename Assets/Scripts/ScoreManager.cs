using UnityEngine;
using TMPro; // if using TextMeshPro

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    public int score = 0;
    public TMP_Text scoreText;

    void Awake()
    {
        Instance = this;
    }

    public void AddPoints(int points)
    {
        score += points;
        scoreText.text = "Score: " + score;
    }
}