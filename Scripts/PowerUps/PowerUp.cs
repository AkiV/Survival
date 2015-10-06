using UnityEngine;
using System.Collections;

public abstract class PowerUp : MonoBehaviour, ICollectible 
{
    public void Collect(GameObject collector)
    {
        ApplyPower(collector);
        Destroy(this.gameObject);
    }

    public abstract void ApplyPower(GameObject collector);
}