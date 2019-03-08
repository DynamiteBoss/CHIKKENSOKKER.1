using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

//https://docs.unity3d.com/ScriptReference/WaitForSeconds.html

public class ActionsPlayer : MonoBehaviour
{
    Transform ZonePlacage { get; set; }
    GameObject JoueurÀPlaquer { get; set; }
    GameObject Balle { get; set; }
    float compteur = 0;

    bool estEnMouvementPlacage = false;
    bool possessionBallon = false;

    void Start()
    {
        ZonePlacage = this.transform;
    }
    void Update()
    {
        possessionBallon = this.transform.parent.Find("Balle");
        compteur += Time.deltaTime;
        float direction = this.transform.parent.eulerAngles.y / 180 * Mathf.PI;
        if (compteur >= 0.95f)
        {
            if (this.transform.parent.GetComponent<CombinerMeshPlayer>().estÉquipeA)
            {
                if (Input.GetKeyDown("e") && !possessionBallon)
                {
                    //bloquer le mouvement du perso pendant un certain temps //VOIR DANSFAIREPLACAGE EN BAS
                    compteur = 0;
                    estEnMouvementPlacage = true;
                    FairePlacage();
                    StartCoroutine(AttendreDéactivationScriptPlaqueur(0.75f, direction));         //attendre un certain temps
                    //faire en sorte de pouvoir faire le ontriggerenter ici ou dans le FairePlacage (avant le frapperadversaire)
                }

                if (Input.GetKeyDown("q") && compteur >= 0.95f && possessionBallon)
                {
                    //bloquer le mouvement du perso pendant un certain temps //VOIR DANSFAIREPLACAGE EN BAS
                    compteur = 0;
                    FairePasse();                                                                                                             //            TANTOT
                    StartCoroutine(AttendreDéactivationScriptPlaqueur(0.75f, direction));         //attendre un certain temps
                    //faire en sorte de pouvoir faire le ontriggerenter ici ou dans le FairePlacage (avant le frapperadversaire)
                }
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.RightShift) && !possessionBallon)
                {
                    //bloquer le mouvement du perso pendant un certain temps //VOIR DANSFAIREPLACAGE EN BAS
                    compteur = 0;
                    estEnMouvementPlacage = true;
                    FairePlacage();
                    StartCoroutine(AttendreDéactivationScriptPlaqueur(0.75f, direction));         //attendre un certain temps
                    //faire en sorte de pouvoir faire le ontriggerenter ici ou dans le FairePlacage (avant le frapperadversaire)
                }

                if (Input.GetKeyDown(KeyCode.RightControl) && compteur >= 0.95f && possessionBallon)
                {
                    //bloquer le mouvement du perso pendant un certain temps //VOIR DANSFAIREPLACAGE EN BAS
                    compteur = 0;
                    FairePasse();                                                                                                             //            TANTOT
                    StartCoroutine(AttendreDéactivationScriptPlaqueur(0.75f, direction));         //attendre un certain temps
                    //faire en sorte de pouvoir faire le ontriggerenter ici ou dans le FairePlacage (avant le frapperadversaire)
                }
            }
        }
    }

    private void FairePasse()
    {
        throw new NotImplementedException();
    }

    private void OnTriggerEnter(Collider other)
    {
        //ca marche pas parce que ca appelle ce fonction la quan nimporte quoi touche a la zone de placage
        //faut genre mettre le OnTriggerEnter dans le FairePlacage ou le FrapperAdversaire
        if (other.transform.tag == "Player" && other.transform.GetComponent<CombinerMeshPlayer>().estÉquipeA != this.transform.parent.GetComponent<CombinerMeshPlayer>().estÉquipeA && other.transform.parent.gameObject != this.transform.parent.gameObject && !estEnMouvementPlacage)
        {
            float direction = this.transform.parent.eulerAngles.y / 180 * Mathf.PI;
            JoueurÀPlaquer = other.transform.parent.gameObject;

            if (JoueurÀPlaquer.transform.Find("Balle"))
            {
                Balle = JoueurÀPlaquer.transform.Find("Balle").gameObject;
            }
            JoueurÀPlaquer.GetComponent<Rigidbody>().isKinematic = false;
            FrapperAdversaire();
            StartCoroutine(AttendreDéactivationScriptPlaqué(1.1f, direction));
        }
    }

    //PAS NÉCESSAIRE JE PENSE
    /*private void OnTriggerExit(Collider other)
    {
        //ca marche pas parce que ca appelle ce fonction la quan nimporte quoi touche a la zone de placage
        //faut genre mettre le OnTriggerEnter dans le FairePlacage ou le FrapperAdversaire
        if (other.name.StartsWith("ZonePlacage") && other.transform.parent.gameObject != this.transform.parent.gameObject) //&& que FairePlacage est en cours, live 
        {
            JoueurÀPlaquer = null;
            JoueurEnDdans = other;
            //pas sûr ca ca va dans le ontriggerenter
            //FrapperAdversaire();
        }
    }*/

    private void FairePlacage()
    {
        float rad = this.transform.parent.eulerAngles.y / 180 * Mathf.PI;
        this.transform.parent.GetComponent<Rigidbody>().isKinematic = false;
        this.transform.parent.GetComponent<Rigidbody>().AddForce(Mathf.Sin(rad) * 65.5f, 0, Mathf.Cos(rad) * 65.5f, ForceMode.Impulse);
        this.transform.parent.GetComponent<Rigidbody>().drag = 10;
    }

    IEnumerator AttendreDéactivationScriptPlaqueur(float durée, float direction)
    {
        this.transform.parent.GetComponentInChildren<ContrôleBallon>().enabled = false;    //désactiver le controle du ballon
        this.GetComponentInParent<MouvementPlayer>().enabled = false;    //désactiver le mouvement du player
        yield return new WaitForSeconds(durée / 3);
        this.transform.parent.GetComponent<Rigidbody>().drag = 0;
        //this.transform.parent.GetComponent<Rigidbody>().AddForce(-(Mathf.Sin(direction) * 45.5f), 0, -(Mathf.Cos(direction) * 45.5f), ForceMode.Impulse);
        yield return new WaitForSeconds(2 * durée / 3);
        this.transform.parent.GetComponentInChildren<ContrôleBallon>().enabled = true;    //réactiver le controle du ballon
        this.GetComponentInParent<MouvementPlayer>().enabled = true;    //réactiver le mouvement du player
        this.transform.parent.GetComponent<Rigidbody>().isKinematic = true;
        estEnMouvementPlacage = false;
    }
    IEnumerator AttendreDéactivationScriptPlaqué(float durée, float direction)
    {
        JoueurÀPlaquer.GetComponentInChildren<ContrôleBallon>().enabled = false;    //désactiver le controle du ballon du player attaqué
        JoueurÀPlaquer.GetComponent<MouvementPlayer>().enabled = false;    //désactiver le mouvement du player attaqué
        yield return new WaitForSeconds(durée / 3);
        //CHANGER LE DRAG DU JOUEUR PLAQUÉ AUSSI
        JoueurÀPlaquer.GetComponent<Rigidbody>().AddForce(-(Mathf.Sin(direction) * 35), 0, -(Mathf.Cos(direction) * 35), ForceMode.Impulse);
        yield return new WaitForSeconds(2 * durée / 3);
        JoueurÀPlaquer.GetComponentInChildren<ContrôleBallon>().enabled = true;    //réactiver le controle du ballon du player attaqué
        JoueurÀPlaquer.GetComponent<MouvementPlayer>().enabled = true;    //réactiver le mouvement du player attaqués
        JoueurÀPlaquer.GetComponent<Rigidbody>().isKinematic = true;
    }
    private void FrapperAdversaire()
    {
        float direction = this.transform.parent.eulerAngles.y / 180 * Mathf.PI;
        if (Balle != null)
        {
            JoueurÀPlaquer.GetComponent<Rigidbody>().isKinematic = false;
            Balle.transform.parent = null;
            Balle.GetComponent<Rigidbody>().AddForce(Mathf.Sin(direction) * 20, 0, Mathf.Cos(direction) * 20, ForceMode.Impulse);
            JoueurÀPlaquer.GetComponent<Rigidbody>().AddForce(Mathf.Sin(direction) * 40, 0, Mathf.Cos(direction) * 40, ForceMode.Impulse);
        }
        else
        {
            JoueurÀPlaquer.GetComponent<Rigidbody>().isKinematic = false;
            JoueurÀPlaquer.GetComponent<Rigidbody>().AddForce(Mathf.Sin(direction) * 40, 0, Mathf.Cos(direction) * 40, ForceMode.Impulse);
        }
    }
}
