using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShield : MonoBehaviour
{
    public PlayerManager player;

    // Start is called before the first frame update
    void Start()
    {
        player = transform.GetComponentInParent<PlayerManager>();
    }
}
