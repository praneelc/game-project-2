using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public int Points;
    private float maxAge;
    private float age;

    private Rigidbody rb;

    [SerializeField]
    private float yBobScale = 1;
    [SerializeField]
    private float yBobSharpness = 1f;

    private void Start()
    {
        this.rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        this.OnHit(collision);
    }

    public void TickMovement(float delta)
    {
        this.rb.velocity = yBobScale * Vector3.up * Mathf.Sin(Time.time* yBobSharpness);

        this.age += delta;
        if (this.age >= this.maxAge)
        {
            //Destroy(this.gameObject);
        }
    }

    public virtual void OnHit(Collision collision)
    {
        if (collision.gameObject.CompareTag("SweetTreat"))
        {
            SweetTreat sweetTreat = collision.gameObject.GetComponent<SweetTreat>();

            if (sweetTreat.wasThrown)
            {
                sweetTreat.owner.ScorePoints(this.Points);
            }
        }
    }

    private void FixedUpdate()
    {
        TickMovement(Time.fixedDeltaTime);
    }
}
