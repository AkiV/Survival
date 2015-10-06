using UnityEngine;
using System.Collections;

public class ShieldPowerUp : PowerUp
{
    public override void ApplyPower(GameObject collector)
    {
        Player player = collector.GetComponent<Player>();

        if (player)
        {
            player.ActivateShield();
        }
    }
}