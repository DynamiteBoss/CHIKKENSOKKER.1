using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptOeufMystère : MonoBehaviour
{
    [SerializeField]
    GameObject JoueurContact { get; set; }
    int compteur = 0;

    const int IndiceMax = 8;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.rotation = Quaternion.Euler(this.transform.rotation.eulerAngles.x + 3.1f*Mathf.Sin(180-(compteur++%360)/5.7f), this.transform.rotation.eulerAngles.y + 1, this.transform.rotation.eulerAngles.z + 2.3f*Mathf.Cos(180 - (compteur++ % 360) / 15.3f));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Tête")
        {
            Destroy(this.transform.gameObject);
            //GetComponent<ScriptMécaniqueMatch>().nbOeufs -= 1;        // PROBLEME AVEC SA CA VA PA CHERCHER A LA BONNE PLACE
            AttribuerObjetJoueur(other.transform.parent.transform.parent.gameObject, UnityEngine.Random.Range(0, IndiceMax));                      
        }
        else if (other.name == "ZoneContrôle" || other.name == "ZonePlacage" || other.name == "Corps")
        {
            Destroy(this.transform.gameObject);
            //GetComponent<ScriptMécaniqueMatch>().nbOeufs -= 1;        // PROBLEME AVEC SA CA VA PA CHERCHER A LA BONNE PLACE (((JE PENSE)))
            AttribuerObjetJoueur(other.transform.parent.gameObject, UnityEngine.Random.Range(0, IndiceMax));
        }
        else{ }

    }

    private void AttribuerObjetJoueur(GameObject joueur, int indice)
    {
        GameObject player = GameObject.Find(joueur.name);
        switch (player.GetComponent<ScriptItems>().Inventaire.Count)
        {
            case 0:
                Debug.Log(player.GetComponent<ScriptItems>().Inventaire.Count);
                player.GetComponent<ScriptItems>().Inventaire.Add(indice);
                Debug.Log(player.GetComponent<ScriptItems>().Inventaire.Count);
                return;
            case 1:
                player.GetComponent<ScriptItems>().Inventaire.Add(indice);
                return;
            case 2:
                return;
            default:
                return;
        }
        //#######################################################
        /*Version Temporaire*/
    }
}
