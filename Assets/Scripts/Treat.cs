using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Treat : MonoBehaviour
{
    public int Points;
    protected Rigidbody rb;
    public PlayerManager owner;


    public void Initialize(Vector3 vel)
    {
        rb = gameObject.GetComponent<Rigidbody>();
        rb.velocity = vel;
    }
}
