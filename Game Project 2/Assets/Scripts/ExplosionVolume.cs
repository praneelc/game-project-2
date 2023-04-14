using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionVolume : MonoBehaviour
{
    public float Damage;
    [SerializeField]
    private float maxExplosiveForce;
    [SerializeField]
    private float blastRadius;
    [SerializeField]
    private float expansionPerSecond;

    private float uniformScale = 0f;

    public void Initialize(float maxExplosiveForce, float blastRadius, float damage)
    {
        this.maxExplosiveForce = maxExplosiveForce;
        this.blastRadius = blastRadius;
        this.Damage = damage;
    }
    
    private void TickExplosion(float delta)
    {
        uniformScale += expansionPerSecond * delta;
        transform.localScale = Vector3.one * uniformScale;
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
