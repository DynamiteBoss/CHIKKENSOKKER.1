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
    bool déplacerGardien = false;
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
        //if (this.transform.parent.GetComponent<NetworkIdentity>().isLocalPlayer)
        //{
        if (Balle.transform.parent == null)
        {

            if (other.gameObject == true)
            {

                if (other.gameObject == Balle)
                {
                    //GetComponent<NetworkIdentity>().RebuildObservers(true);
                   // GetComponent<NetworkIdentity>().AssignClientAuthority(GetComponent<NetworkIdentity>().connectionToClient);
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
        //}

    }
    void OnChanceChange(float changement)
    {
        chance = changement;
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (déplacerGardien)
        {
            if (numéro == "2")
            {
                Gardien.transform.position += (new Vector3(Balle.transform.position.x - 0.5f, Gardien.transform.position.y, Balle.transform.position.z) - Gardien.transform.position).normalized * 0.5f;
            }
            else
            {
                Gardien.transform.position += (new Vector3(Balle.transform.position.x + 0.5f, Gardien.transform.position.y, Balle.transform.position.z) - Gardien.transform.position).normalized * 0.5f;
            }
            if ((Gardien.transform.position - Balle.transform.position).magnitude < 1 || Gardien.transform.Find("Balle"))
                déplacerGardien = false;
        }
        */
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
        //Debug.Log(Balle.GetComponent<Rigidbody>().velocity);
        Vector3 distBB = (balle.transform.position - (balle.GetComponent<PlacerBalle>().positionJouer));
        //Ray directionBalle = new Ray(Balle.transform.position, Balle.transform.position + velo);
        //Debug.DrawRay(Balle.transform.position, Balle.transform.position + velo * 3, Color.red, 1);
        float angle = Vector3.Angle(distGJ, distBB);

        //Debug.DrawRay(balle.transform.position, balle.GetComponent<PlacerBalle>().positionJouer,Color.blue,3f);
        float probabilité;
        Debug.Log(angle);
        if (angle >= 25)
        {
            probabilité = 0.95f;
            PlacerGardien(false);
        }
        else
        {

            probabilité = angle / 20;
            PlacerGardien(true);
        }


        Debug.Log(probabilité);
        //Debug.Log("chance"+chance);
        /*
        if (chance >= probabilité)
        {
            bool type;

            if (numéro == "2")
            {
                type = true;//Gardien.transform.position = new Vector3(Balle.transform.position.x - 0.5f, Balle.transform.position.y, Balle.transform.position.z);
            }
            else
            {
                type = false;
                //Gardien.transform.position = new Vector3(Balle.transform.position.x + 0.5f, Balle.transform.position.y, Balle.transform.position.z);
            }
            PlacerGardien(type);
        }
        else
        {

        }
        */
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
        //Debug.Log(Balle.GetComponent<Rigidbody>().velocity);
        Vector3 distBB = (balle.transform.position - (balle.GetComponent<PlacerBalle>().positionJouer));
        //Ray directionBalle = new Ray(Balle.transform.position, Balle.transform.position + velo);
        //Debug.DrawRay(Balle.transform.position, Balle.transform.position + velo * 3, Color.red, 1);
        float angle = Vector3.Angle(distGJ, distBB);

        //Debug.DrawRay(balle.transform.position, balle.GetComponent<PlacerBalle>().positionJouer,Color.blue,3f);
        float probabilité;
        Debug.Log(angle);
        if (angle >= 19)
        {
            probabilité = 0.95f;
        }
        else
        {

            probabilité = angle / 20;
        }
       

        Debug.Log(probabilité);
        //Debug.Log("chance"+chance);
        if (chance >= probabilité)
        {
            bool type;

            if (numéro == "2")
            {
                type = true;//Gardien.transform.position = new Vector3(Balle.transform.position.x - 0.5f, Balle.transform.position.y, Balle.transform.position.z);
            }
            else
            {
                type = false;
                //Gardien.transform.position = new Vector3(Balle.transform.position.x + 0.5f, Balle.transform.position.y, Balle.transform.position.z);
            }
            PlacerGardien(type);
        }
        else
        {

        }
        //déplacerGardien = chance == 1;
        /*if (chance == 0)
        {
            Gardien.GetComponent<ContrôleGardien>().enabled = false;
            if (numéro == "2")
            {
                déplacerGardien = true;
            }
            else
            {
                Gardien.transform.position = new Vector3(Balle.transform.position.x + 0.5f, Balle.transform.position.y, Balle.transform.position.z);
            }

        }*/

        //RaycastHit hit;
        /*if(Physics.Raycast(directionBalle, out hit))
        {
            if (hit.collider != null && (hit.collider.gameObject == GameObject.Find("But" + numéro) || hit.collider.gameObject == GameObject.Find("Gardien1" + équipe) || hit.collider.transform.parent == GameObject.Find("Gardien1" + équipe)))
            {
                transform.position = Vector3.zero;
                /*
                //Debug.Log("Rentre dans Ray");
                int chance;
                float probabilité = Mathf.Round(angle / 5);
                if(probabilité < 1)
                {
                    chance = 1;
                }
                else
                {
                    chance = (int)Random.Range(0, probabilité);
                }
                //Debug.Log("chance"+chance);

                if(chance == 1)
                {
                    if(numéro == "2")
                    {
                        Gardien.transform.position = new Vector3(Balle.transform.position.x - 0.5f,Balle.transform.position.y, Balle.transform.position.z);
                    }
                    else
                    {
                        Gardien.transform.position = new Vector3(Balle.transform.position.x + 0.5f, Balle.transform.position.y, Balle.transform.position.z);
                    }
                    
                }

            }
        }*/


    }
    [Command]
    void CmdPlacerGardien(bool équipe)
    {
        //RpcPlacerGardien(équipe);
    }

    void PlacerGardien(bool équipe)
    {
        GameObject gardien = this.transform.parent.gameObject;
        GameObject balle = GameObject.FindGameObjectWithTag("Balle");
        AnciennePosition = balle.transform.position;
       // balle.transform.parent = gardien.transform;

        gardien.GetComponent<ContrôleGardien>().enabled = false;
        if (équipe)
        {
            balle.transform.position = new Vector3(gardien.transform.position.x + 0.75f, balle.transform.position.y, gardien.transform.position.z);

           // gardien.transform.position = anciennePosition;
        }
        else
        {
            balle.transform.position = new Vector3(gardien.transform.position.x - 0.75f, balle.transform.position.y, gardien.transform.position.z);

          //  gardien.transform.position = anciennePosition;
        }
        Invoke("GérerSaut", 0.2f);
    }
    void GérerSaut()
    {
        GameObject gardien = this.transform.parent.gameObject;

        gardien.transform.position = AnciennePosition;
       

    }

}
