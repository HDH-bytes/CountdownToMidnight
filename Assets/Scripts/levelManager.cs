using UnityEngine;

public class levelManager
{
    public int points;
    public void CompleteLevel()
    {
        ScoreManager.Instance.AddPoints(points);
    }
}
