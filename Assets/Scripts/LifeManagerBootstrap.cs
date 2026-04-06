using UnityEngine;

/// <summary>
/// Place this on a GameObject in StartScene (or ChooseCharacter scene).
/// It boots the LifeManager singleton before any level loads.
/// The LifeManager itself is DontDestroyOnLoad so it only needs to be created once.
/// </summary>
public class LifeManagerBootstrap : MonoBehaviour
{
    void Awake()
    {
        if (LifeManager.Instance == null)
        {
            new GameObject("LifeManager").AddComponent<LifeManager>();
        }
    }
}
