using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionsPlayerManette : MonoBehaviour
{
    const string NOM_PLAYER_1 = "Player (1)";
    const string NOM_PLAYER_2 = "Player (2)";
    string Name { get; set; }
    int Number { get; set; }
    Transform ZonePlacage { get; set; }
    GameObject JoueurÀPlaquer { get; set; }
    GameObject Balle { get; set; }
    float compteur = 0;
    bool estEnMouvementPlacage = false;
    bool possessionBallon = false;

    void Start()
    {
        Balle = GameObject.FindGameObjectWithTag("Balle");
        Name = transform.parent.name;
        if(Name == NOM_PLAYER_1)
        {
            Number = 1;
        }
        else
        {
            if(Name == NOM_PLAYER_2)
            {
                Number = 2;
            }
        }
        ZonePlacage = this.transform;
    }
    void Update()
    {
        possessionBallon = Balle.transform.parent;
        compteur += Time.deltaTime;
        if(Name == NOM_PLAYER_1 || Name == NOM_PLAYER_2)
        {
            if (Input.GetButtonDown("SquareBtn" + Number.ToString()) && compteur >= 0.95f && !possessionBallon)
            {
                //bloquer le mouvement du perso pendant un certain temps //VOIR DANSFAIREPLACAGE EN BAS
                compteur = 0;
                estEnMouvementPlacage = true;
                FairePlacage();
                //faire en sorte de pouvoir faire le ontriggerenter ici ou dans le FairePlacage (avant le frapperadversaire)
                FrapperAdversaire();
                estEnMouvementPlacage = false;
            }
        }
       
    }

    private void OnTriggerEnter(Collider other)
    {
        //ca marche pas parce que ca appelle ce fonction la quan nimporte quoi touche a la zone de placage
        //faut genre mettre le OnTriggerEnter dans le FairePlacage ou le FrapperAdversaire
        if (other.transform.parent.tag == "Player" && other.transform.parent.gameObject != this.transform.parent.gameObject /*&& que FairePlacage est en cours, live */)
        {
            JoueurÀPlaquer = other.transform.parent.gameObject;
            if (other.transform.Find("Balle"))
            {
                Balle = other.transform.Find("Balle").gameObject;
            }

            //pas sûr ca ca va dans le ontriggerenter
            //FrapperAdversaire();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //ca marche pas parce que ca appelle ce fonction la quan nimporte quoi touche a la zone de placage
        //faut genre mettre le OnTriggerEnter dans le FairePlacage ou le FrapperAdversaire
        if (other.name.StartsWith("ZonePlacage") && other.transform.parent.gameObject != this.transform.parent.gameObject /*&& que FairePlacage est en cours, live */)
        {
            JoueurÀPlaquer = null;

            //pas sûr ca ca va dans le ontriggerenter
            //FrapperAdversaire();
        }
    }

    //PAS NÉCESSAIRE JE PENSE
    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.name.StartsWith("Player") && other.gameObject != this.gameObject)
    //    {
    //        JoueurÀPlaquer = null;
    //        Balle = null;
    //    }
    //}

    private void FairePlacage()
    {
        float rad = this.transform.parent.eulerAngles.y / 180 * Mathf.PI;
        this.transform.parent.GetComponent<Rigidbody>().AddForce(Mathf.Sin(rad) * 45, 0, Mathf.Cos(rad) * 45, ForceMode.Impulse);

        StartCoroutine(AttendreDéactivationScriptPlaqueur(0.75f, rad));         //attendre un certain temps
    }
    IEnumerator AttendreDéactivationScriptPlaqueur(float durée, float direction)
    {
        this.transform.parent.GetComponentInChildren<ContrôleBallonV2>().enabled = false;    //désactiver le controle du ballon
        this.GetComponentInParent<MouvementPlayer>().enabled = false;    //désactiver le mouvement du player
        yield return new WaitForSeconds(durée / 3);
        this.transform.parent.GetComponent<Rigidbody>().AddForce(-(Mathf.Sin(direction) * 36.5f), 0, -(Mathf.Cos(direction) * 36.5f), ForceMode.Impulse);
        yield return new WaitForSeconds(2 * durée / 3);
        this.transform.parent.GetComponentInChildren<ContrôleBallonV2>().enabled = true;    //réactiver le controle du ballon
        this.GetComponentInParent<MouvementPlayer>().enabled = true;    //réactiver le mouvement du player
    }
    IEnumerator AttendreDéactivationScriptPlaqué(float durée, float direction)
    {
        //JoueurÀPlaquer.GetComponentInChildren<ContrôleBallon2>().enabled = false;    //désactiver le controle du ballon du player attaqué
        //JoueurÀPlaquer.GetComponent<MouvementPlayer2>().enabled = false;    //désactiver le mouvement du player attaqué
        yield return new WaitForSeconds(durée / 3);
        JoueurÀPlaquer.GetComponent<Rigidbody>().AddForce(-(Mathf.Sin(direction) * 24), 0, -(Mathf.Cos(direction) * 24), ForceMode.Impulse);
        yield return new WaitForSeconds(2 * durée / 3);
        //JoueurÀPlaquer.GetComponentInChildren<ContrôleBallon2>().enabled = true;    //réactiver le controle du ballon du player attaqué
        //JoueurÀPlaquer.GetComponent<MouvementPlayer2>().enabled = true;    //réactiver le mouvement du player attaquésd
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
}
