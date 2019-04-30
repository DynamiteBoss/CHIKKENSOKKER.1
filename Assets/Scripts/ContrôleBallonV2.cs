﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ContrôleBallonV2 : NetworkBehaviour
{
    const float TEMPS_MIN = 1f;
    const float FORCE = 60f;
    GameObject Gardien { get; set; }
    GameObject ZoneC { get; set; }
    BoxCollider [] Liste { get; set; }
    GameObject Balle { get; set; }
    Transform ZoneContrôle { get; set; }
    Vector3 Courbe { get; set; }

    float compteur1 = 0f;
    // Start is called before the first frame update
    void Start()
    {
        Balle = GameObject.FindGameObjectWithTag("Balle");
        ZoneContrôle = this.transform.Find("ZoneContrôle");
    }

    // Update is called once per frame
    void Update()
    {
        Balle = GameObject.FindGameObjectWithTag("Balle");


        if (!isLocalPlayer)
        {
            return;
        }

        compteur1 += Time.deltaTime;
        //if(isLocalPlayer)
        //{
        if (tag == "Player")
        {
            if (Balle.transform.parent != null)
            {
                if (Balle.transform.parent == transform)
                {
                    if(name.StartsWith("Joueur1"))
                    {
                        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("CircleBtn1")) && compteur1 >= TEMPS_MIN)
                        {
                            CmdTirerBalle1();
                            //Invoke("CmdTirerBalle", 0.1f);
                            CmdTirerBalle();
                            //CmdTirerBalle();
                        }
                    }
                    else
                    {
                        if ((Input.GetKeyDown(KeyCode.Keypad3) || Input.GetButtonDown("CircleBtn2")) && compteur1 >= TEMPS_MIN)
                        {
                            CmdTirerBalle1();
                            //Invoke("CmdTirerBalle", 0.1f);
                            CmdTirerBalle();
                            //CmdTirerBalle();
                        }
                    }
                }
            }
        }
        //}
        
    }

   [Command]
    void CmdTirerBalle()
    {
        RpcTirer1();
        //Vector3 direction = new Vector3(Balle.transform.position.x - ZoneContrôle.transform.position.x, 0, Balle.transform.position.z - ZoneContrôle.transform.position.z).normalized;
        
        //Balle.GetComponent<Rigidbody>().AddForce(direction*FORCE, ForceMode.Impulse);
        /* if (GameObject.Find("Balle") != null)
         {
             GameObject balle = GameObject.Find("Balle");
             balle.transform.parent  = null;
             /*GameObject balle = GameObject.Find("Balle");
             balle.GetComponent<PlacerBalle>().enabled = false;
             balle.transform.GetComponent<Rigidbody>().isKinematic = false;
             StartCoroutine(AttendrePourDistanceBallon(0.4f, balle));
             balle.GetComponent<SphereCollider>().enabled = true;
             balle.transform.parent = null;
             Vector3 direction = new Vector3(balle.transform.position.x - ZoneContrôle.transform.position.x, 0, balle.transform.position.z - ZoneContrôle.transform.position.z).normalized;
             balle.GetComponent<Rigidbody>().AddForce(direction * FORCE, ForceMode.Impulse);
             balle.GetComponent<NetworkIdentity>().localPlayerAuthority = false;

         }
         */
    }
   [ClientRpc]
    void RpcTirer1()
    {
        Vector3 direction = new Vector3(Balle.transform.position.x - ZoneContrôle.transform.position.x, 0, Balle.transform.position.z - ZoneContrôle.transform.position.z).normalized;
        if (Input.GetKey(KeyCode.X) && Input.GetKeyDown(KeyCode.Space))
        {
            Balle.GetComponent<Rigidbody>().AddForce(direction * FORCE, ForceMode.Impulse);
            Balle.GetComponent<Rigidbody>().AddForce(Vector3.up * FORCE, ForceMode.Acceleration);
            Debug.DrawRay(Balle.transform.position, Vector3.back * FORCE/10, Color.black, 3);
        }
        else if (Input.GetKey(KeyCode.Z) && Input.GetKeyDown(KeyCode.Space))
        {
            Balle.GetComponent<Rigidbody>().AddForce(direction * FORCE, ForceMode.Impulse);
            Balle.GetComponent<Rigidbody>().AddForce(Vector3.up * FORCE, ForceMode.Acceleration);
            Debug.DrawRay(Balle.transform.position, Vector3.forward * FORCE/10, Color.black, 3);
        }
        else
        {
           //Debug.Log(direction);
           Balle.GetComponent<Rigidbody>().AddForce(direction.normalized * FORCE, ForceMode.Impulse);
        }

    }

    [Command]
    void CmdTirerBalle1()
    {
        RpcTirer();
        //Liste = GetComponentsInChildren<BoxCollider>();
        //foreach(BoxCollider x in Liste)
        //{
        //    if(x.transform.tag == "ZoneC")
        //    {
        //        x.enabled = false;
        //    }
        //}
        //ZoneC = GameObject.FindGameObjectWithTag("ZoneC");
        //ZoneC.GetComponent<BoxCollider>().enabled = false;
        //Balle.transform.parent = null;
       
        //foreach (GameObject x in Liste)
        //{
        //    if (x.tag == "ZoneC")
        //    {
        //        x.GetComponent<BoxCollider>().enabled = false;
        //    }
        //}
        //Balle.GetComponent<NetworkTransform>().enabled = true;
        //Balle.GetComponent<NetworkTransform>().enabled = false;
        //Balle.transform.GetComponent<Rigidbody>().isKinematic = false;
        //Balle.GetComponent<SphereCollider>().enabled = true;


        //Vector3 direction = new Vector3(Balle.transform.position.x - ZoneContrôle.transform.position.x, 0, Balle.transform.position.z - ZoneContrôle.transform.position.z).normalized;
        //Balle.GetComponent<Rigidbody>().AddForce(direction * FORCE, ForceMode.Impulse);



        //Balle.GetComponent<SphereCollider>().isTrigger = false;

        //Balle.GetComponent<PlacerBalle>().estPlacer = false;
      
        //Invoke("AttendrePourDistanceBallon", 0.1f);
        //AttendrePourDistanceBallon1(4, balle);
    }
    [ClientRpc]
    void RpcTirer()
    {
        GameObject gardien = Balle.transform.parent.gameObject;
        Balle.GetComponent<PlacerBalle>().dernierPosseseur = this.gameObject.name;
        Liste = GetComponentsInChildren<BoxCollider>();
        foreach (BoxCollider x in Liste)
        {
            if (x.transform.tag == "ZoneC")
            {
                x.enabled = false;
            }
        }
        //ZoneC = GameObject.FindGameObjectWithTag("ZoneC");
        //ZoneC.GetComponent<BoxCollider>().enabled = false;
        Balle.transform.parent = null;
        
        //foreach (GameObject x in Liste)
        //{
        //    if (x.tag == "ZoneC")
        //    {
        //        x.GetComponent<BoxCollider>().enabled = false;
        //    }
        //}
        //Balle.GetComponent<NetworkTransform>().enabled = true;
        //Balle.GetComponent<NetworkTransform>().enabled = false;
        Balle.GetComponent<Rigidbody>().isKinematic = false;
        //Balle.GetComponent<SphereCollider>().enabled = true;


        //Vector3 direction = new Vector3(Balle.transform.position.x - ZoneContrôle.transform.position.x, 0, Balle.transform.position.z - ZoneContrôle.transform.position.z).normalized;
        //Balle.GetComponent<Rigidbody>().AddForce(direction * FORCE, ForceMode.Impulse);



        //Balle.GetComponent<SphereCollider>().isTrigger = false;

        //Balle.GetComponent<PlacerBalle>().estPlacer = false;
        Balle.GetComponent<PlacerBalle>().estPlacer = false;
        Balle.GetComponent<SphereCollider>().enabled = true;
        Invoke("AttendrePourDistanceBallon", 0.5f);
        
        Balle.GetComponent<PlacerBalle>().positionJouer = this.transform.position;

        if (Balle.GetComponent<PlacerBalle>().AncienGardien != null)
        {

            Gardien = gardien;

            GameObject[] listeAI = new GameObject[8];
            List<GameObject> listeAIMonÉquipe = new List<GameObject>();
            listeAI = GameObject.FindGameObjectsWithTag("AI");
            string équipe;

            foreach (GameObject x in listeAI)
            {
                if (gardien.GetComponent<TypeÉquipe>().estÉquipeA == x.GetComponent<TypeÉquipe>().estÉquipeA)
                {
                    listeAIMonÉquipe.Add(x);
                }
            }
            if (gardien.GetComponent<TypeÉquipe>().estÉquipeA)
            {
                équipe = "A";
            }
            else
            {
                équipe = "B";
            }
            GameObject joueur = listeAIMonÉquipe[listeAIMonÉquipe.Count-1];
            ChangerJoueurÀGardien(joueur,gardien, équipe);
            Balle.GetComponent<PlacerBalle>().dernierPosseseur = GameObject.Find("Joueur1" + équipe).name;
            //tampon = joueur.name;
            
        }
        //AttendrePourDistanceBallon1(4, balle);
    }
    void RéactiverSave()
    {
        Gardien.GetComponentInChildren<GérerProbabilitéArrêt>().enabled = true;
        Gardien.transform.Find("ZoneArrêt").GetComponent<BoxCollider>().enabled = true;
    }
    void ChangerJoueurÀGardien(GameObject joueur,GameObject gardien,string équipe)
    {
        joueur.name = "Joueur1" + équipe;
        joueur.GetComponent<ScriptMouvementAI>().enabled = false;
        joueur.GetComponent<MouvementPlayer>().enabled = true;
        joueur.tag = "Player";

        gardien.name = "Gardien1" + équipe;
        gardien.transform.Find("ZoneArrêt").GetComponent<GérerProbabilitéArrêt>().enabled = false;
        gardien.GetComponent<MouvementPlayer>().enabled = false;
        gardien.GetComponent<ContrôleGardien>().enabled = true;
        gardien.GetComponentInChildren<ActionPlaquageGardien>().enabled = true;
        
        gardien.tag = "Gardien";

        Balle.GetComponent<PlacerBalle>().AncienGardien = null;
        Invoke("RéactiverSave", 0.5f);
    }

    IEnumerator AttendrePourDistanceBallon1(float durée, GameObject balle)
    {
        balle.GetComponent<SphereCollider>().isTrigger = false;
        yield return new WaitForSeconds(durée);
        balle.GetComponent<SphereCollider>().isTrigger = true;
        balle.GetComponent<PlacerBalle>().enabled = true;
    }

    void AttendrePourDistanceBallon()
    {
        //Balle.GetComponent<NetworkTransform>().enabled = true;
        //Balle.GetComponent<SphereCollider>().isTrigger = true;
        Balle.GetComponent<PlacerBalle>().enabled = true;
        //foreach (GameObject x in Liste)
        //{
        //    if (x.tag == "ZoneC")
        //    {
        //        x.GetComponent<BoxCollider>().enabled = true;
        //    }
        //}
        foreach (BoxCollider x in Liste)
        {
            if (x.transform.tag == "ZoneC")
            {
                x.enabled = true;
            }
        }
        //Balle.GetComponent<Rigidbody>().isKinematic = false;
        
        //ZoneC.GetComponent<BoxCollider>().enabled = true;
    }
}
