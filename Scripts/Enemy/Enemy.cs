using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public partial class Enemy : MonoBehaviour, IDamageable
{
    public GameObject bulletPrefab;

    private NavMeshAgent agent;
    private CombatStatus status;
    private Animator animator;
    private Circle gameArea;

    // Dodging has a small delay that the enemy must wait before doing it again
    private float dodgeDelay = 2.0f;
    private float dodgeTimer = 0.0f;

    private CombatStatus target;
    private float attackRange = 12.5f;

    private Cerebrum cerebrum;

    void Awake()
    {
        gameArea = GameObject.Find("GameArea").GetComponent<Circle>();
        agent = GetComponent<NavMeshAgent>();
        status = GetComponent<CombatStatus>();
        animator = GetComponentInChildren<Animator>();

        cerebrum = new Cerebrum(this.gameObject);
    }

	void Update()
    {
        if (status.IsDead)
            return;

        if (dodgeTimer >= 0.0f) 
            dodgeTimer -= Time.deltaTime;

        animator.SetFloat("Speed", agent.velocity.magnitude);

        cerebrum.Think();
	}

    void OnAttack()
    {
        GameObject.Instantiate(bulletPrefab, 
                               transform.position + transform.forward, 
                               transform.rotation);
    }

    void OnPlayerDead()
    {
        cerebrum.React(new FreeRoamState());
    }

    public void Damage(int amount)
    {
        var ps = GameObject.Find("HitEffect").GetComponent<ParticleSystem>();
        ps.transform.position = transform.position + (Vector3.up * 0.5f);
        ps.Emit(5);

        status.CurrentHealth -= amount;
        status.StartCoroutine("HitBlink");

        if (!(cerebrum.CurrentState is HostileState))
            cerebrum.React(new HostileState());

        if (status.IsDead)
        {
            Disable();
            StartCoroutine(DelayedDestroy(5.0f));
        }
    }

    void Disable()
    {
        animator.SetFloat("Speed", 0);
        animator.SetBool("IsDead", status.IsDead);
        animator.SetBool("IsAttacking", false);

        GetComponent<Collider>().enabled = false;
        GetComponent<CombatStatus>().enabled = false;

        StopAllCoroutines();
        agent.enabled = false;
    }

    void OnNearMiss(GameObject bullet)
    {
        if (!status.IsDead && !(cerebrum.CurrentState is DodgeState) && dodgeTimer <= 0)
            cerebrum.ReactAndRemember(new DodgeState(bullet));
    }

    IEnumerator DelayedDestroy(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(this.gameObject);
    }

    bool DetectPlayer()
    {
        GameObject player = GameObject.Find("Player");

        if (player == null)
            return false;

        Vector3 toPlayer = (player.transform.position - transform.position).normalized;

        const float MAX_VISION_DISTANCE = 15.0f;

        float distance = Vector3.Distance(transform.position, player.transform.position);

        if (distance < MAX_VISION_DISTANCE)
        {
            float dot = Vector3.Dot(transform.forward, toPlayer);
            float degrees = Mathf.Round(Mathf.Acos(dot) * Mathf.Rad2Deg);

            return degrees >= 0 && degrees <= 45;
        }

        return false;
    }

    class Cerebrum : Cerebrum<Enemy>
    {
        public Cerebrum(GameObject actor) : base(actor) { }

        public override void Think()
        {
            if (FSM.CurrentState is DodgeState)
                return;

            if (actor.DetectPlayer() && actor.target == null)
            {
                React(new HostileState());
            }
            else if (actor.target == null && !(FSM.CurrentState is FreeRoamState))
            {
                React(new FreeRoamState());
            }
        }
    }
}