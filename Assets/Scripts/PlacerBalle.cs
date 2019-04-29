using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlacerBalle : NetworkBehaviour
{
    public Vector3 positionJouer;
    public GameObject AncienGardien = null;
    public GameObject dernierPosseseur;
    const int NOMBRE_PLAYER_MAX = 4;
    public bool estPlacer = false;
    void Start()
    {
        dernierPosseseur = GameObject.Find("Joueur1A");
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.parent.tag == "Untagged")
        {
            return;
        }
        else if (other.transform.parent.tag == "Player")
        {
            if (other.tag == "ZoneC")
            {
                GameObject parent = other.transform.parent.gameObject;
                CmdMettreBalleEnfant(parent);
            }
            
            //CalculerDistanceBalle();
            //this.GetComponent<NetworkIdentity>().localPlayerAuthority = true;
        }
        else if (other.transform.parent.tag == "AI")
        {
            if (other.tag == "ZoneC")
            {
                GameObject parent = other.transform.parent.gameObject;
                TrouverJoueurÀChanger(other.transform.parent.gameObject);
                CmdMettreBalleEnfant(parent);
            }
        }
        else if (other.transform.parent.tag == "Gardien")
        {

            if (other.tag == "ZoneC")
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
    private void Update()
    {
        /*if (estPlacer)
        {
            if (transform.parent.GetComponent<NetworkIdentity>().isLocalPlayer)
                transform.localPosition = new Vector3(-0.1f, 1.5f, 2);
        }*/
    }
    [Command]
    void CmdTrouverJoueurÀChanger(GameObject aI)
    {
        //RpcTrouverJoueurÀChanger(aI);
    }
    
    void TrouverJoueurÀChanger(GameObject aI)
    {
        string tampon;
        if (dernierPosseseur.GetComponent<TypeÉquipe>().estÉquipeA == aI.GetComponent<TypeÉquipe>().estÉquipeA)
        {
            tampon = dernierPosseseur.name;
            CmdChangerAIÀJoueur(aI, dernierPosseseur, tampon);
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
            int aléatoire = Random.Range(1, grandeur);
            tampon = liste2[aléatoire - 1].name;
            CmdChangerAIÀJoueur(aI, liste2[aléatoire - 1], tampon);

        }
    }
    void CmdChangerAIÀJoueur(GameObject aI, GameObject joueur, string tampon)
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
            */
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

        CmdChangerGardienÀJoueur(joueur, gardien, tampon, grandeur, équipe);
    }
    [Command]
    void CmdChangerGardienÀJoueur(GameObject joueur,GameObject gardien, string tampon,int grandeur,string équipe)
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
