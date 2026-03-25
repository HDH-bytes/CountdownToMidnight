using UnityEngine;

public class levelManager
{
    public void CompleteLevel()
    {
        FindObjectOfType<ScoreManager>().AddPoints(100);
    }
}