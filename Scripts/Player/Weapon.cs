using UnityEngine;
using System.Collections;

public abstract class Weapon
{
    protected float shootTimer = 0.0f;
    protected float shootDelay = 0.2f;
    protected int level = 1;

    public Weapon(int level = 1)
    {
        this.level = level;
    }

    public virtual void Update()
    {
        shootTimer -= Time.deltaTime;
        shootTimer = Mathf.Clamp(shootTimer, 0.0f, shootDelay);
    }

    public void Upgrade()
    {
        level++;
    }

    public void Downgrade()
    {
        level--;
        level = Mathf.Clamp(level, 0, int.MaxValue);
    }

    abstract public void Shoot(Transform transform);

    public bool CanShoot()
    {
        return shootTimer <= 0;
    }
}
