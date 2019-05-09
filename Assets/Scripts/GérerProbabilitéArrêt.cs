using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GérerProbabilitéArrêt : NetworkBehaviour
{
    [SyncVar(hook = "OnChanceChange")]
    public float chance;

    string numéro;
    string équipe;
    int compteur;

    GameObject Gardien { get; set; }
    GameObject Balle { get; set; }
    [SyncVar(hook = "OnAnciennePositionChange")]
    Vector3 AnciennePosition;
    // Start is called before the first frame update
    void Start()
    {
        Balle = GameObject.FindGameObjectWithTag("Balle");
        Gardien = this.transform.parent.gameObject;
    }
    void OnAnciennePositionChange(Vector3 changement)
    {
        if(transform.parent.gameObject.GetComponent<NetworkIdentity>().isLocalPlayer)
        {
            AnciennePosition = changement;
        }
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (Balle.transform.parent == null)
        {

            if (other.gameObject == true)
            {

                if (other.gameObject == Balle)
                {
                    if(!isLocalPlayer)
                    {
                        chance = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<ScriptMécaniqueMatch>().CalculerChance();
                        CalculerProbabilité();

                    }
                    else
                    {
                        chance = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<ScriptMécaniqueMatch>().CalculerChance();
                        CmdCalculerProbabilité();
                    }                     
                }
            }
        }
    }
    void OnChanceChange(float changement)
    {
        chance = changement;
    }

    [Command]
    void CmdCalculerProbabilité()
    {
        RpcCalculerProbabilité();
    }
    [ClientRpc]
    void RpcCalculerProbabilité()
    {
        GameObject balle = GameObject.FindGameObjectWithTag("Balle");
        if (Gardien.GetComponent<TypeÉquipe>().estÉquipeA)
        {
            numéro = "2";
            équipe = "A";
        }
        else
        {
            numéro = "1";
            équipe = "B";
        }
        Vector3 distGJ = (Gardien.transform.position - balle.GetComponent<PlacerBalle>().positionJouer);
        Vector3 velo = balle.GetComponent<Rigidbody>().velocity;
        Vector3 distBB = (balle.transform.position - (balle.GetComponent<PlacerBalle>().positionJouer));
        float angle = Vector3.Angle(distGJ, distBB);

        float probabilité;
        Debug.Log(angle);
        if (angle >= 25)
        {
            
            PlacerGardien(false);
        }
        else
        {

           
            PlacerGardien(true);
        }      
    }
    void CalculerProbabilité()
    {

        GameObject balle = GameObject.FindGameObjectWithTag("Balle");
        if (Gardien.GetComponent<TypeÉquipe>().estÉquipeA)
        {
            numéro = "2";
            équipe = "A";
        }
        else
        {
            numéro = "1";
            équipe = "B";
        }
        Vector3 distGJ = (Gardien.transform.position - balle.GetComponent<PlacerBalle>().positionJouer);
        Vector3 velo = balle.GetComponent<Rigidbody>().velocity;
        Vector3 distBB = (balle.transform.position - (balle.GetComponent<PlacerBalle>().positionJouer));
        
        float angle = Vector3.Angle(distGJ, distBB);

       
      
        if (angle >= 25)
        {
           
            PlacerGardien(false);
        }
        else
        {

           
            PlacerGardien(true);
        }
    }

    void PlacerGardien(bool équipe)
    {
        GameObject gardien = this.transform.parent.gameObject;
        GameObject balle = GameObject.FindGameObjectWithTag("Balle");
        AnciennePosition = balle.transform.position;
     

        gardien.GetComponent<ContrôleGardien>().enabled = false;
        if (équipe)
        {
            balle.transform.position = new Vector3(gardien.transform.position.x + 0.75f, balle.transform.position.y, gardien.transform.position.z);

           
        }
        else
        {
            balle.transform.position = new Vector3(gardien.transform.position.x - 0.75f, balle.transform.position.y, gardien.transform.position.z);

         
        }
        Invoke("GérerSaut", 0.2f);
    }
}
