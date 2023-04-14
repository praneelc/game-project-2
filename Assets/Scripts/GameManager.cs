using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public readonly float MAX_TIME = 180;
    public float GameTime { get; private set; }
    private float treatInterval;
    private float explosiveChance;
    private float powerupInterval;
    private float targetInterval;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    #region Game Sequence
    public void StartGame()
    {

    }

    public void EndGame()
    {

    }

    public void ResetGame()
    {

    }

    #endregion

    #region Game Play
    public void SpawnSweetTreat()
    {

    }

    public void SpawnExplosive()
    {

    }

    public void SpawnTarget()
    {

    }

    public void SpawnPowerup()
    {

    }

    #endregion
}
