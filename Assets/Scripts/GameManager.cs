using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public readonly float MAX_TIME = 180;
    public float GameTime { get; private set; }

    [Header("Treat / Target Params")]
    [SerializeField]
    private float treatInterval;
    [SerializeField]
    private float explosiveChance;
    [SerializeField]
    private float powerupInterval;
    [SerializeField]
    private float targetInterval;

    public InputActionMap inputActions;
    private PlayerHandController leftHand;
    private PlayerHandController rightHand;

    [SerializeField]
    public GameObject sweetTreat;

    private PlayerManager player;
    private void Awake()
    {
        
    }


    // Start is called before the first frame update
    void Start()
    {
        leftHand = GameObject.Find("LeftController").GetComponent<PlayerHandController>();
        rightHand = GameObject.Find("RightController").GetComponent<PlayerHandController>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();

        inputActions["LeftHandClose"].performed += _ => leftHand.CloseHand();
        inputActions["LeftHandClose"].canceled += _ => leftHand.OpenHand();
        inputActions["RightHandClose"].performed += _ => rightHand.CloseHand();
        inputActions["RightHandClose"].canceled += _ => rightHand.OpenHand();

        InvokeRepeating("SpawnTreat", 0, treatInterval);
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
        GameTime += Time.deltaTime;
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

    [SerializeField]
    float angleSpawnRange = 60;
    [SerializeField]
    private float spawnDist = 4f;
    [SerializeField]
    private float heightOffset = 0f;

    [Header("TreatSpawning")]
    [SerializeField]
    private List<GameObject> sweetTreatPrefabs;
    [SerializeField]
    private List<GameObject> explosiveTreatPrefabs;

    public void SpawnTreat()
    {
        // Select a random direction from appropriate range around player
        float randAngle = Random.Range(-angleSpawnRange, angleSpawnRange);
        Vector3 spawnDir = Quaternion.Euler(0, randAngle,0) * player.transform.forward;

        // Select a point horizon-distance away along that line
        Vector3 spawnPoint = player.transform.position + spawnDist * spawnDir.normalized + Vector3.up * (player.transform.position.y + heightOffset);

        // Determine the velocity needed from that point assuming UnityEngine.Physics.gravity to make the treat target the player
        // get velocity direction (z and x components only) from spawn direction
        // pick random angle (angle from ground) for launch, and calculate velocity magnitude accordingly 

        float radAngle;
        float velMag;
        
        radAngle = Random.Range(40, 60) * Mathf.PI / 180;
        velMag = spawnDist / (Mathf.Cos(radAngle)) * Mathf.Sqrt(Mathf.Abs(UnityEngine.Physics.gravity.y / (2)/(spawnDist * Mathf.Tan(radAngle) + heightOffset)));

        Vector3 spawnVelocity = velMag*Mathf.Cos(radAngle)*(-Vector3.Normalize(spawnDir)+Vector3.up*Mathf.Tan(radAngle));

        // Random float; if <= explosiveChance, spawn explosive treat, else sweettreat
        float f = Random.Range(0.0f, 1.0f);

        if (f < this.explosiveChance)
        {
            SpawnExplosive(spawnPoint, spawnVelocity);
        } else
        {
            SpawnSweetTreat(spawnPoint, spawnVelocity);
        }

    }
    
    public void SpawnSweetTreat(Vector3 pos, Vector3 velocity)
    {
        // TODO: determine spawning algorithm
        //TODO make this actually random
        int index = Random.Range(0, sweetTreatPrefabs.Count);
        SweetTreat newTreat = Instantiate(sweetTreatPrefabs[index], pos, Quaternion.identity).GetComponent<SweetTreat>();
        newTreat.Initialize(velocity);
    }

    public void SpawnExplosive(Vector3 pos, Vector3 velocity)
    {
        // TODO
        int index = Random.Range(0, explosiveTreatPrefabs.Count);
        ExplosiveTreat newTreat = Instantiate(explosiveTreatPrefabs[index], pos, Quaternion.identity).GetComponent<ExplosiveTreat>();
        newTreat.Initialize(velocity);
    }

    public void SpawnTarget()
    {

    }

    public void SpawnPowerup()
    {

    }

    #endregion
}
