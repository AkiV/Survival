using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour 
{
    private float lifeTime = 0.85f;
    private Rigidbody body;
    private Circle gameArea;

    const float NEAR_HIT_DISTANCE = 2f;

    void Awake() 
    {
        body = GetComponent<Rigidbody>();
        gameArea = GameObject.Find("GameArea").GetComponent<Circle>();
    }
	
    void FixedUpdate() 
    {
        Vector3 move = (transform.forward * 30f * Time.deltaTime);
        body.MovePosition(body.position + move);
    }

    void Update()
    {
        CheckNearMiss();

        lifeTime -= Time.deltaTime;

        if (lifeTime <= 0 || !gameArea.InsideCircle(transform.position))
            Destroy(this.gameObject);
    }

    void CheckNearMiss()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        if (enemies.Length == 0)
            return;
        
        foreach (GameObject enemy in enemies)
        {
            Vector3 toEnemy = (enemy.transform.position - transform.position).normalized;
            float distance = Vector3.Distance(transform.position, enemy.transform.position);

            if (distance < NEAR_HIT_DISTANCE)
            {
                float dot = Vector3.Dot(toEnemy, transform.forward);

                // The bullet nearly misses the enemy when the angle between the distance vector
                // and the bullet's direction is small enough, i.e. it flies by the side of the enemy.
                // This enables the player to shoot at the enemy without it dodging every time.
                bool isNearMiss = dot > -0.4f && dot < 0.4f;

                if (isNearMiss)
                    enemy.BroadcastMessage("OnNearMiss", this.gameObject);
            }
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (!col.CompareTag("Player") && !col.CompareTag("Bullet") && !col.CompareTag("Shield"))
        {
            IDamageable target = col.GetComponent<IDamageable>();

            if (target != null)
                target.Damage(4);
        }
    }
}
