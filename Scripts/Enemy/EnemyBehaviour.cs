using UnityEngine;
using System.Collections;

public partial class Enemy : MonoBehaviour
{
    class FreeRoamState : AIState
    {
        public void OnStart(GameObject actor)
        {
            actor.GetComponent<Enemy>().StartCoroutine("FreeRoam");
        }

        public void OnStop(GameObject actor)
        {
            actor.GetComponent<Enemy>().StopCoroutine("FreeRoam");
        }
    }

    class DodgeState : AIState
    {
        private GameObject bullet;

        public DodgeState(GameObject bullet)
        {
            this.bullet = bullet;
        }

        public void OnStart(GameObject actor)
        {
            actor.GetComponent<Enemy>().StartCoroutine("DodgeBullet", bullet);
        }

        public void OnStop(GameObject actor)
        {
            Enemy enemy = actor.GetComponent<Enemy>();
            enemy.StopCoroutine("DodgeBullet");

            if (!enemy.status.IsDead)
            {
                enemy.agent.enabled = true;
            }
        }
    }

    class HostileState : AIState
    {
        public void OnStart(GameObject actor)
        {
            Enemy enemy = actor.GetComponent<Enemy>();
            var player = GameObject.Find("Player");

            if (player)
            {
                enemy.target = player.GetComponent<CombatStatus>();
                enemy.StartCoroutine("Hostile");
            }
        }

        public void OnStop(GameObject actor)
        {
            actor.GetComponent<Enemy>().StopCoroutine("Hostile");
            Enemy enemy = actor.GetComponent<Enemy>();
            enemy.animator.SetBool("IsAttacking", false);
            enemy.target = null;
        }
    }

    IEnumerator FreeRoam()
    {
        while (true)
        {
            if (agent.enabled && agent.remainingDistance <= agent.stoppingDistance)
            {
                Vector2 random = Random.insideUnitCircle * gameArea.radius;

                if (gameArea.InsideCircle(random))
                    agent.SetDestination(new Vector3(random.x, 0.0f, random.y));
            }

            yield return new WaitForSeconds(1.0f);
        }
    }

    IEnumerator DodgeBullet(GameObject bullet)
    {
        float dodgeDistance = 4.0f;

        animator.SetBool("IsAttacking", false);

        dodgeTimer = dodgeDelay;
        agent.enabled = false;

        // The enemy will move away from the bullet, which is the inverse of distance vector.
        Vector3 toBullet = (bullet.transform.position - transform.position).normalized;
        toBullet -= Vector3.up * toBullet.y;

        Vector3 dodge = (toBullet.normalized) * -dodgeDistance;

        float totalDistance = 0.0f;

        while (totalDistance < dodgeDistance)
        {
            animator.SetFloat("Speed", 4f);

            Vector3 move = (dodge * 4.0f * Time.deltaTime);
            Vector3 newPosition = transform.position + move;

            totalDistance += move.magnitude;

            if (gameArea.InsideCircle(newPosition))
                GetComponent<Rigidbody>().MovePosition(newPosition);
            else
                totalDistance = dodgeDistance;

            yield return null;
        }

        if (!status.IsDead)
            cerebrum.SignalStop();
    }

    IEnumerator Hostile()
    {
        agent.enabled = true;

        while (true)
        {
            Vector3 toPlayer = (transform.position - target.transform.position);
            agent.SetDestination(target.transform.position + (toPlayer.normalized * 3.0f));

            bool atTarget = agent.enabled && agent.remainingDistance <= attackRange;
            animator.SetBool("IsAttacking", atTarget);

            yield return null;
        }
    }
}