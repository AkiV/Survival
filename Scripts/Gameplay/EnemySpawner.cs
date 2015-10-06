using UnityEngine;

class EnemySpawner : Spawner
{
    GameObject enemyPrefab;
    Circle gameArea;

    int spawnCount = 5;

    float increaseSpawnTimer = 15.0f;
    float increaseSpawnDelay = 15.0f;

    public EnemySpawner(GameObject enemyPrefab, Circle gameArea, float spawnDelay = 8.0f) 
        : base(spawnDelay)
    {
        this.enemyPrefab = enemyPrefab;
        this.gameArea = gameArea;
    }

    public override void Update()
    {
        base.Update();

        if (increaseSpawnTimer <= 0)
        {
            increaseSpawnTimer = increaseSpawnDelay * 1.1f;

            spawnDelay -= 0.2f;
            spawnDelay = Mathf.Clamp(spawnDelay, 1.0f, 10.0f);
        }

        increaseSpawnTimer -= Time.deltaTime;
    }


    public override void Spawn()
    {
        for (int i = 0; i < spawnCount; i++)
        {
            Vector3 spawnPos = gameArea.RandomOnCircle() * 1.5f + Vector3.up;
            GameObject.Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
        }
    }
}