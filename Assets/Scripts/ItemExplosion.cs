using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemExplosion : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.name == "Tête" && other.transform.parent.parent.tag == "Player")
        {

            other.transform.parent.parent.gameObject.GetComponent<MouvementPlayer>().enabled = false;
            other.transform.parent.parent.gameObject.GetComponent<MouvementManette>().enabled = false;
            other.transform.parent.parent.gameObject.GetComponent<ActionsPlayerV2>().enabled = false;
            other.transform.parent.parent.gameObject.GetComponent<ScriptItems>().enabled = false;
            StartCoroutine(AttendreRéactivationScript(other.transform.parent.parent.gameObject));
        }
        else if ((other.name == "ZoneContrôle" || other.name == "ZonePlacage" || other.name == "Corps") && other.transform.parent.tag == "Player")
        {

            other.transform.parent.gameObject.GetComponent<MouvementPlayer>().enabled = false;
            other.transform.parent.gameObject.GetComponent<MouvementManette>().enabled = false;
            other.transform.parent.gameObject.GetComponent<ActionsPlayerV2>().enabled = false;
            other.transform.parent.gameObject.GetComponent<ScriptItems>().enabled = false;
            StartCoroutine(AttendreRéactivationScript2(other.transform.parent.gameObject));
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
