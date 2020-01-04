using UnityEngine;

public abstract class Boost : MonoBehaviour
{
    public abstract void ApplyBoost();

    public abstract void Interact(Vector3 position);
}
