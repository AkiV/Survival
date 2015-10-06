using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour, IDamageable
{
    public GameObject BulletPrefab;
    public GameObject ShieldPrefab;
    public GameObject ExplosionPrefab;
    public GameObject BombPrefab;

    public AudioClip LaserSound;
    public AudioClip ShieldSound;

    public Weapon Weapon { get; private set; }
    public int BombCount { get; private set; }

    public bool IsDead { get { return status.IsDead; } }

    private CombatStatus status;
    private PlayerController controller;
    private GameObject shield;

    private float inputDeadZone = 0.5f;

	void Awake() 
    {
        status = GetComponent<CombatStatus>();
        controller = GetComponent<PlayerController>();
        Weapon = new PlasmaWeapon(BulletPrefab);
        BombCount = 1;
	}
	
    void FixedUpdate()
    {
        Vector2 dirMouse = controller.GetDirectionMouse();

        if (Input.GetMouseButton(1) && dirMouse.magnitude > inputDeadZone)
        {
            controller.ProcessMovement(dirMouse);
            controller.Rotate(dirMouse);
        }
    }

	void Update()
    {
        Weapon.Update();

        if (Input.GetButton("Fire3") && Weapon.CanShoot())
        {
            GetComponent<AudioSource>().PlayOneShot(LaserSound);
            Weapon.Shoot(transform);
        }

        if (Input.GetMouseButton(0))
        {
            controller.Rotate(controller.GetDirectionMouse());
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            LaunchBomb();
        }
	}

    public void IncreaseBombs()
    {
        BombCount++;
        GameManager.GameUI.AddBombIcon();
    }

    public void LaunchBomb()
    {
        if (BombCount > 0)
        {
            BombCount--;
            GameManager.GameUI.RemoveBombIcon();
            Instantiate(BombPrefab, transform.position, BombPrefab.transform.rotation);
        }
    }

    public void ActivateShield()
    {
        if (shield)
            Destroy(shield);

        shield = Instantiate(ShieldPrefab,
                             transform.position,
                             Quaternion.identity) as GameObject;

        GetComponent<AudioSource>().PlayOneShot(ShieldSound, 5.0f);
    }

    public void Damage(int amount)
    {
        if (status.IsDead)
            return;

        status.CurrentHealth -= amount;

        GameManager.GameUI.UpdateHealth((float)status.CurrentHealth / status.MaxHealth);

        status.StartCoroutine("HitBlink");

        if (status.IsDead)
        {
            Instantiate(ExplosionPrefab, transform.position, Quaternion.identity);
            Camera.main.GetComponentInParent<CameraFollow>().target = null;

            foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
            {
                enemy.SendMessage("OnPlayerDead");
            }

            Break();
            this.enabled = false;
        }
    }

    public void Break()
    {
        foreach (Transform child in gameObject.GetComponentsInChildren<Transform>())
        {
            child.parent = null;

            if (!child.GetComponent<Rigidbody>())
            {
                var rb = child.gameObject.AddComponent<Rigidbody>();
                rb.angularDrag = 5f;
            }

            child.gameObject.AddComponent<SphereCollider>();
        }

        Destroy(this.gameObject);
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Collectible"))
        {
            ICollectible collectible = collider.GetComponent<ICollectible>();

            if (collectible != null)
            {
                collectible.Collect(this.gameObject);
            }
        }
    }

}