using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionVolume : MonoBehaviour
{
    public float Damage;
    private float maxExplosiveForce;
    private float blastRadius;

    [SerializeField]
    private float expansionFactor;

    private float uniformScale = 0f;

    [Header("Audio")]
    [SerializeField]
    private AudioClip explosion;
    [SerializeField]
    private AudioClip shieldExplosion;

    private AudioSource audioSource;


    public void Initialize(float maxExplosiveForce, float blastRadius, float damage, bool onPlayer = true)
    {
        audioSource = this.GetComponent<AudioSource>();
        this.maxExplosiveForce = maxExplosiveForce;
        this.blastRadius = blastRadius;
        this.Damage = damage;

        if (onPlayer)
        {
            GameObject.FindGameObjectWithTag("PlayerHead").GetComponent<AudioSource>().PlayOneShot(explosion);
        }
        else
        {
            audioSource.PlayOneShot(shieldExplosion);
        }
    }
    
    private void TickExplosion(float delta)
    {
        uniformScale += delta * 2;
        transform.localScale = Vector3.one * uniformScale;

        if (transform.localScale.x >= blastRadius)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        // if hit player
        if (collider.gameObject.CompareTag("PlayerHead") || collider.gameObject.CompareTag("PlayerBody")) {
            PlayerManager playerManager;
            collider.transform.parent.TryGetComponent<PlayerManager>(out playerManager);
            if (playerManager != null)
            {
                playerManager.TakeDamage(CalculateDamage());
            }
        } else if (collider.gameObject.CompareTag("SweetTreat"))
        {
            Rigidbody treat;
            collider.TryGetComponent<Rigidbody>(out treat);

            if (treat != null)
            {
                Vector3 forceDir = (treat.position - this.transform.position).normalized;
                treat.AddForce(forceDir * CalculateForce(), ForceMode.Impulse);
            }
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void FixedUpdate()
    {
        TickExplosion(Time.fixedDeltaTime);
    }

    private float CalculateDamage()
    {

        return Mathf.Clamp(Mathf.Sin((1 - this.uniformScale) * Mathf.PI / 2) * Damage, 0, Damage);
    }

    private float CalculateForce() {

        return Mathf.Clamp(Mathf.Sin((1 - this.uniformScale) * Mathf.PI / 2) * maxExplosiveForce, 0, maxExplosiveForce);
    }
}
