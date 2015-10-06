using UnityEngine;
using System.Collections;
using System;

public class CombatStatus : MonoBehaviour
{
    public int MaxHealth = 100;
    public AudioClip HitSound;
    public bool IsDead { get { return CurrentHealth <= 0; } }

    public int CurrentHealth { get; set; }

    void Awake()
    {
        CurrentHealth = MaxHealth;
    }

    bool hitBlink = false;

    IEnumerator HitBlink()
    {
        if (!hitBlink)
        {
            hitBlink = true;

            if (HitSound)
                GetComponent<AudioSource>().PlayOneShot(HitSound);

            Material material = GetComponentInChildren<Renderer>().material;
            Color originalColor = material.color;

            material.color = Color.red;

            yield return new WaitForSeconds(0.1f);

            material.color = originalColor;

            hitBlink = false;
        }
    }
}