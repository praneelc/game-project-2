using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandController : MonoBehaviour
{
    public bool handOpen = true;


    private List<GameObject> catchableObjects = new();

    private readonly Vector3 holdPosition = new Vector3(0, 0, .08f);
    private GameObject heldObject;

    private Vector3 heldObjectLastPos;
    private bool throwFlag = false;
    private float throwForce = 1f;

    public void CloseHand()
    {
        Debug.Log("Hand Closing!");
        handOpen = false;

        heldObject = NearestCatchable();
        catchableObjects.Remove(heldObject);
        Debug.Log("Caught Something?: " + heldObject != null);
    }

    public void OpenHand()
    {
        Debug.Log("Hand Opening!");
        handOpen = true;

        // TODO: Release held object
        if (heldObject != null)
        {
            this.throwFlag = true;
        }
    }

    public void FreeHand()
    {
        throwFlag = false;
        heldObject = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == heldObject)
        {
            return;
        }

        if (other.gameObject.CompareTag("SweetTreat"))
        {
            Treat treat = other.gameObject.GetComponent<Treat>();
            if (treat.owner == null)
            {
                catchableObjects.Add(other.gameObject);
            }
        } else if (other.gameObject.CompareTag("ExplosiveTreat"))
        {
            ExplosiveTreat explosiveTreat = other.gameObject.GetComponent<ExplosiveTreat>();
            if (explosiveTreat.owner == null || explosiveTreat.secondaryOwner == null)
            {
                catchableObjects.Add(other.gameObject);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        catchableObjects.Remove(other.gameObject);
    }

    private void CatchTreat(GameObject caught)
    {
        caught.transform.SetParent(this.transform);
        // TODO: set local position of treat to be in your hand
        caught.transform.localPosition = holdPosition;
        Treat caughtTreat = caught.GetComponent<Treat>();
        caughtTreat.owner = this;
        caughtTreat.FreezeTreat();
    }

    public void removeTreat(Treat treat)
    {
        catchableObjects.Remove(treat.gameObject);
    }

    private GameObject NearestCatchable()
    {
        GameObject caught = null;
        float minDistance = float.MaxValue;

        foreach (GameObject obj in catchableObjects){
            if (obj == null)
            {
                Debug.LogWarning("Should not have null object in catchable objects! Length: " + catchableObjects.Count);
                continue;
            }
            float dist = Vector3.Distance(obj.transform.position, this.transform.position);

            if (dist < minDistance) {
                caught = obj;
                minDistance = dist;
            }
        }
        
        if (caught == null)
        {
            return null;
        }

        switch (caught.tag)
        {
            case "SweetTreat":
                CatchTreat(caught);
                break;
            case "ExplosiveTreat":
                {
                    ExplosiveTreat explosiveTreat = caught.GetComponent<ExplosiveTreat>();
                    if (explosiveTreat.owner == null) // this is the first hand to catch the explosive treat
                    {
                        CatchTreat(caught);
                        explosiveTreat.StartCountdown();
                    } else // this is the second hand to diffuse the explosive treat
                    {
                        explosiveTreat.secondaryOwner = this;
                        explosiveTreat.Diffuse();
                    }
                }
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
            // Check sweet or explosive
            heldObject.GetComponent<SweetTreat>().UnfreezeTreat();
            heldObject.GetComponent<Rigidbody>().AddForce((heldObject.transform.position - heldObjectLastPos) / Time.deltaTime * throwForce, ForceMode.Impulse);
            heldObject.transform.parent = null;

            heldObject = null;
            throwFlag = false;
        }


        if (heldObject != null)
        {
            heldObjectLastPos = heldObject.transform.position;
        }
    }


    // OnCollisionEnter
    // -> touchingSweet
    // OnCollisionExit
    // -> noTouch
}
