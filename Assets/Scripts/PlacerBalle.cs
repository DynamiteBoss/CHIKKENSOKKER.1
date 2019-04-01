using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlacerBalle : NetworkBehaviour
{
    public bool estPlacer = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.parent.tag != "Player")
        {

        }
        else
        {
            MettreBalleEnfant(other);
            //CalculerDistanceBalle();
            //this.GetComponent<NetworkIdentity>().localPlayerAuthority = true;
        }
    }
    private void MettreBalleEnfant(Collider other)
    {
        //changer pour pas qu'on puisse prendre le ballon  aquelquun qui la deja
        if(other.tag == "ZoneC")
        {
            estPlacer = true;
            //GetComponent<NetworkTransform>().enabled = false;
            this.transform.parent = other.transform.parent;
            transform.localScale = Vector3.one;
            
            this.transform.localPosition = new Vector3(0, 1.5f, 2);
          
            transform.GetComponent<Rigidbody>().isKinematic = true;
            
            
        }
        
       

        /*
        if (other.transform.parent == null)
        {
            //faire en sorte que lautre player puisse pa faire bouger le ballon
            other.transform.localScale = Vector3.one;
            other.transform.parent = ZoneContrôle.parent;
            other.transform.GetComponent<Rigidbody>().isKinematic = true;
            other.GetComponent<SphereCollider>().enabled = false;
        }
        */
    }
    private void CalculerDistanceBalle()
    {
        this.transform.localPosition = new Vector3(0, 1.5f, 2);
        // Balle.transform.localPosition = new Vector3(0, 1.5f, 2);

        //mettre la balle vers le milieu de la zone de controle
    }
    private void Update()
    {
        //if(estPlacer)
        //{
        //    if(transform.parent.GetComponent<NetworkIdentity>().isLocalPlayer)
        //    transform.localPosition = new Vector3(0, 1.5f, 2);
        //}
    }
}
