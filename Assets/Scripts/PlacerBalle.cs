using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlacerBalle : NetworkBehaviour
{
    const int NOMBRE_PLAYER_MAX = 4;
    public bool estPlacer = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.parent.tag == "Player")
        {
            MettreBalleEnfant(other);
            //CalculerDistanceBalle();
            //this.GetComponent<NetworkIdentity>().localPlayerAuthority = true;
        }
        if(other.transform.parent.tag == "AI")
        {
            
        }
    }
    private void MettreBalleEnfant(Collider other)
    {
        //changer pour pas qu'on puisse prendre le ballon  aquelquun qui la deja
        if(other.tag == "ZoneC" )
        {
            estPlacer = true;
            //GetComponent<NetworkTransform>().enabled = false;
            this.transform.parent = other.transform.parent;
            transform.localScale = Vector3.one;
            
            this.transform.localPosition = new Vector3(0, 1.5f, 2);
            GetComponent<SphereCollider>().enabled = false;
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
        if (estPlacer)
        {
            /*if (transform.parent.GetComponent<NetworkIdentity>().isLocalPlayer)
                transform.localPosition = new Vector3(0, 1.5f, 2);*/
        }
    }
    void TrouverJoueurÀChanger(GameObject aI)
    {
        GameObject tampon;
        GameObject dernierPosseseur = GetComponent<ContrôleBallonV2>().dernierPosseseur;
        if (dernierPosseseur.GetComponent<TypeÉquipe>().estÉquipeA == aI.GetComponent<TypeÉquipe>().estÉquipeA)
        {
            tampon = GetComponent<ContrôleBallonV2>().dernierPosseseur;
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
            tampon = liste2[aléatoire];
            ChangerAIÀJoueur(aI, liste2[aléatoire], tampon);

        }
    }
    void ChangerAIÀJoueur(GameObject aI,GameObject joueur, GameObject tampon)
    {
        joueur.GetComponent<MouvementPlayer>().enabled = false;
        joueur.GetComponent<ScriptMouvementAI>().enabled = true;
        joueur.tag = "AI";
        joueur.name = aI.name;

        aI.name = tampon.name;
        aI.GetComponent<MouvementPlayer>().enabled = true;
        aI.GetComponent<ScriptMouvementAI>().enabled = false;
        aI.tag = "Player";
    }
}
