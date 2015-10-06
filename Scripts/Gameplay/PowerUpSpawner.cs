using UnityEngine;
using System.Collections;

public class PowerUpSpawner : Spawner 
{
    GameObject[] powerUpPrefabs;
    Circle gameArea;

    public PowerUpSpawner(GameObject[] powerUpPrefabs, Circle gameArea, float spawnDelay = 30.0f) 
        : base(spawnDelay)
    {
        this.powerUpPrefabs = powerUpPrefabs;
        this.gameArea = gameArea;
        this.spawnTimer = spawnDelay;
    }

    public override void Spawn()
    {
        this.spawnTimer = Random.Range(spawnDelay, spawnDelay * 1.333f);

        int roll = Random.Range(0, powerUpPrefabs.Length);
        GameObject toInstantiate = powerUpPrefabs[roll];

        if (toInstantiate != null)
        {
            Vector3 randomPos = Random.insideUnitCircle * (gameArea.radius * 0.8f);
            Vector3 spawnPos = new Vector3(randomPos.x, 0, randomPos.y) + (Vector3.up * 0.5f);

            GameObject.Instantiate(toInstantiate, spawnPos, 
                                   toInstantiate.transform.rotation);
        }
    }
}
