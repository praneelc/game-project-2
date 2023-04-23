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
        else if (collider.CompareTag("PlayerHead"))
        {
            if (owner != null)
            {
                // Free up the hand holding the treat, then let the treat be eaten
                bool canEat = owner.FreeHand();
                if (canEat)
                {
                    PlayerManager player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();
                    player.EatTreat(this);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
