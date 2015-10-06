using UnityEngine;
using System.Collections;

public class PlasmaWeapon : Weapon 
{
    private GameObject bulletPrefab = null;

    public PlasmaWeapon(GameObject bulletPrefab, int level = 1) : base(level)
    {
        this.bulletPrefab = bulletPrefab;
    }

    public override void Shoot(Transform parent)
    {
        shootTimer = shootDelay;

        if (level == 1)
        {
            GameObject.Instantiate(bulletPrefab, parent.position +
                                    parent.forward + (parent.right * 0.5f),
                                    parent.rotation);

            GameObject.Instantiate(bulletPrefab, parent.position +
                                   parent.forward + (-parent.right * 0.5f),
                                   parent.rotation);
        }

        if (level == 2 || level == 3)
        {
            float forward = PolarToAngle(parent.forward);

            float angle1 = forward + 10;
            float angle2 = forward - 10;

            GameObject.Instantiate(bulletPrefab, parent.position,
                                   Quaternion.Euler(0, angle1, 0));

            GameObject.Instantiate(bulletPrefab, parent.position + parent.forward,
                                  parent.rotation);

            GameObject.Instantiate(bulletPrefab, parent.position,
                                   Quaternion.Euler(0, angle2, 0));
        }

        if (level == 3 || level >= 4)
        {
            float forward = PolarToAngle(-parent.forward);

            float angle1 = forward + 45;
            float angle2 = forward - 45;

            GameObject.Instantiate(bulletPrefab, parent.position,
                                   Quaternion.Euler(0, angle1, 0));

            GameObject.Instantiate(bulletPrefab, parent.position,
                                   Quaternion.Euler(0, angle2, 0));
        }

        if (level >= 4)
        {
            float forward = PolarToAngle(parent.forward);

            for (int i = -2; i < 2; i++)
            {
                GameObject.Instantiate(bulletPrefab, parent.position,
                                      Quaternion.Euler(0, forward + i * 7.5f, 0));
            }
        }
    }

    float PolarToAngle(Vector3 vector)
    {
        return Mathf.Atan2(vector.x, vector.z) * Mathf.Rad2Deg;
    }
}
