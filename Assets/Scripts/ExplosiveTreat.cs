using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveTreat : Treat
{
    public float Damage;
    public int PointsOnDiffuse;

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
    private readonly float TIME_TO_EXPLODE = 0.7f;

    public bool diffuseFlag = false;

    [Header("Audio")]
    [SerializeField]
    private AudioClip explosiveDiffused;


    private void Start()
    {
        

        
    }

    public override void Initialize(Vector3 vel)
    {
        base.Initialize(vel);
    }

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
        if (collider.CompareTag("PlayerBody") || collider.CompareTag("PlayerHead") ){
            PlayerManager player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();
            HandlePlayerCollision(player);
        } else if (collider.CompareTag("PlayerShield")) {
            collider.gameObject.GetComponent<PlayerShield>().player.AddShield(-Damage, 0);
            Explode(false);
        } else if (collider.CompareTag("PlayerHand"))
        {
            StartCoroutine("StartCountdown");
        }
        else if (collider.CompareTag("SweetTreat"))
        {
            SweetTreat treat = collider.gameObject.GetComponent<SweetTreat>();
            HandleTreatCollision(treat);
        } else if (collider.CompareTag("Ground"))
        {
            FallThrough(collider);
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


        // TODO: Determine which method actually works
        StartCoroutine("ExplosiveVanish");
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>().ScorePoints(PointsOnDiffuse);
    }

    private IEnumerator ExplosiveVanish()
    {
        transform.SetParent(null);
        rb.isKinematic = true;
        rb.velocity = Vector3.zero;

        Vector3 startScale = transform.localScale;

        float sentinel = 1f;

        AudioSource audio = this.GetComponent<AudioSource>();
        
        audio.PlayOneShot(explosiveDiffused);

        while (sentinel > 0)
        {
            sentinel = Mathf.Max(sentinel - Time.deltaTime, 0f); 

            transform.localScale = startScale * sentinel;

            yield return new WaitForEndOfFrame();
        }

        while (audio.isPlaying)
        {
            yield return new WaitForEndOfFrame();
        }

        Destroy(gameObject);
        Debug.Log("Diffused explosive treat with 2 hands");
    }

    public void Explode(bool onPlayer = true)
    {
        Debug.Log("Spawning explosion");
        GameObject explosion = Instantiate(explosionVolume, transform.position, Quaternion.identity);
        ExplosionVolume ev = explosion.GetComponent<ExplosionVolume>();
        ev.Initialize(maxExplosiveForce, blastRadius, explosionDamage, onPlayer);
        Destroy(gameObject);
    }
}
