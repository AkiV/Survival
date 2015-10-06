using UnityEngine;
using System.Collections;

public class WeaponPowerUp : PowerUp
{
    public override void ApplyPower(GameObject collector)
    {
        Player player = collector.GetComponent<Player>();

        if (player)
        {
            player.Weapon.Upgrade();
        } 
    }
}