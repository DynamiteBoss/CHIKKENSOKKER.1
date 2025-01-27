﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Networking;


//https://docs.unity3d.com/ScriptReference/WaitForSeconds.html

public class ActionsPlayerV2 : NetworkBehaviour
{
    string[] tags = new string[] { "Player", "AI", "Gardien" };
    Transform ZonePlacage { get; set; }
    public GameObject JoueurÀPlaquer { get; set; }
    public GameObject Balle { get; set; }

    [SerializeField]
    public static Vector3[] PositionJoueursAlliés { get; set; }
    public static GameObject[] JoueursAlliés { get; set; }
    float direction { get; set; }
    const float ForceMaxPasse = 50f;
    const float DistPasseForceMax = 50f;
    const float DistMaxPasse = 75f;
    const float ForceLobe = 2.5f;

    int compteurBoucle = 0;
    float compteur = 0;

    bool estEnMouvementPlacage = false;
    bool possessionBallon = false;


    int cpt = 0;
    int cpt2 = 150;

    void Start()
    {
        ZonePlacage = this.transform.Find("ZonePlacage");
        GameObject balle = GameObject.FindGameObjectWithTag("Balle");
    }
    void Update()
    {
        float direction = this.transform.eulerAngles.y / 180f * Mathf.PI;
       

        if (!transform.GetComponent<NetworkIdentity>().isLocalPlayer)
        {
            return;
        }
        possessionBallon = this.transform.Find("Balle");
        compteur += Time.deltaTime;

        if (transform.tag == "Player")
        {
           
            if (compteur >= 0.95f)
            {
                if (transform.name.StartsWith("Joueur1"))
                {
                    if ((Input.GetKeyDown("e") || Input.GetButtonDown("TriangleBtn1")) && !possessionBallon)
                    {
                      
                        compteur = 0;
                        estEnMouvementPlacage = true;
                        CmdFairePlacage();
                        StartCoroutine(AttendreDéactivationScriptPlaqueur(0.75f, direction));         
                                                                                                      
                    }


                    if ((Input.GetKeyDown("q") || Input.GetButtonDown("XBtn1")) && compteur >= 0.95f)
                    {
                        if (possessionBallon)
                        {
                            //bloquer le mouvement du perso pendant un certain temps //VOIR DANSFAIREPLACAGE EN BAS
                            compteur = 0;
                                                                                                                                
                            CmdFairePasse(direction);
                                
                                                                                                        
                        }
                        else
                        {
                            CmdChanger();
                            
                        }
                    }
                }
                else
                {
                    if ((Input.GetKeyDown(KeyCode.Keypad1) || Input.GetButtonDown("TriangleBtn2")) && !possessionBallon)
                    {
                       
                        compteur = 0;
                        estEnMouvementPlacage = true;
                        CmdFairePlacage();
                        StartCoroutine(AttendreDéactivationScriptPlaqueur(1f, direction));         
                                                                                                   
                    }

                    if ((Input.GetKeyDown(KeyCode.Keypad2) || Input.GetButtonDown("XBtn1")) && compteur >= 0.95f && possessionBallon)
                    {
                        
                        compteur = 0;
                                                                                                                            
                        CmdFairePasse(direction);
                              
                                                                                                  
                    }
                }
            }
        }
    }
    [Command]
    void CmdChanger()
    {
        RpcChanger();
    }
    [ClientRpc]
    void RpcChanger()
    {
        GameObject[] liste = new GameObject[10];
        List<GameObject> listeA = new List<GameObject>();
        List<GameObject> listeB = new List<GameObject>();
        foreach(string x in tags)
        {
            liste = GameObject.FindGameObjectsWithTag(x);
            foreach(GameObject z in liste)
            {
                if(z.GetComponent<TypeÉquipe>() == this.GetComponent<TypeÉquipe>().estÉquipeA)
                {
                    listeA.Add(z);
                }

            }
        }

        foreach(GameObject x in listeA)
        {
            if(x.tag == "AI")
            {
                listeB.Add(x);
            }
        }

        float distance = Vector3.Distance(transform.position, listeB[0].transform.position);
        GameObject changement = listeB[0];

        foreach (GameObject x in listeB)
        {
            float test = Vector3.Distance(transform.position, x.transform.position);
            if(test < distance)
            {
                distance = test;
                changement = x;
            }
        }
        string tampon = this.name;

        ChagerNom(this.gameObject, changement, tampon);
    }
    void ChagerNom(GameObject joueur,GameObject aI,string tampon)
    {
        joueur.GetComponent<MouvementPlayer>().enabled = false;
        joueur.GetComponent<ScriptMouvementAI>().enabled = true;
        joueur.tag = "AI";
        joueur.name = aI.name;
        joueur.GetComponentInChildren<Rigidbody>().isKinematic = true;

        aI.name = tampon;
        aI.GetComponent<MouvementPlayer>().enabled = true;
        aI.GetComponent<ScriptMouvementAI>().enabled = false;
        aI.tag = "Player";
    }
    private Vector3 TrouverPosJoueurPasse(float direction)
    {
      
        Vector3 posJoueurÀPasser = PositionJoueursAlliés[0] == null ? Vector3.up : PositionJoueursAlliés[0];

        for (int i = 1; i < JoueursAlliés.Length; i++)
        {
            PositionJoueursAlliés[i] += PositionJoueursAlliés[i] == this.transform.position ? Vector3.up : Vector3.zero;
            posJoueurÀPasser = (CalculerAngle(posJoueurÀPasser, direction) < CalculerAngle(PositionJoueursAlliés[i], direction)) ? posJoueurÀPasser : PositionJoueursAlliés[i];
        }
        if ((posJoueurÀPasser - this.transform.position).magnitude < 2)
        {
            posJoueurÀPasser = ((posJoueurÀPasser - this.transform.position).normalized * ForceLobe) + this.transform.position + Vector3.up * ForceLobe;   //Effectue un lobe
        }
        else if ((posJoueurÀPasser - this.transform.position).magnitude > DistMaxPasse)
        {
            posJoueurÀPasser = ((posJoueurÀPasser - this.transform.position).normalized * ForceLobe) + this.transform.position + Vector3.up * ForceLobe;
        }
   
        return posJoueurÀPasser;
    }

    private void TrouverPosJoueursÉquipe()
    {
        for (int i = 0; i < JoueursAlliés.Length /*Remplacer par le nb de clients*/; i++)
        {
            PositionJoueursAlliés[i] = JoueursAlliés[i].transform.Find("ZoneContrôle").position;
           
        }
    }

  
    private void FaireRencensementJoueurs()
    {
       
        JoueursAlliés = new GameObject[(GameObject.FindGameObjectsWithTag("Player").Where(x => x.GetComponent<TypeÉquipe>().estÉquipeA == this.transform.GetComponent<TypeÉquipe>()).ToArray().Length)];
        PositionJoueursAlliés = new Vector3[JoueursAlliés.Length];

        JoueursAlliés = GameObject.FindGameObjectsWithTag("Player").Where(x => x.GetComponent<TypeÉquipe>().estÉquipeA == this.transform.GetComponent<TypeÉquipe>()).ToArray();
        foreach (GameObject g in JoueursAlliés)
        {

            PositionJoueursAlliés[compteurBoucle] = g.transform.position;
            compteurBoucle++;
        }
        compteurBoucle = 0;


        

    }

    float CalculerAngle(Vector3 joueurÀPasser, float angle)
    {
        Vector3 dirPasse = joueurÀPasser - this.transform.position;
        Vector3 rotationJoueur = new Vector3(Mathf.Sin(angle), 0, Mathf.Cos(angle));

        return Vector3.Angle(rotationJoueur, dirPasse) / 180f * Mathf.PI;
    }

    void RetirerJoueurPossessionBallon()
    {
        JoueursAlliés = new GameObject[GameObject.FindGameObjectsWithTag("Player").Where(x => x.GetComponent<TypeÉquipe>().estÉquipeA == this.transform.GetComponent<TypeÉquipe>()).Count() - 1];
        GameObject.FindGameObjectsWithTag("Player").Where(x => x.GetComponent<TypeÉquipe>().estÉquipeA == this.transform.GetComponent<TypeÉquipe>()).Except(new GameObject[] { this.transform.gameObject }).ToArray();
    }

    [Command]
    void CmdFairePasse(float direction)
    {
        RpcFairePasse(direction);
    }
    [ClientRpc]
    void RpcFairePasse(float direction)
    {
        GameObject visé;


        GameObject[] liste = GameObject.FindGameObjectsWithTag("AI");
        List<GameObject> bonneListe = new List<GameObject>();
        foreach (GameObject x in liste)
        {
            if (x.GetComponent<TypeÉquipe>().estÉquipeA == transform.GetComponent<TypeÉquipe>().estÉquipeA)
            {
                bonneListe.Add(x);
            }
        }
        visé = bonneListe[0];

        float angle = CalculerAngle(visé.transform.position, direction);
        foreach (GameObject x in bonneListe)
        {

            float test = CalculerAngle(x.transform.position, direction);
            if (test < angle)
            {
                angle = test;
                visé = x;
            }
        }


        Transform balle = gameObject.transform.Find("Balle");
        Vector3 vecteurPasse = new Vector3(visé.transform.position.x - this.transform.position.x, .5f, visé.transform.position.z - this.transform.position.z).normalized;
       
        if (balle != null)
        {
            balle.GetComponent<PlacerBalle>().dernierPosseseur = this.gameObject.name;
            transform.Find("ZonePlacage").GetComponent<BoxCollider>().enabled = false;
           
            balle.transform.parent = null;
            balle.transform.GetComponent<Rigidbody>().isKinematic = false;

            balle.GetComponent<PlacerBalle>().estPlacer = false;
            balle.GetComponent<SphereCollider>().enabled = true;
           
            balle.GetComponent<PlacerBalle>().positionJouer = transform.position;
            balle.GetComponent<PlacerBalle>().enabled = true;
            Invoke("AttendreDistance", 0.1f);




           
            balle.GetComponent<Rigidbody>().AddForce(vecteurPasse * ForceMaxPasse, ForceMode.Impulse);


            
        }
    }
    void AttendreDistance()
    {
        transform.Find("ZonePlacage").GetComponent<BoxCollider>().enabled = true;
    }

    IEnumerator AttendrePourDistanceBallon(float durée)
    {
        this.transform.Find("ZoneContrôle").GetComponent<BoxCollider>().isTrigger = false;
        yield return new WaitForSeconds(durée);
        this.transform.Find("ZoneContrôle").GetComponent<BoxCollider>().isTrigger = true;
    }
   
    [Command]
    private void CmdFairePlacage()
    {
        RpcFairePlacage();
    }
    [ClientRpc]
    private void RpcFairePlacage()
    {
        this.transform.Find("ZonePlacage").GetComponent<DétectionPlacage>().estEnPlacage = true;
        this.transform.GetComponent<Rigidbody>().isKinematic = false;
        this.transform.Find("Corps").transform.GetComponent<Rigidbody>().isKinematic = false;


        float rad = this.transform.eulerAngles.y / 180 * Mathf.PI;
        this.transform.GetComponent<Rigidbody>().isKinematic = false;
        this.transform.GetComponent<Rigidbody>().AddForce(Mathf.Sin(rad) * 65.5f, 0, Mathf.Cos(rad) * 65.5f, ForceMode.Impulse);
        this.transform.GetComponent<Rigidbody>().drag = 10;
 
        PlaquerJoueur();
    }
    public void PlaquerJoueur()
    {
        if (!JoueurÀPlaquer)
            return;
        if (JoueurÀPlaquer.transform.Find("Balle"))
        {
            Balle = JoueurÀPlaquer.transform.Find("Balle").gameObject;
            Balle.transform.parent = null;
            Balle.GetComponent<Rigidbody>().AddForce(Mathf.Sin(direction) * 45f, 0, Mathf.Cos(direction) * 45f, ForceMode.Impulse);
        }
        JoueurÀPlaquer.GetComponent<Rigidbody>().isKinematic = false;
        FrapperAdversaire();
        StartCoroutine(AttendreDéactivationScriptPlaqué(1.1f, direction));
    }

    IEnumerator AttendreDéactivationScriptPlaqueur(float durée, float direction)
    {
        this.transform.GetComponentInChildren<ContrôleBallonV2>().enabled = false;    //désactiver le controle du ballon
        this.GetComponent<MouvementPlayer>().enabled = false;    //désactiver le mouvement du player
        yield return new WaitForSeconds(durée / 3);
        this.transform.GetComponent<Rigidbody>().drag = 0;
        //this.transform..GetComponent<Rigidbody>().AddForce(-(Mathf.Sin(direction) * 45.5f), 0, -(Mathf.Cos(direction) * 45.5f), ForceMode.Impulse);
        yield return new WaitForSeconds(2 * durée / 3);
        this.transform.GetComponentInChildren<ContrôleBallonV2>().enabled = true;    //réactiver le controle du ballon
        this.GetComponent<MouvementPlayer>().enabled = true;    //réactiver le mouvement du player
        this.transform.GetComponent<Rigidbody>().isKinematic = !(this.transform.GetComponent<MouvementPlayer>().modePluie | this.transform.GetComponent<MouvementPlayer>().modeGlace);
        //this.transform.Find("Corps").transform.rotation = Quaternion.identity;

        estEnMouvementPlacage = false;
        this.GetComponentInChildren<DétectionPlacage>().estEnPlacage = true;
    }
    IEnumerator AttendreDéactivationScriptPlaqué(float durée, float direction)
    {
        JoueurÀPlaquer.GetComponentInChildren<ContrôleBallonV2>().enabled = false;    //désactiver le controle du ballon du player attaqué
        JoueurÀPlaquer.GetComponent<MouvementPlayer>().enabled = false;    //désactiver le mouvement du player attaqué
        yield return new WaitForSeconds(durée / 3);
        //CHANGER LE DRAG DU JOUEUR PLAQUÉ AUSSI
        JoueurÀPlaquer.GetComponent<Rigidbody>().AddForce(-(Mathf.Sin(direction) * 35), 0, -(Mathf.Cos(direction) * 35), ForceMode.Impulse);
        yield return new WaitForSeconds(2 * durée / 3);
        JoueurÀPlaquer.GetComponentInChildren<ContrôleBallonV2>().enabled = true;    //réactiver le controle du ballon du player attaqué
        JoueurÀPlaquer.GetComponent<MouvementPlayer>().enabled = true;    //réactiver le mouvement du player attaqués
        JoueurÀPlaquer.GetComponent<Rigidbody>().isKinematic = !(this.transform.GetComponent<MouvementPlayer>().modePluie || this.transform.GetComponent<MouvementPlayer>().modeGlace);
    }
    private void FrapperAdversaire()
    {
        float direction = this.transform.eulerAngles.y / 180 * Mathf.PI;
        if (Balle != null)
        {
            JoueurÀPlaquer.GetComponent<Rigidbody>().isKinematic = false;
            Balle.transform.parent = null;
            Balle.GetComponent<Rigidbody>().AddForce(Mathf.Sin(direction) * 20, 0, Mathf.Cos(direction) * 20, ForceMode.Impulse);
            JoueurÀPlaquer.GetComponent<Rigidbody>().AddForce(Mathf.Sin(direction) * 40, 0, Mathf.Cos(direction) * 40, ForceMode.Impulse);
        }
        else
        {
            JoueurÀPlaquer.GetComponent<Rigidbody>().isKinematic = false;
            JoueurÀPlaquer.GetComponent<Rigidbody>().AddForce(Mathf.Sin(direction) * 40, 0, Mathf.Cos(direction) * 40, ForceMode.Impulse);
        }
    }
}
