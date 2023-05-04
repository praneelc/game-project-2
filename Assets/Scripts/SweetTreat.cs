using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SweetTreat : Treat
{
    public float healthRestored;
    public float fullnessRestored;

    public override void Initialize(Vector3 vel)
    {
        base.Initialize(vel);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Collider collider = collision.collider;
        if (collider.CompareTag("Ground"))
        {
            FallThrough(collider);
        }
    }
}
