using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionPlaquageGardien : MonoBehaviour
{
    Transform ZonePlacage { get; set; }
    GameObject JoueurÀPlaquer { get; set; }
    GameObject Joueur { get; set; }
    float compteur = 0;
    bool estEnMouvementPlacage = false;
    bool possessionBallon = false;


    GameObject Gardien { get; set; }
    GameObject Balle { get; set; }
    // Start is called before the first frame update
    void Start()
    {
        Gardien = this.transform.parent.gameObject;
        Balle = GameObject.FindGameObjectWithTag("Balle");
    }
    private void OnTriggerEnter(Collider other)
    {
        if(Balle.transform.parent == null || other.transform.parent == null)
        {

        }
        else
        {
            Joueur = Balle.transform.parent.gameObject;
            if (other.transform.parent.name != Joueur.name)
            { }
            else
            {
                if (Joueur.name == other.transform.parent.name && compteur >= 0.95f)
                {

                    Gardien.GetComponent<ContrôleGardien>().enabled = false;
                    FairePlacage();

                    JoueurÀPlaquer = Joueur;
                    compteur = 0;
                    //estEnMouvementPlacage = true;
                    //CmdFairePlacage();
                    //FrapperAdversaire();
                    //estEnMouvementPlacage = false;
                    
                }
            }
        }       
    }
    private void FairePlacage()
    {
        var tourner = Gardien.transform.forward;
        Rigidbody corps = Gardien.GetComponent<Rigidbody>();
        GameObject body = Gardien.transform.Find("Corps").gameObject;
        corps.isKinematic = false;
        corps.AddForce(tourner*25f, ForceMode.Impulse);

        StartCoroutine(AttendreDéactivationScriptPlaqueur(0.75f,corps,body));         //attendre un certain temps
        
    }
    private void FrapperAdversaire()
    {
        float direction = this.transform.parent.eulerAngles.y / 180 * Mathf.PI;
        if (Balle != null)
        {
            Balle.GetComponent<Rigidbody>().AddForce(Mathf.Sin(direction) * 20, 0, Mathf.Cos(direction) * 20, ForceMode.Impulse);
            JoueurÀPlaquer.GetComponent<Rigidbody>().AddForce(Mathf.Sin(direction) * 30, 0, Mathf.Cos(direction) * 30, ForceMode.Impulse);

            StartCoroutine(AttendreDéactivationScriptPlaqué(1.1f, direction));
        }
        else
        {
            //JoueurÀPlaquer.GetComponentInChildren<Rigidbody>().AddForce(new Vector3(JoueurÀPlaquer.transform.position.x - this.transform.parent.position.x, 0, JoueurÀPlaquer.transform.position.z - this.transform.parent.position.z).normalized * 10f, ForceMode.Impulse);
            JoueurÀPlaquer.GetComponent<Rigidbody>().AddForce(Mathf.Sin(direction) * 30, 0, Mathf.Cos(direction) * 30, ForceMode.Impulse);

            StartCoroutine(AttendreDéactivationScriptPlaqué(1.1f, direction));
        }

    }
    IEnumerator AttendreDéactivationScriptPlaqué(float durée, float direction)
    {
        JoueurÀPlaquer.GetComponentInChildren<ContrôleBallonV2>().enabled = false;    //désactiver le controle du ballon du player attaqué
        JoueurÀPlaquer.GetComponent<MouvementPlayer>().enabled = false;    //désactiver le mouvement du player attaqué
        yield return new WaitForSeconds(durée / 3);
        JoueurÀPlaquer.GetComponent<Rigidbody>().AddForce(-(Mathf.Sin(direction) * 24), 0, -(Mathf.Cos(direction) * 24), ForceMode.Impulse);
        yield return new WaitForSeconds(2 * durée / 3);
        JoueurÀPlaquer.GetComponentInChildren<ContrôleBallonV2>().enabled = true;    //réactiver le controle du ballon du player attaqué
        JoueurÀPlaquer.GetComponent<MouvementPlayer>().enabled = true;    //réactiver le mouvement du player attaquésd
    }
    IEnumerator AttendreDéactivationScriptPlaqueur(float durée,Rigidbody corps,GameObject body)
    {
        this.GetComponentInParent<ContrôleGardien>().enabled = false;    //désactiver le mouvement du player
        yield return new WaitForSeconds(durée / 3);
        this.transform.parent.GetComponent<Rigidbody>().AddForce(0,0,1, ForceMode.Impulse);
        yield return new WaitForSeconds(2 * durée / 3);
     
        this.GetComponentInParent<ContrôleGardien>().enabled = true;    //réactiver le mouvement du player
        this.transform.rotation = Quaternion.identity;
        corps.isKinematic = true;
        body.transform.rotation = new Quaternion(0,0,0,0);
    }
    // Update is called once per frame
    void Update()
    {
        compteur += Time.deltaTime;
    }
}
