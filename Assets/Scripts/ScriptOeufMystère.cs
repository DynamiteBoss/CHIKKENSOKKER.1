using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ScriptOeufMystère : NetworkBehaviour
{
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
        if (other.name == "Tête" && other.transform.parent.transform.parent.tag == "Player")
        {
            Destroy(this.transform.gameObject);
            GameObject.Find("Main Camera").GetComponent<ScriptMécaniqueMatch>().nbOeufs -= 1;        // PROBLEME AVEC SA CA VA PA CHERCHER A LA BONNE PLACE
            AttribuerObjetJoueur(other.transform.parent.transform.parent.gameObject, UnityEngine.Random.Range(0, IndiceMax));                      
        }
        if ((other.name == "ZoneContrôle" || other.name == "ZonePlacage" || other.name == "Corps") && other.transform.parent.tag == "Player")
        {
            Destroy(this.transform.gameObject);
            GameObject.Find("Main Camera").GetComponent<ScriptMécaniqueMatch>().nbOeufs -= 1;        // PROBLEME AVEC SA CA VA PA CHERCHER A LA BONNE PLACE (((JE PENSE)))
            AttribuerObjetJoueur(other.transform.parent.gameObject, UnityEngine.Random.Range(0, IndiceMax));
        }
        else{ }

    }
    private void AttribuerObjetJoueur(GameObject joueur, int indice)
    {
        GameObject player = GameObject.Find(joueur.name);
        if (player.GetComponent<TypeÉquipe>().estÉquipeA)
        {
            if (Inventaire.itemA2 == Inventaire.ITEMNUL && Inventaire.itemA1 != Inventaire.ITEMNUL)
            {
                Inventaire.itemA2 = indice;
                Inventaire.AfficherInventaire('A', 2);
                //METTRE ITEM A2
            }
            else if (Inventaire.itemA2 == Inventaire.ITEMNUL && Inventaire.itemA1 == Inventaire.ITEMNUL)
            {
                Inventaire.itemA1 = indice;
                Inventaire.AfficherInventaire('A', 1);
                //METTRE ITEM A1
            }
        }
        else
        {
            if (Inventaire.itemB2 == Inventaire.ITEMNUL && Inventaire.itemB1 != Inventaire.ITEMNUL)
            {
                Inventaire.itemB2 = indice;
                Inventaire.AfficherInventaire('B', 2);
                //METTRE ITEM B2
            }
            else if (Inventaire.itemB2 == Inventaire.ITEMNUL && Inventaire.itemB1 == Inventaire.ITEMNUL)
            {
                Inventaire.itemB1 = indice;
                Inventaire.AfficherInventaire('B', 1);
                //METTRE ITEM B1
            }
        }
    }
}
