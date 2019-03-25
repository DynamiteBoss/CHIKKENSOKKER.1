using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//https://docs.unity3d.com/ScriptReference/WaitForSeconds.html

public class ActionsPlayer2 : MonoBehaviour
{
    Transform ZonePlacage { get; set; }
    GameObject JoueurÀPlaquer { get; set; }
    GameObject Balle { get; set; }
    float compteur = 0;
    float cptgénéral = 0;
    bool possessionBallon = false;

    void Start()
    {
        ZonePlacage = this.transform;
        //LES CHANGER DE PLACE
        //JoueurÀPlaquer = ZonePlacage.parent.Find("Balle").gameObject;
        //Balle = JoueurÀPlaquer.transform.Find("Balle").gameObject;
    }
    void Update()
    {
        possessionBallon = this.transform.parent.Find("Balle");
        compteur += Time.deltaTime;
        if (Input.GetKeyDown("p") && compteur >= 1.2f && !possessionBallon)
        {
            //bloquer le mouvement du perso pendant un certain temps //VOIR DANSFAIREPLACAGE EN BAS
            compteur = 0;
            FairePlacage();
            compteur = 0;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        //ca marche pas parce que ca appelle ce fonction la quan nimporte quoi touche a la zone de placage
        //faut genre mettre le OnTriggerEnter dans le FairePlacage ou le FrapperAdversaire
        if (other.name.StartsWith("Player") && other.gameObject != this.transform.parent.gameObject)
        {
            JoueurÀPlaquer = other.gameObject;
            Balle = GameObject.FindGameObjectWithTag("Balle");
        }
    }

    //PAS NÉCESSAIRE JE PENSE
    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.name.StartsWith("Player") && other.gameObject != this.transform.parent.gameObject)
    //    {
    //        JoueurÀPlaquer = null;
    //        Balle = null;
    //    }
    //}

    private void FairePlacage()
    {
        float rad = this.transform.parent.eulerAngles.y / 180 * Mathf.PI;
        this.transform.parent.GetComponent<Rigidbody>().AddForce(Mathf.Sin(rad) * 45, 0, Mathf.Cos(rad) * 45, ForceMode.Impulse);

        StartCoroutine(AttendreDéactivationScript(0.7f, rad));         //attendre un certain temps

        //voir si le ontrigger se trigger  
    }
    IEnumerator AttendreDéactivationScript(float durée, float direction)
    {
        //this.transform.parent.GetComponentInChildren<ContrôleBallon2>().enabled = false;    //désactiver le controle du ballon
       // this.GetComponentInParent<MouvementPlayer2>().enabled = false;    //désactiver le mouvement du player
        yield return new WaitForSeconds(durée / 3);
        this.transform.parent.GetComponent<Rigidbody>().AddForce(-(Mathf.Sin(direction) * 36.5f), 0, -(Mathf.Cos(direction) * 36.5f), ForceMode.Impulse);
        yield return new WaitForSeconds(2 * durée / 3);
        //this.transform.parent.GetComponentInChildren<ContrôleBallon2>().enabled = true;    //réactiver le controle du ballon
        //this.GetComponentInParent<MouvementPlayer2>().enabled = true;    //réactiver le mouvement du player
    }

    private void FrapperAdversaire()
    {
        if (Balle != null)
        {
            Balle.GetComponentInChildren<Rigidbody>().AddForce(new Vector3(Balle.transform.position.x - JoueurÀPlaquer.transform.parent.position.x, 0, Balle.transform.position.z - JoueurÀPlaquer.transform.parent.position.z).normalized * 30.5f, ForceMode.Impulse);
            JoueurÀPlaquer.GetComponentInChildren<Rigidbody>().AddForce(new Vector3(JoueurÀPlaquer.transform.position.x - this.transform.parent.position.x, 0, JoueurÀPlaquer.transform.position.z - this.transform.parent.position.z).normalized * 10.5f, ForceMode.Impulse);
            /**/JoueurÀPlaquer.transform.GetComponentInChildren<Rigidbody>();
        }
        else
        {
            JoueurÀPlaquer.GetComponentInChildren<Rigidbody>().AddForce(new Vector3(JoueurÀPlaquer.transform.position.x - this.transform.parent.position.x, 0, JoueurÀPlaquer.transform.position.z - this.transform.parent.position.z).normalized * 10f, ForceMode.Impulse);
        }
        //balle.transform.parent = null  LE BALLON DU JOUEUR ADVERSE VA SE FAIRE PPOUSSER DANS LA DIRECTION QUE :LE BALLON FACE, PA LA DIRECTION QUE LE JOUEUR FACE
    }
}
