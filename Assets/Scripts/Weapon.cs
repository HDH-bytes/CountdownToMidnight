using System.Collections;
using UnityEngine;

//base class, extend to other weapon subclasses
public abstract class Weapon : MonoBehaviour
{
    public int unlockSceneIndex = 1;
    // Each type of weapon will implement this differently
    public abstract void Attack();
}