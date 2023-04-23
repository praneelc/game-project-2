using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHeadController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("SweetTreat"))
        {
            Debug.Log("collided with player head");
            SweetTreat treat = other.GetComponent<SweetTreat>();
            if (treat.owner != null)
            {
                Debug.Log("owner not null");
                // Free up the hand holding the treat, then let the treat be eaten
                treat.owner.FreeHand();
                PlayerManager player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();
                player.EatTreat(treat);
            }
        }
    }
}
