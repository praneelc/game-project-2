using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public int Score { get; private set; }

    public readonly float MAX_HEALTH;
    public float health { get; private set; }
    private float shield;
    private float shieldRemainingTime;

    public readonly float MAX_FULLNESS;
    public float fullness { get; private set; }
    private float starveThreshold;

    public float FullnessDepletionRate { get; private set; }  = .5f;

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
        return Score;
    }

    #endregion

    #region Health

    public float TakeDamage(float damage)
    {
        health = Mathf.Clamp(health - damage, 0, MAX_HEALTH);
        return health;
    }

    public float RestoreHealth(float healthRestored)
    {
        health = Mathf.Clamp(health + healthRestored, 0, MAX_HEALTH);
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
        return fullness;
    }

    private float StarvingDamage()
    {
        // TODO: determine StarvingDamage
        return 1f;
    }

    #endregion
}
