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
        if (other.name == "Tête" && other.transform.parent.transform.parent.tag == "Player")
        {
            Destroy(this.transform.gameObject);
            GameObject.Find("Main Camera").GetComponent<ScriptMécaniqueMatch>().nbOeufs -= 1;        // PROBLEME AVEC SA CA VA PA CHERCHER A LA BONNE PLACE
            CmdAttribuerObjetJoueur(other.transform.parent.transform.parent.gameObject, UnityEngine.Random.Range(0, IndiceMax));                      
        }
        if ((other.name == "ZoneContrôle" || other.name == "ZonePlacage" || other.name == "Corps") && other.transform.parent.tag == "Player")
        {
            Destroy(this.transform.gameObject);
            GameObject.Find("Main Camera").GetComponent<ScriptMécaniqueMatch>().nbOeufs -= 1;        // PROBLEME AVEC SA CA VA PA CHERCHER A LA BONNE PLACE (((JE PENSE)))
            CmdAttribuerObjetJoueur(other.transform.parent.gameObject, UnityEngine.Random.Range(0, IndiceMax));
        }
        else{ }

    }
    [Command]
    private void CmdAttribuerObjetJoueur(GameObject joueur, int indice)
    {
        GameObject player = GameObject.Find(joueur.name);
        if (player.name.Contains("1"))
        {
            switch (player.GetComponent<ScriptItems>().Inventaire.Count)
            {
                case 0:
                    player.GetComponent<ScriptItems>().Inventaire.Add(indice);
                    return;
                case 1:
                    player.GetComponent<ScriptItems>().Inventaire.Add(indice);
                    return;
                case 2:
                    return;
                default:
                    return;
            }
        }
        if (player.name.Contains("2"))
        {
            if (player.GetComponent<TypeÉquipe>().estÉquipeA)
            {
                switch (GameObject.Find("Joueur1A").gameObject.GetComponent<ScriptItems>().Inventaire.Count)
                {
                    case 0:
                        GameObject.Find("Joueur1A").gameObject.GetComponent<ScriptItems>().Inventaire.Add(indice);
                        return;
                    case 1:
                        GameObject.Find("Joueur1A").gameObject.GetComponent<ScriptItems>().Inventaire.Add(indice);
                        return;
                    case 2:
                        return;
                    default:
                        return;
                }
            }
            else
            {
                switch (GameObject.Find("Joueur1B").gameObject.GetComponent<ScriptItems>().Inventaire.Count)
                {
                    case 0:
                        GameObject.Find("Joueur1B").gameObject.GetComponent<ScriptItems>().Inventaire.Add(indice);
                        return;
                    case 1:
                        GameObject.Find("Joueur1B").gameObject.GetComponent<ScriptItems>().Inventaire.Add(indice);
                        return;
                    case 2:
                        return;
                    default:
                        return;
                }
            }
        }
        //#######################################################
        /*Version Temporaire*/
    }
}
