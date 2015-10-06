using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameUI : MonoBehaviour 
{
    public Canvas UICanvas;
    public GameObject EndUI;


    public Image HealthBar;
    public Image ShieldBar;
    public Text time;

    public GameObject BombIcon;
    private Stack<GameObject> bombIcons;

    void Awake()
    {
        time = GameObject.Find("Timer").GetComponent<Text>();
        bombIcons = new Stack<GameObject>();
    }

    void Start()
    {
        Player player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        for (int i = 0; i < player.BombCount; i++)
        {
            var bomb = GameObject.Instantiate(BombIcon);
            bomb.transform.position += Vector3.right * 50f * i;
            bomb.transform.SetParent(UICanvas.transform, false);
            bombIcons.Push(bomb);
        }
    }

    float healthUI = 1.0f;
    float shieldUI = 0.0f;

    void Update()
    {
        HealthBar.fillAmount = Mathf.MoveTowards(HealthBar.fillAmount, healthUI, Time.deltaTime);
        ShieldBar.fillAmount = Mathf.MoveTowards(ShieldBar.fillAmount, shieldUI, Time.deltaTime * 2f);
    }

    public void AddBombIcon()
    {
        var bomb = GameObject.Instantiate(BombIcon);
        bomb.transform.position += Vector3.right * 50f * (bombIcons.Count);
        bomb.transform.SetParent(UICanvas.transform, false);
        bombIcons.Push(bomb);
    }

    public void RemoveBombIcon()
    {
        Destroy(bombIcons.Pop());
    }

    public void UpdateHealth(float amount)
    {
        healthUI = amount;
    }

    public void UpdateShield(float amount)
    {
        if (amount <= 0)
            HealthBar.enabled = true;
        else
            HealthBar.enabled = false;

        shieldUI = amount;
    }

    public void ToggleEnd()
    {
        EndUI.SetActive(!EndUI.activeSelf);
    }
}
