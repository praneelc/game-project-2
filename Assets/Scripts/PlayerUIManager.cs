using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerUIManager : MonoBehaviour
{
    PlayerManager player;
    GameManager gm;


    [SerializeField]
    TextMeshProUGUI timer;
    [SerializeField]
    TextMeshProUGUI health;
    [SerializeField]
    TextMeshProUGUI fullness;
    [SerializeField]
    TextMeshProUGUI score;
    [SerializeField]
    TextMeshProUGUI shield;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();
        gm = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();


        UpdateHealth();
        UpdateFullness();
        UpdateScore();
        UpdateShield();
        
    }

    private void Update()
    {
        timer.SetText(formatTime(gm.GameTime));
    }



    public void UpdateHealth()
    {
        health.SetText(string.Format("{0:000.0}", player.health));
    }

    public void UpdateFullness()
    {
        fullness.SetText(string.Format("{0:000.0}", player.fullness));
    }

    public void UpdateScore()
    {
        score.SetText(string.Format("{0:000.0}", player.Score));
    }

    public void UpdateShield()
    {
        shield.SetText(string.Format("{0:000}", player.shield));
    }

    public string formatTime(float seconds)
    {
        int mins = (int) Mathf.Floor(seconds / 60);
        float secs = seconds % 60;

        return string.Format("{0:00}:{1:00.0}", mins, secs);
    }






}
