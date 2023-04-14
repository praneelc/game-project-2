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

    public void FreezeTreat()
    {
        base.rb.isKinematic = true;
        base.rb.velocity = Vector3.zero;
        base.rb.angularVelocity = Vector3.zero;
        // TODO: add gravity in unity
    }

    public void UnfreezeTreat()
    {
        rb.isKinematic = false;
        // TODO rb.useGravity = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision with sweet treat detected");
        Collider collider = collision.collider;
        if (collider.CompareTag("PlayerHand"))
        {
            PlayerHandController hand = collider.gameObject.GetComponent<PlayerHandController>();
        }
        else if (collider.CompareTag("Player"))
        {
            PlayerManager player = collider.gameObject.GetComponent<PlayerManager>();
            HandlePlayerCollision(player);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
