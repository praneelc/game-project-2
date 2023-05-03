using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerUIManager : MonoBehaviour
{
    PlayerManager player;
    GameManager gm;


    [SerializeField]
    List<TextMeshProUGUI> timers;
    [SerializeField]
    TextMeshProUGUI health;
    [SerializeField]
    TextMeshProUGUI fullness;
    [SerializeField]
    List<TextMeshProUGUI> scores;
    [SerializeField]
    List<TextMeshProUGUI> shields;
    [SerializeField]
    List<TextMeshProUGUI> shieldTimers;
    [SerializeField]
    TextMeshProUGUI noShieldText;

    [SerializeField]
    TextMeshProUGUI healthBar;
    [SerializeField]
    TextMeshProUGUI fullnessBar;

    private readonly int BAR_LENGTH = 50;

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
        foreach (var timer in timers)
        {
            timer.SetText(formatTime(gm.MAX_TIME - gm.GameTime));
        }
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
        foreach (var score in scores)
        {
            score.SetText(string.Format("{0:0}", player.Score));
        }
    }

    public void UpdateShield()
    {
        foreach (var shield in shields)
        {
            if (player.shield <= 0 || player.shieldRemainingTime <= 0)
            {
                shield.SetText("");
            } else
            {
                shield.SetText(string.Format("{0:0}", player.shield));
            }
        }

        foreach (var shieldTimer in shieldTimers)
        {
            if (player.shield <= 0 || player.shieldRemainingTime <= 0)
            {
                shieldTimer.SetText("");
            }
            else
            {
                shieldTimer.SetText(formatTime(player.shieldRemainingTime));
            }
        }

        if (player.shield <= 0 || player.shieldRemainingTime <= 0)
        {
            noShieldText.SetText("Hit the floating shields to gain a shield!");
        }
    }

    public string formatTime(float seconds)
    {
        int mins = (int) Mathf.Floor(seconds / 60);
        float secs = seconds % 60;

        return string.Format("{0:0}:{1:00}", mins, secs);
    }

    public void UpdateHealthBar()
    {
        float healthRatio = player.health / player.MAX_HEALTH;
        int numBars = Mathf.RoundToInt(healthRatio * BAR_LENGTH);
        string text = new string('|', numBars);
        healthBar.SetText(text);
    }

    public void UpdateFullnessBar()
    {
        float fullnessRatio = player.fullness / player.MAX_FULLNESS;
        int numBars = Mathf.RoundToInt(fullnessRatio * BAR_LENGTH);
        string text = new string('|', numBars);
        fullnessBar.SetText(text);
    }
}
