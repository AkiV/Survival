using UnityEngine;
using System.Collections;

public class BombPowerUp : PowerUp 
{
    public override void ApplyPower(GameObject collector)
    {
        Player player = collector.GetComponent<Player>();

        if (player)
        {
            player.IncreaseBombs();
        } 
    }
}
