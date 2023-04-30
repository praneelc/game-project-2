using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SweetTreat : Treat
{
    [SerializeField]
    private float healthRestored;
    [SerializeField]
    private float fullnessRestored;

    public override void Initialize(Vector3 vel)
    {
        base.Initialize(vel);

        GetComponent<ExplosiveTreat>().enabled = false;
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
