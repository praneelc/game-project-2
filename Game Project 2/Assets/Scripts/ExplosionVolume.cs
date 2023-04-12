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
    private float expansionRate;

    public void Initialize(float maxExplosiveForce, float blastRadius, float damage)
    {
        this.maxExplosiveForce = maxExplosiveForce;
        this.blastRadius = blastRadius;
        this.Damage = damage;
    }
    
    private void TickExplosion()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
