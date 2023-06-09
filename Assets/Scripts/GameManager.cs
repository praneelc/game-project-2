using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public float MAX_TIME = 180f;
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
    public float timeToNextTarget { get; private set; } = 0f;

    // TODO: make sure this is false sometime
    private bool targetSpawningEnabled = true;

    public InputActionMap inputActions;
    private PlayerHandController leftHand;
    private PlayerHandController rightHand;


    private PlayerManager player;

    private Transform playerPos;


    private void Awake()
    {
        
    }


    // Start is called before the first frame update
    void Start()
    {
        playerPos = GameObject.FindGameObjectWithTag("PlayerHead").transform;

        leftHand = GameObject.Find("LeftController").GetComponent<PlayerHandController>();
        rightHand = GameObject.Find("RightController").GetComponent<PlayerHandController>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();

        inputActions["LeftHandClose"].performed += _ => leftHand.CloseHand();
        inputActions["LeftHandClose"].canceled += _ => leftHand.OpenHand();
        inputActions["RightHandClose"].performed += _ => rightHand.CloseHand();
        inputActions["RightHandClose"].canceled += _ => rightHand.OpenHand();
        inputActions["ShowUI"].performed += _ => leftHand.ShowUI();
        inputActions["ShowUI"].canceled += _ => leftHand.HideUI();
        inputActions["ToggleShield"].performed += _ => player.ToggleShield();

        InvokeRepeating("SpawnTreat", 0, treatInterval);
        InvokeRepeating("SpawnTarget", 0, targetInterval);
        InvokeRepeating("SpawnPowerup", 0, powerupInterval);
    }

    private void OnEnable()
    {
        inputActions.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        GameTime += Time.deltaTime;
        RenderSettings.skybox.SetFloat("_Rotation", Time.time * 0.5f);

        if (GameTime >= MAX_TIME || player.health <= 0)
        {
            EndGame();
        }
    }

    private void EndGame()
    {
        inputActions.Disable();
        PlayerPrefs.SetInt("Score", player.Score);
        if (player.Score > PlayerPrefs.GetInt("HighScore"))
        {
            PlayerPrefs.SetInt("HighScore", player.Score);
        }

        SceneManager.LoadScene("StartEndScene");
    }


    #region Game Sequence
    public void StartGame()
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
    private float heightOffset = -3f;

    [Header("TreatSpawning")]
    [SerializeField]
    private List<GameObject> sweetTreatPrefabs;
    [SerializeField]
    private List<GameObject> explosiveTreatPrefabs;
    [SerializeField]
    private List<GameObject> targetPrefabs;
    [SerializeField]
    private List<GameObject> powerupPrefabs;

    [SerializeField]
    private GameObject head;

    public void SpawnTreat()
    {
        // Random float; if <= explosiveChance, spawn explosive treat, else sweettreat
        bool spawnExplosive = Random.Range(0f, 1f) < explosiveChance;
        Vector3 targetPos = head.transform.position - Vector3.up * -1f + (spawnExplosive ? Vector3.up * -.1f : Random.insideUnitSphere * .5f);

        // Select a random direction from appropriate range around player
        float randAngle = Random.Range(-angleSpawnRange, angleSpawnRange);
        Vector3 spawnDir = Quaternion.Euler(0, randAngle,0) * player.transform.forward;

        // Select a point horizon-distance away along that line
        Vector3 spawnPoint = targetPos + spawnDist * spawnDir.normalized + Vector3.up * (targetPos.y);

        //Debug.Log(player.gameObject.name);
        // Determine the velocity needed from that point assuming UnityEngine.Physics.gravity to make the treat target the player
        // get velocity direction (z and x components only) from spawn direction
        // pick random angle (angle from ground) for launch, and calculate velocity magnitude accordingly 

        float radAngle;
        float velMag;
        
        radAngle = Random.Range(15f, 40f) * Mathf.PI / 180;
        velMag = spawnDist / (Mathf.Cos(radAngle)) * Mathf.Sqrt(Mathf.Abs(UnityEngine.Physics.gravity.y / (2)/(spawnDist * Mathf.Tan(radAngle) - heightOffset)));

        Vector3 spawnVelocity = velMag*Mathf.Cos(radAngle)*(-Vector3.Normalize(spawnDir)+Vector3.up*Mathf.Tan(radAngle));

        float f = Random.Range(0.0f, 1.0f);

        if (spawnExplosive)
        {
            SpawnExplosive(spawnPoint, spawnVelocity);
        } else
        {
            SpawnSweetTreat(spawnPoint, spawnVelocity);
        }

    }

    [Header("TargetSpawnRates")]
    [SerializeField]
    private float t10;
    [SerializeField]
    private float t25;
    [SerializeField]
    private float t50;
    [SerializeField]
    private float t100;

    public void SpawnSweetTreat(Vector3 pos, Vector3 velocity)
    {
        float f = Random.Range(0f, 1f);
        int index = 0;

        if (f < t10)
        {
            index = 0;
        } else if (f < t10 + t25)
        {
            index = 1;
        } else if (f < t10 + t25 + t50)
        {
            index = 2;
        } else
        {
            index = 3;
        }
        SweetTreat newTreat = Instantiate(sweetTreatPrefabs[index], pos, Quaternion.identity).GetComponent<SweetTreat>();
        newTreat.Initialize(velocity);

        newTreat.GetComponent<Rigidbody>().AddTorque(Random.insideUnitSphere * 0.5f);
    }

    public void SpawnExplosive(Vector3 pos, Vector3 velocity)
    {
        int index = Random.Range(0, explosiveTreatPrefabs.Count);
        ExplosiveTreat newTreat = Instantiate(explosiveTreatPrefabs[index], pos, Quaternion.identity).GetComponent<ExplosiveTreat>();
        newTreat.Initialize(velocity);
    }

    public void SpawnTarget()
    {
        // TODO: Target Age

        float heightOffset = Random.Range(1f, 3f);

        float randAngle = Random.Range(-angleSpawnRange, angleSpawnRange);
        Vector3 spawnDir = Quaternion.Euler(0, randAngle, 0) * player.transform.forward;

        // Select a point horizon-distance away along that line
        Vector3 spawnPoint = head.transform.position + Random.Range(1f, 4f) * spawnDir.normalized + Vector3.up * (head.transform.position.y);
        Quaternion rot = Quaternion.LookRotation(head.transform.position - spawnPoint, Vector3.up);

        int index = Random.Range(0, targetPrefabs.Count);
        Target target = Instantiate(targetPrefabs[index], spawnPoint, rot).GetComponent<Target>();
        target.Initialize(5f);

        
    }

    public void TargetDestroyed()
    {
        timeToNextTarget = 0f;
    }

    public void SpawnPowerup()
    {
        // TODO: Powerup Age

        float heightOffset = Random.Range(1f, 3f);

        float randAngle = Random.Range(-angleSpawnRange, angleSpawnRange);
        Vector3 spawnDir = Quaternion.Euler(0, randAngle, 0) * player.transform.forward;

        // Select a point horizon-distance away along that line
        Vector3 spawnPoint = head.transform.position + Random.Range(2f, 4f) * spawnDir.normalized + Vector3.up * (head.transform.position.y);
        Quaternion rot = Quaternion.LookRotation(Vector3.up * 1 + player.transform.position - spawnPoint, Vector3.up);

        int index = Random.Range(0, powerupPrefabs.Count);
        Target powerup = Instantiate(powerupPrefabs[index], spawnPoint, Quaternion.Euler(0, 180, 0)).GetComponent<Target>();
        powerup.Initialize(5f);
    }

    #endregion
}
