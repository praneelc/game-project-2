using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : Target
{
    [SerializeField]
    private float shield = 30f;
    [SerializeField]
    private float shieldTime = 10f;


    public override void OnHit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerManager>().AddShield(this.shield, this.shieldTime);
        }
    }
}
