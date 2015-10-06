using UnityEngine;
using System.Collections;

public class Bomb : MonoBehaviour 
{
    ParticleSystem particles;
    SphereCollider sphereCollider;

    float speed = 0.0f;

    void Awake() 
    {
        particles = GetComponent<ParticleSystem>();
        sphereCollider = GetComponent<SphereCollider>();
        speed = particles.startSpeed - particles.startSize;

        StartCoroutine(Flash());
    }
	
    void Update() 
    {
        if (particles.isPlaying)
        {
            sphereCollider.radius += speed * Time.deltaTime;
        }
        else
        {
            sphereCollider.enabled = false;
            StartCoroutine(FinishAndDestroy());
        }   
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Enemy"))
        {
            IDamageable target = col.gameObject.GetComponent<IDamageable>();
            target.Damage(50);
        }
        else if(col.CompareTag("EnemyBullet"))
        {
            Destroy(col.gameObject);
        }
    }

    IEnumerator Flash()
    {
        Light light = GameObject.Find("Directional Light").GetComponent<Light>();

        while (light.intensity < 3)
        {
            light.intensity += Time.deltaTime * 2f;
            yield return null;
        }
    }

    IEnumerator FinishAndDestroy()
    {
        Light light = GameObject.Find("Directional Light").GetComponent<Light>();

        while (light.intensity > 1)
        {
            light.intensity -= Time.deltaTime;
            yield return null;
        }

        light.intensity = 1;
        Destroy(this.gameObject);
    }
}
