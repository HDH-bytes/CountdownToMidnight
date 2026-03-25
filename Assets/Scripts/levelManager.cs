using UnityEngine;

public class levelManager
{
    public void CompleteLevel()
    {
        ScoreManager.Instance.AddPoints(points);
    }
}
