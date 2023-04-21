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

    public void Initialize(float maxExplosiveForce, float blastRadius, float damage)
    {
        this.maxExplosiveForce = maxExplosiveForce;
        this.blastRadius = blastRadius;
        this.Damage = damage;
    }
    
    private void TickExplosion(float delta)
    {
        uniformScale += delta;
        transform.localScale = Vector3.one * uniformScale;

        if (transform.localScale.x >= blastRadius)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        PlayerManager playerManager;
        collider.TryGetComponent<PlayerManager>(out playerManager);

        if (playerManager != null)
        {
            playerManager.TakeDamage(Damage);
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
}
