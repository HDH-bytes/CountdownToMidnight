using UnityEngine;

/// <summary>
/// Place this on a GameObject in StartScene (or ChooseCharacter scene).
/// Always resets lives to 10 when StartScene is visited, clearing any stale
/// PlayerPrefs value from a previous session.
/// </summary>
public class LifeManagerBootstrap : MonoBehaviour
{
    void Awake()
    {
        if (LifeManager.Instance == null)
            new GameObject("LifeManager").AddComponent<LifeManager>();

        // Always start at full health whenever the player is on StartScene
        LifeManager.Instance.ResetLives();
    }
}
