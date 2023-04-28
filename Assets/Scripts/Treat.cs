using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Treat : MonoBehaviour
{
    public int Points;
    protected Rigidbody rb;
    public PlayerHandController owner;
    public bool wasThrown = false;

    public void Initialize(Vector3 vel)
    {
        rb = gameObject.GetComponent<Rigidbody>();
        rb.velocity = vel;
    }

    public void FreezeTreat()
    {
        rb.isKinematic = true;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
    }

    public virtual void UnfreezeTreat()
    {
        rb.isKinematic = false;
        wasThrown = true;
        rb.interpolation = RigidbodyInterpolation.None;

    }
}
