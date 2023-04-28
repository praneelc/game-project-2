using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SweetTreat : Treat
{
    [SerializeField]
    private float healthRestored;
    [SerializeField]
    private float fullnessRestored;

    private void OnCollisionEnter(Collision collision)
    {
        Collider collider = collision.collider;
        if (collider.CompareTag("Ground"))
        {
            FallThrough(collider);
        }
    }
}
