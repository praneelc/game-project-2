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

    private void HandleTreatCollision(SweetTreat treat)
    {
        Debug.Log("Explosive collided with treat");

        if (treat == null)
        {
            Debug.LogError("Missing sweet treat script on object!");
        }

        Explode();
    }

    private void HandlePlayerCollision(PlayerManager player)
    {
        Debug.Log("Explosive collided with player: taking " + Damage + " damage");

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
        // TODO: add "Player" tag to player body
        // Also add "PlayerHand" tag to player left/right hand controllers and check those
        if (collider.CompareTag("Player")) {
            PlayerManager player = collider.gameObject.GetComponent<PlayerManager>();
            HandlePlayerCollision(player);
        } else if (collider.CompareTag("PlayerHand"))
        {
            // TODO: get player and also call handle player collision
        }
        else if (collider.CompareTag("SweetTreat"))
        {
            SweetTreat treat = collider.gameObject.GetComponent<SweetTreat>();
            HandleTreatCollision(treat);
        }
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
