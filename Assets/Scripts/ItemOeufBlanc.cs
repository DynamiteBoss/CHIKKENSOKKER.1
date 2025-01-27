﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Networking;

public class ItemOeufBlanc : NetworkBehaviour
{
    void Start()
    {

    }
    void Update()
    {

    }

    public static void FaireEffetItem(int indice, GameObject[] listNbOeuf)
    {
        if (indice == 0) // DONC OEUFBLANCGROS
        {
            foreach(GameObject oeuf in listNbOeuf)
            {
                oeuf.transform.localScale = new Vector3(5, 5, 5);
            }
        }
        foreach(GameObject oeuf in listNbOeuf)
        {
            oeuf.GetComponent<Rigidbody>().AddForce(oeuf.transform.up * 1500, ForceMode.Force);
        }

        // PAS DE ELSE PARCE QUE OEUFBLANC3 **ET** OEUFBLANCGROS VONT FAIRE LE RESTE DU SCRIPT

        // OEUF QUI VA SE PARTIR DE L'AVANT DU JOUEUR PI QUI VA REBONDIR SUR LES MURS (DONC ***PAS*** DE DRAG)
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

            this.GetComponentInChildren<GestionAudio>().FaireJouerSon(this.GetComponents<AudioSource>().Where(x => x.clip.name.StartsWith("Oeuf - Son")).First());
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

            this.GetComponentInChildren<GestionAudio>().FaireJouerSon(this.GetComponents<AudioSource>().Where(x => x.clip.name.StartsWith("Oeuf - Son")).First());
        }
    }
    IEnumerator AttendreRéactivationScript(GameObject joueur)
    {
        yield return new WaitForSeconds(2);
        joueur.GetComponent<MouvementPlayer>().enabled = true;
        joueur.GetComponent<MouvementManette>().enabled = true;
        joueur.GetComponent<ActionsPlayerV2>().enabled = true;
        joueur.GetComponent<ScriptItems>().enabled = true;
        Destroy(this.transform.gameObject);
    }
    IEnumerator AttendreRéactivationScript2(GameObject joueur)
    {
        yield return new WaitForSeconds(2);
        joueur.GetComponent<MouvementPlayer>().enabled = true;
        joueur.GetComponent<MouvementManette>().enabled = true;
        joueur.GetComponent<ActionsPlayerV2>().enabled = true;
        joueur.GetComponent<ScriptItems>().enabled = true;
        Destroy(this.transform.gameObject);
    }
}
