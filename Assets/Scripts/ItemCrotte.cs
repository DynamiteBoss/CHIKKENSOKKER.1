using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Networking;

public class ItemCrotte : NetworkBehaviour
{

    void Start()
    {

    }
    void Update()
    {

    }

    public static void FaireEffetItem(GameObject[] listNbCrotte)
    {
        foreach (GameObject crotte in listNbCrotte)
        {

        }
        // DONNER UNE PETITE FORCE VERS L'AVANT DU JOUEUR ET LEUR METTRE UN DRAG SIGNIFICATIF
        // ON TRIGGER ENTER, LAUTRE JOUEUR SE PLANTE OU KEKCHOSE COMME CA
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.name == "Tête" && other.transform.parent.parent.tag == "Player")
        {
            this.GetComponentInChildren<MeshRenderer>().enabled = false;
            this.GetComponentInChildren<BoxCollider>().enabled = false;

            other.transform.parent.parent.gameObject.GetComponent<MouvementPlayer>().enabled = false;
            other.transform.parent.parent.gameObject.GetComponent<MouvementManette>().enabled = false;
            other.transform.parent.parent.gameObject.GetComponent<ActionsPlayerV2>().enabled = false;
            other.transform.parent.parent.gameObject.GetComponent<ScriptItems>().enabled = false;
            StartCoroutine(AttendreRéactivationScript(other.transform.parent.parent.gameObject));

            this.GetComponentInChildren<GestionAudio>().FaireJouerSon(this.GetComponents<AudioSource>().Where(x => x.clip.name.StartsWith("Caca2")).First()); //Fait jouer le son de caca quand qqun pile dessus
        }
        else if ((other.name == "ZoneContrôle" || other.name == "ZonePlacage" || other.name == "Corps") && other.transform.parent.tag == "Player")
        {
            this.GetComponentInChildren<MeshRenderer>().enabled = false;
            this.GetComponentInChildren<BoxCollider>().enabled = false;

            other.transform.parent.gameObject.GetComponent<MouvementPlayer>().enabled = false;
            other.transform.parent.gameObject.GetComponent<MouvementManette>().enabled = false;
            other.transform.parent.gameObject.GetComponent<ActionsPlayerV2>().enabled = false;
            other.transform.parent.gameObject.GetComponent<ScriptItems>().enabled = false;
            StartCoroutine(AttendreRéactivationScript2(other.transform.parent.gameObject));
            this.GetComponentInChildren<GestionAudio>().FaireJouerSon(this.GetComponents<AudioSource>().Where(x => x.clip.name.StartsWith("Caca2")).First()); //Fait jouer le son de caca quand qqun pile dessus
        }
    }
    IEnumerator AttendreRéactivationScript(GameObject joueur)
    {
        yield return new WaitForSeconds(1.5f);
        joueur.GetComponent<MouvementPlayer>().enabled = true;
        joueur.GetComponent<MouvementManette>().enabled = true;
        joueur.GetComponent<ActionsPlayerV2>().enabled = true;
        joueur.GetComponent<ScriptItems>().enabled = true;
        Destroy(this.transform.gameObject);
    }
    IEnumerator AttendreRéactivationScript2(GameObject joueur)
    {
        yield return new WaitForSeconds(1.5f);
        joueur.GetComponent<MouvementPlayer>().enabled = true;
        joueur.GetComponent<MouvementManette>().enabled = true;
        joueur.GetComponent<ActionsPlayerV2>().enabled = true;
        joueur.GetComponent<ScriptItems>().enabled = true;
        Destroy(this.transform.gameObject);
    }

}
