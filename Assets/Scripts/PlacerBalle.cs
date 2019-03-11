using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlacerBalle : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.parent.tag != "Player" || other.tag != "ZoneC")
        {

        }
        else
        {
            MettreBalleEnfant(other);
            //this.GetComponent<NetworkIdentity>().localPlayerAuthority = true;
        }
    }
    private void MettreBalleEnfant(Collider other)
    {

        transform.parent = other.transform.parent;
        transform.localScale = Vector3.one;
        transform.localPosition = new Vector3(0, 1.5f, 2);
        transform.GetComponent<Rigidbody>().isKinematic = true;

    }
}
