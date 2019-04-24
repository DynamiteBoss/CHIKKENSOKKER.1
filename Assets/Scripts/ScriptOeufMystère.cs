using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ScriptOeufMystère : NetworkBehaviour
{
    [SyncVar(hook = "OnIndiceChange")]
    public int indice;
    [SerializeField]
    GameObject JoueurContact { get; set; }
    int compteur = 0;

    public const int IndiceMax = 8;

    void Start()
    {    
    }

    void Update()
    {
        this.transform.rotation = Quaternion.Euler(this.transform.rotation.eulerAngles.x + 3.1f*Mathf.Sin(180-(compteur++%360)/5.7f), this.transform.rotation.eulerAngles.y + 1, this.transform.rotation.eulerAngles.z + 2.3f*Mathf.Cos(180 - (compteur++ % 360) / 15.3f));
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.name == "Tête" && other.transform.parent.parent.tag == "Player")
        {
            Destroy(this.transform.gameObject);
            GameObject.Find("Main Camera").GetComponent<ScriptMécaniqueMatch>().nbOeufs -= 1;        // PROBLEME AVEC SA CA VA PA CHERCHER A LA BONNE PLACE
            //GetComponent<NetworkIdentity>().AssignClientAuthority(this.GetComponent<NetworkIdentity>().connectionToClient);
            //int random = UnityEngine.Random.Range(0, IndiceMax);
            indice = GameObject.FindGameObjectWithTag("Balle").GetComponent<ScriptBut>().random;
            //indice = UnityEngine.Random.Range(0, IndiceMax);
            AttribuerObjetJoueur(other.transform.parent.parent.gameObject, indice);    
        }
        else if ((other.name == "ZoneContrôle" || other.name == "ZonePlacage" || other.name == "Corps") && other.transform.parent.tag == "Player")
        {
            Destroy(this.transform.gameObject);
            GameObject.Find("Main Camera").GetComponent<ScriptMécaniqueMatch>().nbOeufs -= 1;        // PROBLEME AVEC SA CA VA PA CHERCHER A LA BONNE PLACE (((JE PENSE)))
            //GetComponent<NetworkIdentity>().AssignClientAuthority(this.GetComponent<NetworkIdentity>().connectionToClient);
            //int random = UnityEngine.Random.Range(0, IndiceMax);
            indice = GameObject.FindGameObjectWithTag("Balle").GetComponent<ScriptBut>().random;
            //ndice = UnityEngine.Random.Range(0, IndiceMax);
            AttribuerObjetJoueur(other.transform.parent.gameObject, indice);
        }
        //GetComponent<NetworkIdentity>().RemoveClientAuthority(this.GetComponent<NetworkIdentity>().connectionToClient);
    }
    [Command]
    private void CmdAttribuerObjetJoueur(GameObject joueur, int indice)
    {
        AttribuerObjetJoueur(joueur, indice);
    }

    private void AttribuerObjetJoueur(GameObject joueur, int indice)
    {
        Debug.Log(indice);
        if (joueur.GetComponent<TypeÉquipe>().estÉquipeA)
        {
            if (Inventaire.itemA2 == Inventaire.ITEMNUL && Inventaire.itemA1 != Inventaire.ITEMNUL)
            {
                Inventaire.itemA2 = indice;
                Inventaire.CmdAfficherInventaire('A', 2);
                //METTRE ITEM A2
                Debug.Log("L'item A2 a été changé en " + Inventaire.EnTexte(indice));

            }
            else if (Inventaire.itemA2 == Inventaire.ITEMNUL && Inventaire.itemA1 == Inventaire.ITEMNUL)
            {
                Inventaire.itemA1 = indice;
                Inventaire.CmdAfficherInventaire('A', 1);
                //METTRE ITEM A1
                Debug.Log("L'item A1 a été changé en " + Inventaire.EnTexte(indice));
            }
        }
        else
        {
            if (Inventaire.itemB2 == Inventaire.ITEMNUL && Inventaire.itemB1 != Inventaire.ITEMNUL)
            {
                Inventaire.itemB2 = indice;
                Inventaire.CmdAfficherInventaire('B', 2);
                Debug.Log("L'item B2 a été changé en " + Inventaire.EnTexte(indice));
                //METTRE ITEM B2
            }
            else if (Inventaire.itemB2 == Inventaire.ITEMNUL && Inventaire.itemB1 == Inventaire.ITEMNUL)
            {
                Inventaire.itemB1 = indice;
                Inventaire.CmdAfficherInventaire('B', 1);
                //METTRE ITEM B1
                Debug.Log("L'item B1 a été changé en " + Inventaire.EnTexte(indice));
            }
        }
    }
    void OnIndiceChange(int changement)
    {
        indice = changement;
    }
}
