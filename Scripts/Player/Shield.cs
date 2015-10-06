using UnityEngine;
using System.Collections;

public class Shield : MonoBehaviour, IDamageable
{
    int maxForce = 60;
    int force = 0;

    GameObject player;
    Material material;

    void Awake() 
    {
        player = GameObject.FindGameObjectWithTag("Player");
        material = GetComponent<Renderer>().material;

        force = maxForce;
        GameManager.GameUI.UpdateShield(1.0f);
    }

    float offset = 1.0f;

    void Update() 
    {
        offset += Time.deltaTime * 0.75f;
        transform.position = player.transform.position;
        material.SetTextureOffset("_MainTex", new Vector2(0.0f, offset));
    }

    public void Damage(int amount)
    {
        StartCoroutine("HitBlink");

        force -= amount;
        GameManager.GameUI.UpdateShield((float) force / maxForce);

        if (force <= 0)
        {
            GetComponent<Animator>().SetBool("Destroyed", true);
            Invoke("Finish", 1.0f);
            transform.parent = player.transform;
            this.enabled = false;
        }
    }

    void Finish()
    {
        Destroy(this.gameObject);
    }

    bool hitBlink = false;

    IEnumerator HitBlink()
    {
        if (!hitBlink)
        {
            hitBlink = true;

            Color originalColor = GetComponentInChildren<Light>().color;

            Light light = GetComponentInChildren<Light>();

            light.color = Color.blue * 0.3f;

            yield return new WaitForSeconds(0.1f);

            light.color = originalColor;

            hitBlink = false;
        }
    }
}
