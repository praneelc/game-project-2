using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PlayerManager : MonoBehaviour
{
    public int Score { get; private set; }

    public readonly float MAX_HEALTH = 1000.0f;
    public float health { get; private set; }
    public float shield { get; private set; }
    public float shieldRemainingTime { get; private set; }

    public readonly float MAX_FULLNESS = 1000.0f;
    public float fullness { get; private set; }
    private float starveThreshold = 100f;
    private float starveWarningThreshold = 500f;

    public float healthRestoreRate;
    public float healthRestoreCost;

    public float FullnessDepletionRate;

    [SerializeField]
    private GameObject playerShield;

    [Header("Audio")]
    [SerializeField]
    private AudioClip hungerGrowl;
    [SerializeField]
    private AudioClip hungerComplain;

    private AudioSource audioSource;

    [SerializeField]
    private Volume ppVolume;

    [SerializeField]
    private float maxVignette = .4f;

    private Vignette healthVignette;

    private void Start()
    {
        health = MAX_HEALTH;
        fullness = MAX_FULLNESS;
        shield = 0;
        shieldRemainingTime = 0;

        uIManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<PlayerUIManager>();

        playerShield = transform.Find("PlayerShield").gameObject;
        playerShield.SetActive(false);

        audioSource = GetComponent<AudioSource>();

        ppVolume.profile.TryGet<Vignette>(out healthVignette);
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

        float prevFullness = fullness;
        
        if (fullness <= starveThreshold)
        {
            TakeDamage(StarvingDamage() * deltaTime);
        } else if (health < MAX_HEALTH)
        {
            RestoreHealth(healthRestoreRate * deltaTime);
            RestoreFullness(-healthRestoreCost * deltaTime);
        }

        TickFullness(deltaTime);

        // Play "I'm hungry sound" once when fullness crosses below 2 x threshold
        if ((prevFullness > starveWarningThreshold && fullness <= starveWarningThreshold)
            || (prevFullness > starveThreshold && fullness <= starveThreshold))
        {
            audioSource.PlayOneShot(hungerComplain);
            Debug.Log("Play I'm hungry");
        }

        // Play stomach growling sound effect randomly when under warning threshold
        if (fullness <= starveWarningThreshold)
        {
            float rand01 = Random.Range(0.0f, 1.0f);
            if (rand01 < 0.005f)
            {
                audioSource.PlayOneShot(hungerGrowl);
                Debug.Log("Play hunger growl");
            }
        }

        healthVignette.intensity.Override(Mathf.Pow((1- health/MAX_HEALTH), 2f) * maxVignette);
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
        RestoreFullness(treat.fullnessRestored);
        RestoreHealth(treat.healthRestored);

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
        return starveThreshold - fullness;
    }

    #endregion

    #region UI

    [SerializeField]
    private PlayerUIManager uIManager;




    #endregion

}
