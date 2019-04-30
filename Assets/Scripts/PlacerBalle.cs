using System.Collections;
using System.Linq;
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

        if(other.tag == "ZoneC")
        {
            if (other.transform.parent.tag == "Player")
            {
             
                GameObject parent = other.transform.parent.gameObject;
                CmdMettreBalleEnfant(parent);
                foreach(GameObject p in GameObject.FindGameObjectsWithTag("Player"))        //Retire les joueurs ­­­"joueur" inactifs
                {
                    if (p != dernierPosseseur)
                    {

                    }
                }

                //CalculerDistanceBalle();
                //this.GetComponent<NetworkIdentity>().localPlayerAuthority = true;
            }
            else if (other.transform.parent.tag == "AI")
            {
                
                    GameObject parent = other.transform.parent.gameObject;
                    TrouverJoueurÀChanger(other.transform.parent.gameObject);
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
    void Update()
    {
        
        GameObject[] listeJoueur = GameObject.FindGameObjectsWithTag("Player");
        
        List<GameObject> listeA = new List<GameObject>();
        List<GameObject> listeB = new List<GameObject>();

        List<GameObject> listeA1 = new List<GameObject>();
        List<GameObject> listeB1 = new List<GameObject>();
        List<GameObject> listeA2 = new List<GameObject>();
        List<GameObject> listeB2 = new List<GameObject>();

        foreach (GameObject x in listeJoueur)
        {
           if(x.GetComponent<TypeÉquipe>().estÉquipeA)
           {
                listeA.Add(x);
           }
           else
           {
                listeB.Add(x);
           }
        }
        foreach(GameObject x in listeA)
        {
            if(x.name[x.name.Length-2] == 1)
            {
                listeA1.Add(x);
            }
            if(x.name[x.name.Length-2] == 2)
            {
                listeA2.Add(x);
            }
        }
        foreach (GameObject x in listeB)
        {
            if (x.name[x.name.Length - 2] == 1)
            {
                listeB1.Add(x);
            }
            else if (x.name[x.name.Length - 2] == 2)
            {
                listeB2.Add(x);
            }
        }
        VérifierChaqueListe(listeA1, listeA2, listeB1, listeB2);
        // foreach()
        /*if (estPlacer)
        {
            if (transform.parent.GetComponent<NetworkIdentity>().isLocalPlayer)
                transform.localPosition = new Vector3(-0.1f, 1.5f, 2);
        }*/
    }
    void VérifierChaqueListe(List<GameObject> listeA1, List<GameObject> listeA2, List<GameObject> listeB1, List<GameObject> listeB2)
    {
        if (listeA1.Count == 2)
        {
            VériifierÉtat(listeA1, "A");
        }
        if (listeA2.Count == 2)
        {
            VériifierÉtat(listeA2, "A");
        }
        if (listeB1.Count == 2)
        {
            VériifierÉtat(listeB1, "B");
        }
        if (listeB2.Count == 2)
        {
            VériifierÉtat(listeB2, "B");
        }
    }
    void VériifierÉtat(List<GameObject> liste,string équipe)
    {
        GameObject balle = GameObject.FindGameObjectWithTag("Balle");
        if (balle.transform.parent != null)
        {
            foreach (GameObject x in liste)
            {
                if (x.transform.Find("Balle") == true)
                {

                }
                else
                {
                    int indice = TrouverListeAI(x);
                    x.tag = "AI";
                    x.name = "AI" + indice + équipe;
                    x.GetComponent<MouvementPlayer>().enabled = false;
                    x.GetComponent<ScriptMouvementAI>().enabled = true;
                }
            }
        }
    }
    int TrouverListeAI(GameObject joueur)
    {
        int indice = 1;
        bool estAI1 = false;
        bool estAI2 = false;
        bool estAI3 = false;
        GameObject[] listeAI = new GameObject[10];
        GameObject[] listeJoueur = GameObject.FindGameObjectsWithTag("Player");
        List<GameObject> listeJoueurÉquipe = new List<GameObject>();
        foreach (GameObject x in listeJoueur)
        {
            if(x.GetComponent<TypeÉquipe>().estÉquipeA == joueur.GetComponent<TypeÉquipe>().estÉquipeA)
            {
                listeJoueurÉquipe.Add(x);
            }
        }
        int nombreJoueur = listeJoueurÉquipe.Count;
        

        List<GameObject> bonneListe = new List<GameObject>();
        listeAI = GameObject.FindGameObjectsWithTag("AI");

        foreach(GameObject x in listeAI)
        {
            if(x.GetComponent<TypeÉquipe>().estÉquipeA == joueur.GetComponent<TypeÉquipe>().estÉquipeA)
            {
                bonneListe.Add(x);
            }
        }
        if(nombreJoueur == 2)
        {
            foreach(GameObject x in bonneListe)
            {
                if(x.name[x.name.Length - 2] == 1)
                {
                    estAI1 = false;
                }
                else if(x.name[x.name.Length - 2] == 2)
                {
                    estAI2 = false;
                }
                else if (x.name[x.name.Length - 2] == 3)
                {
                    estAI3 = false;
                }
            }
        }
        else if(nombreJoueur == 3)
        {
            foreach (GameObject x in bonneListe)
            {
                if (x.name[x.name.Length - 2] == 1)
                {
                    estAI1 = false;
                }
                else if (x.name[x.name.Length - 2] == 2)
                {
                    estAI2 = false;
                }
            }
        }
        if(estAI1)
        {
            indice = 1;
        }
        else if(estAI2)
        {
            indice = 2;
        }
        else if(estAI3)
        {
            indice = 3;
        }
        return indice;
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
            RpcChangerAIÀJoueur(aI, dernierPosseseur, tampon);
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
