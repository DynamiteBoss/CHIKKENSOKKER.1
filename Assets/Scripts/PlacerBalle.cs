using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlacerBalle : NetworkBehaviour
{
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
            MettreBalleEnfant(other.transform);
            //CalculerDistanceBalle();
            //this.GetComponent<NetworkIdentity>().localPlayerAuthority = true;
        }
        else if(other.transform.parent.tag == "AI")
        {
            if (other.tag == "ZoneC")
            {
                TrouverJoueurÀChanger(other.transform.parent.gameObject);
                MettreBalleEnfant(other.transform);
            }
        }
        else if (other.transform.parent.tag == "Gardien")
        {
            
            if (other.tag == "ZoneC")
            {
                TrouverJoueurÀChangerGardien(other.transform.parent.gameObject);
                MettreBalleEnfant(other.transform);
            }
        }
    }
    private void MettreBalleEnfant(Transform other)
    {
        //changer pour pas qu'on puisse prendre le ballon  aquelquun qui la deja
        if(other.tag == "ZoneC")
        {
            Debug.Log("NOM DU COLLIDER" + other.gameObject.name.ToString());
            Debug.Log("PARENT: " + other.parent.name.ToString());
            estPlacer = true;
            //GetComponent<NetworkTransform>().enabled = false;
            this.transform.parent = other.transform.parent;
            transform.localScale = Vector3.one;
            
            this.transform.localPosition = new Vector3(0, 1.5f, 2);
            GetComponent<SphereCollider>().enabled = false;
            transform.GetComponent<Rigidbody>().isKinematic = this.transform.parent != null;
            
            
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
    void TrouverJoueurÀChanger(GameObject aI)
    {
        string tampon;
        if (dernierPosseseur.GetComponent<TypeÉquipe>().estÉquipeA == aI.GetComponent<TypeÉquipe>().estÉquipeA)
        {
            tampon = dernierPosseseur.name;
            ChangerAIÀJoueur(aI, dernierPosseseur, tampon);
        }
        else
        {
            GameObject[] liste = new GameObject[NOMBRE_PLAYER_MAX];
            List<GameObject> liste2 = new List<GameObject>();
            liste = GameObject.FindGameObjectsWithTag("Player");
            foreach(GameObject x in liste)
            {
                if(aI.GetComponent<TypeÉquipe>().estÉquipeA == x.GetComponent<TypeÉquipe>().estÉquipeA)
                {
                    liste2.Add(x);
                }
            }
            int grandeur = liste2.Count;
            int aléatoire = Random.Range(1, grandeur);
            tampon = liste2[aléatoire].name;
            ChangerAIÀJoueur(aI, liste2[aléatoire], tampon);

        }
    }
    void ChangerAIÀJoueur(GameObject aI,GameObject joueur, string tampon)
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
    void TrouverJoueurÀChangerGardien(GameObject gardien)
    {
        AncienGardien = gardien;
        string tampon;
        
        GameObject[] listeAI = new GameObject[10];
        List<GameObject> listeAIMonÉquipe = new List<GameObject>();
        listeAI = GameObject.FindGameObjectsWithTag("AI");
        string équipe;

        foreach (GameObject x in listeAI)
        {
            if(gardien.GetComponent<TypeÉquipe>().estÉquipeA == x.GetComponent<TypeÉquipe>().estÉquipeA)
            {
                listeAIMonÉquipe.Add(x);
            }
        }
        if(gardien.GetComponent<TypeÉquipe>().estÉquipeA)
        {
            équipe = "A";
        }
        else
        {
            équipe = "B";
        }
        GameObject joueur = GameObject.Find("Joueur1" + équipe);
        int grandeur = listeAIMonÉquipe.Count + 1;
        tampon = joueur.name;

        ChangerGardienÀJoueur(joueur, gardien, tampon,grandeur,équipe);

    }

    void ChangerGardienÀJoueur(GameObject joueur,GameObject gardien, string tampon,int grandeur,string équipe)
    {
        joueur.GetComponent<MouvementPlayer>().enabled = false;
        joueur.GetComponent<ScriptMouvementAI>().enabled = true;
        joueur.tag = "AI";
        joueur.name = "AI"+équipe+grandeur;
        joueur.GetComponentInChildren<Rigidbody>().isKinematic = true;

        gardien.name = tampon;
        gardien.GetComponent<MouvementPlayer>().enabled = true;
        gardien.GetComponent<ContrôleGardien>().enabled = false;
        gardien.GetComponentInChildren<ActionPlaquageGardien>().enabled = false;
        gardien.GetComponentInChildren<GérerProbabilitéArrêt>().enabled = false;
        gardien.tag = "Player";
    }
}
