using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Treat : MonoBehaviour
{
    public int Points;
    public Rigidbody rb;
    public PlayerManager owner;

    void TickMovement(float deltaTime)
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
