using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public int Score { get; private set; }

    public readonly float MAX_HEALTH = 1000.0f;
    public float health { get; private set; }
    public float shield { get; private set; }
    public float shieldRemainingTime { get; private set; }

    public readonly float MAX_FULLNESS = 1000.0f;
    public float fullness { get; private set; }
    private float starveThreshold = 0.1f;

    public float healthRestoreRate = 15f;
    public float healthRestoreCost = 15f;

    public float FullnessDepletionRate { get; private set; }  = .1f;

    [SerializeField]
    private GameObject playerShield;

    private void Start()
    {
        health = MAX_HEALTH;
        fullness = MAX_FULLNESS;
        shield = 0;
        shieldRemainingTime = 0;

        uIManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<PlayerUIManager>();

        playerShield = transform.Find("PlayerShield").gameObject;
        playerShield.SetActive(false);

    }

    private void Update()
    {
        TickPlayerAttributes(Time.deltaTime);
        uIManager.UpdateHealth();
        uIManager.UpdateFullness();
        uIManager.UpdateScore();
        uIManager.UpdateShield();
        uIManager.UpdateFullnessBar();
        uIManager.UpdateHealthBar();

        TickShield();

    }



    #region General

    private AudioSource headAudio;

    public void TickShield()
    {
        shieldRemainingTime = Mathf.Max(0, shieldRemainingTime - Time.deltaTime);

        if (playerShield.active && (shieldRemainingTime <= 0 || shield <= 0))
            playerShield.SetActive(false);
    }

    public void ToggleShield()
    {
        if (playerShield.active)
        {
            playerShield.SetActive(false);
        }
        else
        {
            if (shield > 0 && shieldRemainingTime > 0)
            {
                playerShield.SetActive(true);
            }
        }
    }

    public void TickPlayerAttributes(float deltaTime)
    {
        
        if (fullness <= starveThreshold)
        {
            TakeDamage(StarvingDamage() * deltaTime);
        } else if (health < MAX_HEALTH)
        {
            RestoreHealth(healthRestoreRate * deltaTime);
            RestoreFullness(-healthRestoreCost * deltaTime);
        }
        else
        {
            TickFullness(deltaTime);
        }
    }

    public int ScorePoints(int pointsToAdd)
    {
        Score += pointsToAdd;
        Score = Mathf.Max(0, Score);
        Debug.Log("Player scored " + pointsToAdd + " points! Current score is " + Score);
        return Score;
    }

    public void EatTreat(SweetTreat treat)
    {
        ScorePoints(treat.Points);
        Destroy(treat.gameObject);
        Debug.Log("Sweet treat was eaten");
    }

    #endregion

    #region Health

    public float TakeDamage(float damage)
    {
        // Deplete Shield first

        if (shield > 0)
        {
            float depletion = Mathf.Min(shield, damage);

            shield -= depletion;
            damage -= depletion;

            // TODO: handle shield visual update
        }

        health = Mathf.Clamp(health - damage, 0, MAX_HEALTH);
        return health;
    }

    public float AddShield(float shield, float time)
    {
        this.shield += shield;
        this.shieldRemainingTime += time;

        return shield;
    }

    public float RestoreHealth(float healthRestored)
    {
        health = Mathf.Clamp(health + healthRestored, 0, MAX_HEALTH);
        return health;
    }

    public float TickFullness(float delta)
    {
        float temp = fullness;
        fullness = Mathf.Clamp(fullness - FullnessDepletionRate * delta, 0, MAX_FULLNESS);
        return fullness;
    }

    public float RestoreFullness(float fullnessRestored)
    {
        fullness = Mathf.Clamp(fullness + fullnessRestored, 0, MAX_FULLNESS);
        //Debug.Log("Player fullness increased by " + fullnessRestored + "! Current fullness is " + fullness);
        return fullness;
    }

    private float StarvingDamage()
    {
        // TODO: determine StarvingDamage
        return 1f;
    }

    #endregion

    #region UI

    [SerializeField]
    private PlayerUIManager uIManager;




    #endregion

}
