using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandController : MonoBehaviour
{
    public bool handOpen = true;


    private List<GameObject> catchableObjects = new();

    private readonly Vector3 holdPosition = new Vector3(0, 0, .08f);
    private bool isHolding { get => heldObject != null; }
    private GameObject heldObject;

    private Vector3 heldObjectLastPos;
    private bool throwFlag = false;
    private float throwForce = 1f;

    public void CloseHand()
    {
        Debug.Log("Hand Closing!");
        handOpen = false;

        heldObject = NearestCatchable();
        Debug.Log("Caught Something?: " + isHolding);
    }

    public void OpenHand()
    {
        Debug.Log("Hand Opening!");
        handOpen = true;

        // TODO: Release held object
        this.throwFlag = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        catchableObjects.Add(other.gameObject);
    }

    private GameObject NearestCatchable()
    {
        GameObject caught = null;

        float minDistance = float.MaxValue;

        foreach (GameObject obj in catchableObjects){
            float dist = Vector3.Distance(obj.transform.position, this.transform.position);

            if (dist < minDistance) {
                caught = obj;
                minDistance = dist;
            }
        }

        switch (caught.tag)
        {
            case "SweetTreat":
                caught.transform.SetParent(this.transform);
                // TODO: set local position of treat to be in your hand
                caught.transform.localPosition = holdPosition;
                caught.GetComponent<SweetTreat>().FreezeTreat();
                break;
            default:
                break;
        }

        return caught;
    }

    private void Update()
    {
        if (throwFlag)
        {
            heldObject.GetComponent<SweetTreat>().UnfreezeTreat();
            heldObject.GetComponent<Rigidbody>().AddForce((heldObject.transform.position - heldObjectLastPos) / Time.deltaTime * throwForce, ForceMode.Impulse);
            heldObject.transform.parent = null;

            heldObject = null;
            throwFlag = false;
        }


        if (isHolding)
        {
            heldObjectLastPos = heldObject.transform.position;
        }
    }


    // OnCollisionEnter
    // -> touchingSweet
    // OnCollisionExit
    // -> noTouch
}
