using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class ItemOeufBrun : NetworkBehaviour
{
    static bool enRoute;
    static string équipe = "";

    void Start()
    {

    }
    void Update()
    {
        if (enRoute)
        {
            if (équipe == "A")
            {
                this.transform.LookAt(GameObject.Find("Player1B").transform);  //je sais ca va juste viser le player 1
                this.transform.Translate(Vector3.forward * 10 * Time.deltaTime);
            }
            else if (équipe == "B")
            {
                this.transform.LookAt(GameObject.Find("Player1A").transform);  // same quen haut
                this.transform.Translate(Vector3.forward * 10 * Time.deltaTime);
            }

        }
    }

    public static void FaireEffetItem(GameObject item, GameObject player)
    {
        enRoute = true;
        if (player.name.Contains("A")) { équipe = "A"; }
        else if (player.name.Contains("B")) { équipe = "B"; }
        // OEUF A TETE CHERCHEUSE PRENDRE LANCIEN SCRIPT DE L'AI 
        // ON TRIGGER ENTER, LAUTRE JOUEUR 
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.name == "Tête" && other.transform.parent.parent.tag == "Player")
        {
            this.GetComponentInChildren<MeshRenderer>().enabled = false;
            this.GetComponentInChildren<SphereCollider>().enabled = false;

            other.transform.parent.parent.gameObject.GetComponent<MouvementPlayer>().enabled = false;
            other.transform.parent.parent.gameObject.GetComponent<MouvementManette>().enabled = false;
            other.transform.parent.parent.gameObject.GetComponent<ActionsPlayerV2>().enabled = false;
            other.transform.parent.parent.gameObject.GetComponent<ScriptItems>().enabled = false;
            StartCoroutine(AttendreRéactivationScript(other.transform.parent.parent.gameObject));

            this.GetComponentInChildren<GestionAudio>().FaireJouerSon(this.GetComponents<AudioSource>().Where(x => x.clip.name.StartsWith("PouKehh")).First());
        }
        else if ((other.name == "ZoneContrôle" || other.name == "ZonePlacage" || other.name == "Corps") && other.transform.parent.tag == "Player")
        {
            this.GetComponentInChildren<MeshRenderer>().enabled = false;
            this.GetComponentInChildren<SphereCollider>().enabled = false;

            other.transform.parent.gameObject.GetComponent<MouvementPlayer>().enabled = false;
            other.transform.parent.gameObject.GetComponent<MouvementManette>().enabled = false;
            other.transform.parent.gameObject.GetComponent<ActionsPlayerV2>().enabled = false;
            other.transform.parent.gameObject.GetComponent<ScriptItems>().enabled = false;
            StartCoroutine(AttendreRéactivationScript2(other.transform.parent.gameObject));
            this.GetComponentInChildren<GestionAudio>().FaireJouerSon(this.GetComponents<AudioSource>().Where(x => x.clip.name.StartsWith("PouKehh")).First());
        }
    }
    IEnumerator AttendreRéactivationScript(GameObject joueur)
    {
        yield return new WaitForSeconds(2.5f);
        joueur.GetComponent<MouvementPlayer>().enabled = true;
        joueur.GetComponent<MouvementManette>().enabled = true;
        joueur.GetComponent<ActionsPlayerV2>().enabled = true;
        joueur.GetComponent<ScriptItems>().enabled = true;
        Destroy(this.transform.gameObject);
    }
    IEnumerator AttendreRéactivationScript2(GameObject joueur)
    {
        yield return new WaitForSeconds(2.5f);
        joueur.GetComponent<MouvementPlayer>().enabled = true;
        joueur.GetComponent<MouvementManette>().enabled = true;
        joueur.GetComponent<ActionsPlayerV2>().enabled = true;
        joueur.GetComponent<ScriptItems>().enabled = true;
        Destroy(this.transform.gameObject);
    }
}
