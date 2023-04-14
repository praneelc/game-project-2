using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SweetTreat : Treat
{
    public bool wasThrown;
    [SerializeField]
    private float healthRestored;
    [SerializeField]
    private float fullnessRestored;

    private void HandleTreatCollision(Treat treat)
    {

    }

    private void HandlePlayerCollision(PlayerManager player)
    {
        Debug.Log("Player collided with sweet treat!");
        if (player == null)
        {
            Debug.LogError("Missing player manager script on object!");
        }
        // TODO: make treat bounce off player?
    }

    private void HandlePlayerHandCollision(PlayerHandController hand)
    {
        Debug.Log("Player hand collided with sweet treat! Is catching: " + hand.isCatching);
        if (hand == null)
        {
            Debug.LogError("Missing hand controller script on object!");
        }

        if (hand.isCatching)
        {
            owner = Object.FindObjectOfType<PlayerManager>();
            owner.ScorePoints(Points);
            owner.RestoreFullness(healthRestored);
            owner.RestoreHealth(fullnessRestored);

            transform.SetParent(hand.transform);
            // TODO: set local position to place object in palm of hand
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision with sweet treat detected");
        Collider collider = collision.collider;
        if (collider.CompareTag("PlayerHand"))
        {
            PlayerHandController hand = collider.gameObject.GetComponent<PlayerHandController>();
            HandlePlayerHandCollision(hand);
        }
        else if (collider.CompareTag("Player"))
        {
            PlayerManager player = collider.gameObject.GetComponent<PlayerManager>();
            HandlePlayerCollision(player);
        }
        Destroy(this.gameObject);
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
