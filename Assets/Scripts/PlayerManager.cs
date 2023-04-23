using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public int Score { get; private set; }

    public readonly float MAX_HEALTH = 10.0f;
    public float health { get; private set; }
    private float shield;
    private float shieldRemainingTime;

    public readonly float MAX_FULLNESS = 10.0f;
    public float fullness { get; private set; }
    private float starveThreshold = 0.1f;

    public float FullnessDepletionRate { get; private set; }  = .5f;

    private void Start()
    {
        health = MAX_HEALTH;
        fullness = MAX_FULLNESS;
        shield = 0;
        shieldRemainingTime = 0;
    }

    #region General

    public void TickPlayerAttributes(float deltaTime)
    {
        TickFullness();
        if (fullness <= starveThreshold)
        {
            TakeDamage(StarvingDamage());
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
        Debug.Log("Player took " + damage + " damage! Current health is " + health);
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
        Debug.Log("Player health increased by " + healthRestored + "! Current health is " + health);
        return health;
    }

    public float TickFullness()
    {
        fullness = Mathf.Clamp(fullness - FullnessDepletionRate, 0, MAX_FULLNESS);
        return fullness;
    }

    public float RestoreFullness(float fullnessRestored)
    {
        fullness = Mathf.Clamp(fullness + fullnessRestored, 0, MAX_FULLNESS);
        Debug.Log("Player fullness increased by " + fullnessRestored + "! Current fullness is " + fullness);
        return fullness;
    }

    private float StarvingDamage()
    {
        // TODO: determine StarvingDamage
        return 1f;
    }

    #endregion
}
