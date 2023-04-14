using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public readonly float MAX_TIME = 180;
    public float GameTime { get; private set; }
    private float treatInterval;
    private float explosiveChance;
    private float powerupInterval;
    private float targetInterval;

    public InputActionMap inputActions;
    private PlayerHandController leftHand;
    private PlayerHandController rightHand;

    private void Awake()
    {
        
    }


    // Start is called before the first frame update
    void Start()
    {
        leftHand = GameObject.Find("LeftController").GetComponent<PlayerHandController>();
        rightHand = GameObject.Find("RightController").GetComponent<PlayerHandController>();
        
        inputActions["LeftHandClose"].performed += _ => leftHand.CloseHand();
        inputActions["LeftHandClose"].canceled += _ => leftHand.OpenHand();
        inputActions["RightHandClose"].performed += _ => rightHand.CloseHand();
        inputActions["RightHandClose"].canceled += _ => rightHand.OpenHand();

    }

    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
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
