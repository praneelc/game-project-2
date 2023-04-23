using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SweetTreat : Treat
{
    public bool wasThrown;
    [SerializeField]
    private float healthRestored;
    [SerializeField]
    private float fullnessRestored;
}
