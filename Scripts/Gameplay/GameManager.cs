using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class GameManager : MonoBehaviour
{
    public static GameUI GameUI;

    public GameObject EnemyPrefab;
    public GameObject[] PowerUpPrefabs;

    private Player player;
    private List<Spawner> spawners = new List<Spawner>();
    private Circle gameArea;

    float survivalTime = 0.0f;
    bool isRunning = true;

    void Awake() 
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        GameUI = GameObject.Find("UI").GetComponent<GameUI>();
        gameArea = GameObject.Find("GameArea").GetComponent<Circle>();
    }

    void Start()
    {
        spawners.Add(new EnemySpawner(EnemyPrefab, gameArea));
        spawners.Add(new PowerUpSpawner(PowerUpPrefabs, gameArea));
    }
	
    void Update() 
    {
        if (isRunning)
        {
            survivalTime += Time.deltaTime;
            string str = string.Format("{0:00}:{1:00}", Mathf.FloorToInt(survivalTime / 60),
                                                        Mathf.FloorToInt(survivalTime % 60));

            GameUI.time.text = str;

            foreach (Spawner spawner in spawners)
            {
                spawner.Update();
            }

            if (player.IsDead)
            {
                isRunning = false;
                GameUI.ToggleEnd();
            }
        }
        else if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            Application.LoadLevel(0);
        }
    }
}
