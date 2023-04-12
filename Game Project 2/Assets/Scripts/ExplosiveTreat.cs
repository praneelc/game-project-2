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

    private void HandleTreatCollision(SweetTreat treat)
    {
        Debug.Log("Explosive collided with treat");
        Explode();
    }

    private void HandlePlayerCollision(PlayerManager player)
    {
        Debug.Log("Explosive collided with player: taking " + Damage + " damage");
        player.TakeDamage(Damage);
        Explode();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision with explosive detected");
        Collider collider = collision.collider;
        // TODO: add "Player" tag to player body
        // Also add "PlayerHand" tag to player left/right hand controllers and check those
        if (collider.CompareTag("Player")) {
            PlayerManager player = collider.gameObject.GetComponent<PlayerManager>();
            HandlePlayerCollision(player);
        }
        else if (collider.CompareTag("SweetTreat"))
        {
            SweetTreat treat = collider.gameObject.GetComponent<SweetTreat>();
            HandleTreatCollision(treat);
            Destroy(collider.gameObject);
        }
    }

    public void Explode()
    {
        Debug.Log("Spawning explosion");
        Instantiate(explosionVolume, transform.position, Quaternion.identity);
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
