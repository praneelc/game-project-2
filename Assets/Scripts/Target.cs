using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public int Points;
    private float maxAge = 5f;
    private float age = 0f;

    private Rigidbody rb;

    [SerializeField]
    private GameObject hitEffect;

    [SerializeField]
    private float yBobScale = 1;
    [SerializeField]
    private float yBobSharpness = 1f;

    private void Start()
    {
        this.rb = GetComponent<Rigidbody>();
    }

    public void Initialize(float maxAge)
    {
        this.rb = GetComponent<Rigidbody>();
        this.maxAge = maxAge;
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
            Destroy(this.gameObject);
        }
    }

    public virtual void OnHit(Collision collision)
    {
        if (collision.gameObject.CompareTag("SweetTreat"))
        {
            SweetTreat sweetTreat = collision.gameObject.GetComponent<SweetTreat>();

            if (sweetTreat.wasThrown)
            {
                PlayerManager player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();
                player.ScorePoints(this.Points);
                // GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>().TargetDestroyed


                // TODO: Test!
                StartCoroutine("PlaySound");
                Instantiate(hitEffect);

                Destroy(sweetTreat.gameObject);
            }
        }
    }


private IEnumerator PlaySound() {
    AudioSource audio = GetComponent<AudioSource>();

    audio.Play();

    while (audio.isPlaying)
    {
        yield return new WaitForEndOfFrame();
    }

    Destroy(gameObject);
}

private void FixedUpdate()
    {
        TickMovement(Time.fixedDeltaTime);
    }
}
