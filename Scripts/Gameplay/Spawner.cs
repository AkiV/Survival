using UnityEngine;
using System.Collections;

public abstract class Spawner 
{
    protected float spawnTimer = 0.0f;
    protected float spawnDelay = 1.0f;

    public Spawner(float spawnDelay)
    {
        this.spawnDelay = spawnDelay;
    }

    virtual public void Update()
    {
        if (spawnTimer <= 0.0f)
        {
            spawnTimer = spawnDelay;
            Spawn();
        }

        spawnTimer -= Time.deltaTime;
    }

    abstract public void Spawn();
}