using UnityEngine;
using TMPro;
using UnityEngine.UI; 
public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    public int score = 0;
    public TMP_Text scoreText;
    public Slider scoreBar;
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
