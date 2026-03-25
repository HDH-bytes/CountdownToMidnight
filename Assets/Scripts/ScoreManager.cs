using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    public int score = 0;
    public TMP_Text scoreText;
    public Slider ScoreBar;

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