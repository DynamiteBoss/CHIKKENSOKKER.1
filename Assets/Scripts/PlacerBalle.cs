using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Linq;
using System;

public class PlacerBalle : NetworkBehaviour
{
    public Vector3 positionJouer;
    [SyncVar(hook = "OnAcienGardienChange")]
    public string AncienGardien;
    [SyncVar(hook = "OnDernierPosseseurChange")]
    public string dernierPosseseur;
    const int NOMBRE_PLAYER_MAX = 4;
    public bool estPlacer = false;
    void OnDernierPosseseurChange(string change)
    {
        dernierPosseseur = change;
    }
    void OnAcienGardienChange(string change)
    {
        AncienGardien = change;
    }
    void Start()
    {
        AncienGardien = null;
        dernierPosseseur = GameObject.Find("Joueur1A").name;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.parent.tag == "Untagged")
        {
            return;
        }

        if (other.tag == "ZoneC")
        {
            if (other.transform.parent.tag == "Player")
            {

                GameObject parent = other.transform.parent.gameObject;
                CmdMettreBalleEnfant(parent);
                foreach (GameObject p in GameObject.FindGameObjectsWithTag("Player"))        //Retire les joueurs ­­­"joueur" inactifs
                {
                    if (p.name != dernierPosseseur)
                    {

                    }
                }

                //CalculerDistanceBalle();
                //this.GetComponent<NetworkIdentity>().localPlayerAuthority = true;
            }
            else if (other.transform.parent.tag == "AI")
            {
                Debug.Log("COLLI AI");

                GameObject parent = other.transform.parent.gameObject;
                CmdTrouverJoueurÀChanger(other.transform.parent.gameObject);
                CmdMettreBalleEnfant(parent);

            }
            else if (other.transform.parent.tag == "Gardien")
            {


                GameObject parent = other.transform.parent.gameObject;
                CmdTrouverJoueurÀChangerGardien(other.transform.parent.gameObject);
                CmdMettreBalleEnfant(parent);

            }
        }

    }
    [Command]
    void CmdMettreBalleEnfant(GameObject other)
    {
        RpcMettreBalleEnfant(other);
        //changer pour pas qu'on puisse prendre le ballon  aquelquun qui la deja


        /*
        estPlacer = true;
        //GetComponent<NetworkTransform>().enabled = false;
        this.transform.parent = other.transform;
        transform.localScale = Vector3.one;

        this.transform.localPosition = new Vector3(0, 1.5f, 2);
        GetComponent<SphereCollider>().enabled = false;
        transform.GetComponent<Rigidbody>().isKinematic = true;
        */






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
    [ClientRpc]
    void RpcMettreBalleEnfant(GameObject other)
    {
        estPlacer = true;
        //GetComponent<NetworkTransform>().enabled = false;
        this.transform.parent = other.transform;
        transform.localScale = Vector3.one;

        this.transform.localPosition = new Vector3(0, 1.5f, 2);
        GetComponent<SphereCollider>().enabled = false;
        transform.GetComponent<Rigidbody>().isKinematic = true;
    }
    private void CalculerDistanceBalle()
    {
        this.transform.localPosition = new Vector3(-0.1f, 1.5f, 2);
        // Balle.transform.localPosition = new Vector3(0, 1.5f, 2);

        //mettre la balle vers le milieu de la zone de controle
    }
    void RéglerCopie(GameObject joueur, GameObject copie)
    {
        string aI = "AI";
        string équipe = joueur.GetComponent<TypeÉquipe>().estÉquipeA ? "A" : "B";
        List<GameObject> vraiListe = new List<GameObject>();
        GameObject[] listeAI = GameObject.FindGameObjectsWithTag("AI");
        foreach (GameObject x in listeAI)
        {
            if (x.name.StartsWith("Joueur"))
            {

            }
            else
            {
                vraiListe.Add(x);
            }
        }
        int grandeur = (vraiListe.Count / 2) + 1;

        copie.name = aI + grandeur + équipe;
        copie.tag = aI;
        copie.GetComponent<MouvementPlayer>().enabled = false;
        copie.GetComponent<ScriptMouvementAI>().enabled = true;
    }
    void RéglerCopieAI(GameObject bon, GameObject copie, GameObject[] liste)
    {
        bool est1 = false;
        bool est2 = false;
        bool est3 = false;
        bool est4 = false;
        string équipe = bon.GetComponent<TypeÉquipe>().estÉquipeA ? "A" : "B";
        List<GameObject> listeÉquipe = new List<GameObject>();
        foreach (GameObject x in liste)
        {

            if (bon.GetComponent<TypeÉquipe>().estÉquipeA == x.GetComponent<TypeÉquipe>().estÉquipeA)
            {
                listeÉquipe.Add(x);
            }
        }
        foreach (GameObject x in listeÉquipe)
        {
            if (x.name[x.name.Length - 2] == '1')
            {
                est1 = true;
            }
            else if (x.name[x.name.Length - 2] == '2')
            {
                est2 = true;
            }
            else if (x.name[x.name.Length - 2] == '3')
            {
                est3 = true;
            }
            else if (x.name[x.name.Length - 2] == '4')
            {
                est4 = true;
            }
        }
        if (!est1)
        {
            copie.name = "AI1" + équipe;
        }
        else if (!est2)
        {
            copie.name = "AI2" + équipe;
        }
        else if (!est3)
        {
            copie.name = "AI3" + équipe;
        }
        else if (!est4)
        {
            copie.name = "AI4" + équipe;
        }
    }

    void Update()
    {
        int indice = 0;
        GameObject[] listeJoueur1 = GameObject.FindGameObjectsWithTag("Player");

        GameObject[] listeAI = GameObject.FindGameObjectsWithTag("AI");



        foreach (GameObject x in listeJoueur1)
        {
            foreach (GameObject z in listeAI)
            {
                if (x.name == z.name)
                {

                    RéglerCopie(x, z);
                }
            }
        }
        
        foreach (GameObject z in listeAI)
        {
            for (int i = 0; i < listeAI.Length; i++)
            {
                if (z.name == listeAI[i].name && i != indice)
                {
                    RéglerCopieAI(z, listeAI[i], listeAI);
                }
            }
            indice++;
        }


        if (GameObject.FindGameObjectWithTag("Balle").transform.parent != null)
        {
            foreach (GameObject x in listeJoueur1)
            {
                if (GameObject.FindGameObjectWithTag("Balle").transform.parent.gameObject.name == x.name)
                {
                    if (x != GameObject.FindGameObjectWithTag("Balle").transform.parent.gameObject)
                    {

                        RéglerCopie(GameObject.FindGameObjectWithTag("Balle").transform.parent.gameObject, x);
                    }

                }
            }
        }




        if (estPlacer)
        {
            this.transform.localPosition = new Vector3(0, 1.5f, 2);
        }
    }



    [Command]
    void CmdTrouverJoueurÀChanger(GameObject aI)
    {
        RpcTrouverJoueurÀChanger(aI);
    }
    [ClientRpc]
    void RpcTrouverJoueurÀChanger(GameObject aI)
    {
        string tampon = null;
        if (GameObject.Find(dernierPosseseur).GetComponent<TypeÉquipe>().estÉquipeA == aI.GetComponent<TypeÉquipe>().estÉquipeA)
        {
            string équipe;
            if (GameObject.Find(dernierPosseseur).GetComponent<TypeÉquipe>().estÉquipeA)
            {
                équipe = "A";
            }
            else
            {
                équipe = "B";
            }
            if (GameObject.Find(dernierPosseseur).tag == "Player")
            {
                tampon = "Joueur" + GameObject.Find(dernierPosseseur).name[GameObject.Find(dernierPosseseur).name.Length - 2] + équipe;
            }
            //tampon = dernierPosseseur.name;
            RpcChangerAIÀJoueur(aI, GameObject.Find(dernierPosseseur), tampon);
        }
        else
        {
            GameObject[] liste = new GameObject[NOMBRE_PLAYER_MAX];
            List<GameObject> liste2 = new List<GameObject>();
            liste = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject x in liste)
            {
                if (aI.GetComponent<TypeÉquipe>().estÉquipeA == x.GetComponent<TypeÉquipe>().estÉquipeA)
                {
                    liste2.Add(x);
                }
            }
            int grandeur = liste2.Count;
            int aléatoire = UnityEngine.Random.Range(1, grandeur);
            tampon = liste2[aléatoire - 1].name;
            RpcChangerAIÀJoueur(aI, liste2[aléatoire - 1], tampon);

        }
    }
    [Command]
    void CmdChangerAIÀJoueur(GameObject aI, GameObject joueur, string tampon)
    {

        RpcChangerAIÀJoueur(aI, joueur, tampon);
    }
    [ClientRpc]
    void RpcChangerAIÀJoueur(GameObject aI, GameObject joueur, string tampon)
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
    [Command]
    void CmdTrouverJoueurÀChangerGardien(GameObject gardien)
    {
        RpcTrouverJoueurÀChangerGardien(gardien);
        /*
        AncienGardien = gardien;
        string tampon;

        GameObject[] listeAI = new GameObject[10];
        List<GameObject> listeAIMonÉquipe = new List<GameObject>();
        listeAI = GameObject.FindGameObjectsWithTag("AI");
        string équipe;

        foreach (GameObject x in listeAI)
        {/*
            if(gardien.GetComponent<TypeÉquipe>().estÉquipeA == x.GetComponent<TypeÉquipe>().estÉquipeA)
            {
                listeAIMonÉquipe.Add(x);
            }
            
            listeAIMonÉquipe.Add(x);
        }
        if (gardien.GetComponent<TypeÉquipe>().estÉquipeA)
        {
            équipe = "A";
        }
        else
        {
            équipe = "B";
        }
        GameObject joueur = GameObject.Find("Joueur1" + équipe);
        int grandeur = listeAIMonÉquipe.Count / 2 + 1;
        tampon = joueur.name;

        CmdChangerGardienÀJoueur(joueur, gardien, tampon, grandeur, équipe);*/

    }
    [ClientRpc]
    void RpcTrouverJoueurÀChangerGardien(GameObject gardien)
    {

        AncienGardien = "Gardien";
        string tampon;

        GameObject[] listeAI = new GameObject[10];
        List<GameObject> listeAIMonÉquipe = new List<GameObject>();
        listeAI = GameObject.FindGameObjectsWithTag("AI");
        string équipe;

        foreach (GameObject x in listeAI)
        {/*
            if(gardien.GetComponent<TypeÉquipe>().estÉquipeA == x.GetComponent<TypeÉquipe>().estÉquipeA)
            {
                listeAIMonÉquipe.Add(x);
            }
            */
            if (x.GetComponent<TypeÉquipe>().estÉquipeA == gardien.GetComponent<TypeÉquipe>().estÉquipeA)
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
        GameObject joueur = GameObject.Find("Joueur1" + équipe);
        int grandeur = listeAIMonÉquipe.Count + 1;
        Debug.Log(grandeur);
        tampon = joueur.name;

        CmdChangerGardienÀJoueur(joueur, gardien, tampon, grandeur, équipe);
    }
    [Command]
    void CmdChangerGardienÀJoueur(GameObject joueur, GameObject gardien, string tampon, int grandeur, string équipe)
    {
        RpcChangerGardienÀJoueur(joueur, gardien, tampon, grandeur, équipe);
        /*
        joueur.GetComponent<MouvementPlayer>().enabled = false;
        joueur.GetComponent<ScriptMouvementAI>().enabled = true;
        joueur.tag = "AI";
        joueur.name = "AI" + grandeur + équipe;
        joueur.GetComponentInChildren<Rigidbody>().isKinematic = true;

        gardien.name = tampon;
        gardien.GetComponent<MouvementPlayer>().enabled = true;
        gardien.GetComponent<ContrôleGardien>().enabled = false;
        gardien.GetComponentInChildren<ActionPlaquageGardien>().enabled = false;
        gardien.GetComponentInChildren<GérerProbabilitéArrêt>().enabled = false;
        gardien.tag = "Player";
        */
    }
    [ClientRpc]
    void RpcChangerGardienÀJoueur(GameObject joueur, GameObject gardien, string tampon, int grandeur, string équipe)
    {
        joueur.GetComponent<MouvementPlayer>().enabled = false;
        joueur.GetComponent<ScriptMouvementAI>().enabled = true;
        joueur.tag = "AI";
        joueur.name = "AI" + grandeur + équipe;
        joueur.GetComponentInChildren<Rigidbody>().isKinematic = true;

        gardien.name = tampon;
        gardien.GetComponent<MouvementPlayer>().enabled = true;
        gardien.GetComponent<ContrôleGardien>().enabled = false;
        gardien.GetComponentInChildren<ActionPlaquageGardien>().enabled = false;
        gardien.GetComponentInChildren<GérerProbabilitéArrêt>().enabled = false;
        gardien.transform.Find("ZoneArrêt").GetComponent<BoxCollider>().enabled = false;
        gardien.tag = "Player";
    }
}
