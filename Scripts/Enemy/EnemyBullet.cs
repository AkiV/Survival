using UnityEngine;
using System.Collections;

public class EnemyBullet : MonoBehaviour
{
    private float lifeTime = 4f;
    private Rigidbody body;

    void Awake()
    {
        body = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        Vector3 move = (transform.forward * 6.0f * Time.deltaTime);
        body.MovePosition(body.position + move);
    }

    void Update()
    {
        lifeTime -= Time.deltaTime;

        if (lifeTime <= 0)
            Destroy(this.gameObject);
    }

    void OnTriggerEnter(Collider col)
    {
        if (!col.CompareTag("Enemy") && !col.CompareTag("EnemyBullet"))
        {
            IDamageable target = col.GetComponent<IDamageable>();

            if (target != null)
                target.Damage(5);

            Destroy(this.gameObject);
        }
    }
}