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
        if (collision.gameObject.CompareTag("SweetTreat"))
        {
            PlayerManager player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();
            player.AddShield(this.shield, this.shieldTime);
        }
    }
}
