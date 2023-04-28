using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveTreat : Treat
{
    public float Damage;
    [SerializeField]
    private float maxExplosiveForce;
    [SerializeField]
    private float blastRadius;
    [SerializeField]
    private GameObject explosionVolume;
    [SerializeField]
    private float explosionDamage;
    public PlayerHandController secondaryOwner;

    private float timeInHand = -1;
    private readonly float TIME_TO_EXPLODE = 0.3f;

    public bool diffuseFlag = false;

    private void HandleTreatCollision(SweetTreat treat)
    {
        if (treat == null)
        {
            Debug.LogError("Missing sweet treat script on object!");
        }

        // Explode();
    }

    public void HandlePlayerCollision(PlayerManager player)
    {
        Debug.Log("ExplosiveTreat collided with player: taking " + Damage + " damage");

        if (player == null)
        {
            Debug.LogError("Missing player manager script on object!");
        }

        player.TakeDamage(Damage);
        player.ScorePoints(Points); // lose points after getting hit by explosion
        Explode();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Collider collider = collision.collider;
        if (collider.CompareTag("PlayerBody") || collider.CompareTag("PlayerHead")) {
            PlayerManager player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();
            HandlePlayerCollision(player);
        } else if (collider.CompareTag("PlayerHand"))
        {
            StartCoroutine("StartCountdown");
        }
        else if (collider.CompareTag("SweetTreat"))
        {
            SweetTreat treat = collider.gameObject.GetComponent<SweetTreat>();
            HandleTreatCollision(treat);
        }
    }

    public IEnumerator StartCountdown()
    {
        timeInHand = 0.0f;

        while (timeInHand <= TIME_TO_EXPLODE)
        {
            timeInHand += Time.deltaTime;
            if (this.diffuseFlag) // Check for Diffuse Flag
            {
                Diffuse();
            }

            yield return new WaitForEndOfFrame();
        }

        if (owner != null)
        {
            owner.FreeHand();
            owner.removeTreat(this);
        }
        Explode();
    }

    public void Diffuse()
    {
        StopCoroutine("StartCountdown");
        if (owner == null || secondaryOwner == null)
        {
            Debug.LogWarning("Should not diffuse explosive treat! Owner is " + owner + " and secondary owner is " + secondaryOwner);
        }

        if (owner != null)
        {
            owner.FreeHand();
            owner.removeTreat(this);
        }
        if (secondaryOwner != null)
        {
            secondaryOwner.FreeHand();
            secondaryOwner.removeTreat(this);
        }
        // TODO: require that hands hold explosive for a while until it disappears
        Destroy(gameObject);
        Debug.Log("Diffused explosive treat with 2 hands");
    }

    public void Explode()
    {
        Debug.Log("Spawning explosion");
        GameObject explosion = Instantiate(explosionVolume, transform.position, Quaternion.identity);
        ExplosionVolume ev = explosion.GetComponent<ExplosionVolume>();
        ev.Initialize(maxExplosiveForce, blastRadius, explosionDamage);
        Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
}
