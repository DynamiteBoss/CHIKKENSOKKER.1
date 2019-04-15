using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Networking;

//https://docs.unity3d.com/ScriptReference/WaitForSeconds.html

public class ActionsPlayer : NetworkBehaviour
{
    Transform ZonePlacage { get; set; }
    GameObject JoueurÀPlaquer { get; set; }
    GameObject Balle { get; set; }

    [SerializeField]
    public static Vector3[] PositionJoueursAlliés { get; set; }
    public static GameObject[] JoueursAlliés { get; set; }

    const float ForceMaxPasse = 5f;
    const float DistPasseForceMax = 10f;
    const float DistMaxPasse = 40f;
    const float ForceLobe = 2.5f;

    float compteur = 0;

    bool estEnMouvementPlacage = false;
    bool possessionBallon = false;


    int cpt = 0;
    int cpt2 = 150;

    void Start()
    {
        ZonePlacage = this.transform;
        GameObject balle = GameObject.FindGameObjectWithTag("Balle");
    }
    private void Awake()
    {
        FaireRencensementJoueurs();
        TrouverPosJoueursÉquipe();
    }
    void Update()
    {
        float direction = this.transform.parent.eulerAngles.y / 180f * Mathf.PI;
        Debug.DrawRay(new Vector3(this.transform.parent.position.x, 2.425f, this.transform.parent.position.z), new Vector3(Mathf.Sin(direction), 0, Mathf.Cos(direction)) * 100, Color.green);

        if (!transform.parent.GetComponent<NetworkIdentity>().isLocalPlayer)
        {
            return;
        }


        if (cpt2 % 10 == 0)
        {
            if (cpt2 == 150)
            {
                FaireRencensementJoueurs();
                cpt2 = 0;
            }
            TrouverPosJoueursÉquipe();
        }
        else if (cpt2 % 25 == 0)
        {
            RetirerJoueurPossessionBallon();    //<-------------    REVOIR CECI (2019/03/20 13:45)
        }
        ++cpt2;

        possessionBallon = this.transform.parent.Find("Balle");
        compteur += Time.deltaTime;

        if (transform.parent.tag == "Player")
        {
            //possessionBallon = balle.transform.parent;
            if (compteur >= 0.95f) 
            {
                if (transform.parent.name.StartsWith("Joueur1"))
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


                    if (Input.GetKeyDown("q") && compteur >= 0.95f)
                    {
                        if (possessionBallon)
                        {
                            //bloquer le mouvement du perso pendant un certain temps //VOIR DANSFAIREPLACAGE EN BAS
                            compteur = 0;
                            FairePasse(TrouverPosJoueurPasse(direction));                                                                                                             // TANTOT
                            StartCoroutine(AttendreDéactivationScriptPlaqueur(0.75f, direction));         //attendre un certain temps
                                                                                                          //faire en sorte de pouvoir faire le ontriggerenter ici ou dans le FairePlacage (avant le frapperadversaire)
                        }
                        else
                        {

                        }
                    }
                }
                else
                {
                    if (Input.GetKeyDown(KeyCode.Keypad1) && !possessionBallon)
                    {
                        //bloquer le mouvement du perso pendant un certain temps //VOIR DANSFAIREPLACAGE EN BAS
                        compteur = 0;
                        estEnMouvementPlacage = true;
                        FairePlacage();
                        StartCoroutine(AttendreDéactivationScriptPlaqueur(1f, direction));         //attendre un certain temps
                                                                                                   //faire en sorte de pouvoir faire le ontriggerenter ici ou dans le FairePlacage (avant le frapperadversaire)
                    }

                    if (Input.GetKeyDown(KeyCode.Keypad2) && compteur >= 0.95f && possessionBallon)
                    {
                        //bloquer le mouvement du perso pendant un certain temps //VOIR DANSFAIREPLACAGE EN BAS
                        compteur = 0;
                        FairePasse(TrouverPosJoueurPasse(direction));                                                                                                             // TANTOT
                        StartCoroutine(AttendreDéactivationScriptPlaqueur(0.75f, direction));         //attendre un certain temps
                                                                                                      //faire en sorte de pouvoir faire le ontriggerenter ici ou dans le FairePlacage (avant le frapperadversaire)
                    }
                }
            }
        }
    }
    private Vector3 TrouverPosJoueurPasse(float direction)
    {
        /*Vector du But de l'autre team*/

        Vector3 posJoueurÀPasser = PositionJoueursAlliés[0] == null ? Vector3.up : PositionJoueursAlliés[0];

        for (int i = 1; i < JoueursAlliés.Length; i++)
        {
            posJoueurÀPasser = (CalculerAngle(posJoueurÀPasser, direction) < CalculerAngle(PositionJoueursAlliés[i], direction)) ? posJoueurÀPasser : PositionJoueursAlliés[i];
        }
        if ((posJoueurÀPasser - this.transform.parent.position).magnitude < 2)
        {
            posJoueurÀPasser = ((posJoueurÀPasser - this.transform.parent.position).normalized * ForceLobe) + this.transform.parent.position + Vector3.up * ForceLobe;   //Effectue un lobe
        }
        else if ((posJoueurÀPasser - this.transform.parent.position).magnitude > DistMaxPasse)
        {
            posJoueurÀPasser = ((posJoueurÀPasser - this.transform.parent.position).normalized * ForceLobe) + this.transform.parent.position + Vector3.up * ForceLobe;
        }
        Debug.Log(posJoueurÀPasser.ToString());
        return posJoueurÀPasser;
    }

    private void TrouverPosJoueursÉquipe()
    {
        for (int i = 0; i < JoueursAlliés.Length /*Remplacer par le nb de clients*/; i++)
        {
            PositionJoueursAlliés[i] = JoueursAlliés[i].transform.Find("ZoneContrôle").position;
            Debug.DrawLine(new Vector3(this.transform.parent.position.x, 2.425f, this.transform.parent.position.z), new Vector3(PositionJoueursAlliés[i].x, 2.425f, PositionJoueursAlliés[i].z), Color.cyan, 0.2f);
        }
    }

    private void FaireRencensementJoueurs()
    {
        //for (int i = 0; i < GameObject.FindGameObjectsWithTag("Player").Length; i++)
        //{
        //    JoueursAlliés[i] = GameObject.FindWithTag("Player")/*Une patente qui trouve l'équipe*/;
        //}
        JoueursAlliés = new GameObject[(GameObject.FindGameObjectsWithTag("Player").Where(x => x.GetComponent<TypeÉquipe>().estÉquipeA == this.transform.parent.GetComponent<TypeÉquipe>()).ToArray().Length)];
        PositionJoueursAlliés = new Vector3[JoueursAlliés.Length];

        JoueursAlliés = GameObject.FindGameObjectsWithTag("Player").Where(x => x.GetComponent<TypeÉquipe>().estÉquipeA == this.transform.parent.GetComponent<TypeÉquipe>()).ToArray();

        //JoueursAlliés = GameObject.FindGameObjectsWithTag("Player").ToList().SkipWhile(g => g.transform.Find("Balle").gameObject != null).ToArray();

    }

    float CalculerAngle(Vector3 joueurÀPasser, float angle)
    {
        Vector3 dirPasse = joueurÀPasser - this.transform.parent.position;
        Vector3 rotationJoueur = new Vector3(Mathf.Sin(angle), 0, Mathf.Cos(angle));

        return Vector3.Angle(rotationJoueur, dirPasse) / 180f * Mathf.PI;
    }

    void RetirerJoueurPossessionBallon()
    {
        GameObject[] ancienTabJoueur = (GameObject[])JoueursAlliés.Clone();
        for (int i = 0; i < JoueursAlliés.Length; i++)
        {
            if (JoueursAlliés[i] == this.gameObject)
            {
                JoueursAlliés = new GameObject[(GameObject.FindGameObjectsWithTag("Player").Where(x => x.GetComponent<TypeÉquipe>().estÉquipeA == this.transform.parent.GetComponent<TypeÉquipe>()).ToArray().Length - 1)];
                int indexTableauCopie = 0;

                for (int j = 0; j < JoueursAlliés.Length; j++)
                {
                    if (j != i)
                    {
                        JoueursAlliés[indexTableauCopie] = ancienTabJoueur[j];
                        indexTableauCopie++;
                    }
                }
                break;
            }
        }
    }

    public void FairePasse(Vector3 positionJoueurVisé)
    {
        Transform balle = gameObject.transform.parent.Find("Balle");
        Vector3 vecteurPasse = new Vector3(positionJoueurVisé.x - this.transform.parent.position.x, .5f, positionJoueurVisé.z - this.transform.parent.position.z);
        if (balle != null)
        {
            balle.transform.GetComponentInChildren<Rigidbody>().isKinematic = false;
            StartCoroutine(AttendrePourDistanceBallon(0.6f));
            balle.GetComponent<SphereCollider>().enabled = true;
            balle.transform.parent = null;
            //balle.GetComponentInChildren<Rigidbody>().AddForce(new Vector3(Mathf.Sin(CalculerAngle(positionJoueurVisé)), 0, Mathf.Cos(CalculerAngle(positionJoueurVisé))/*new Vector3(this.transform.position.x - positionJoueurVisé.x, 0.2f, this.transform.position.z - positionJoueurVisé.z*/) * 10f, ForceMode.Impulse);
            balle.GetComponentInChildren<Rigidbody>().AddForce(vecteurPasse.magnitude > DistPasseForceMax ? ((vecteurPasse).normalized * ForceMaxPasse) : vecteurPasse, ForceMode.Impulse);


            Debug.DrawRay(new Vector3(this.transform.parent.position.x, 2.425f, this.transform.parent.position.z), vecteurPasse, Color.red, 2);
            //Debug.DrawLine(new Vector3(this.transform.parent.position.x, 2.425f, this.transform.parent.position.z), new Vector3(this.transform.position.x - positionJoueurVisé.x, 0, this.transform.position.z - positionJoueurVisé.z) * 100, Color.blue, 2);
        }
    }

    IEnumerator AttendrePourDistanceBallon(float durée)
    {
        this.transform.parent.Find("ZoneContrôle").GetComponent<BoxCollider>().isTrigger = false;
        yield return new WaitForSeconds(durée);
        this.transform.parent.Find("ZoneContrôle").GetComponent<BoxCollider>().isTrigger = true;
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
        this.transform.parent.GetComponentInChildren<ContrôleBallonV2>().enabled = false;    //désactiver le controle du ballon
        this.GetComponentInParent<MouvementPlayer>().enabled = false;    //désactiver le mouvement du player
        yield return new WaitForSeconds(durée / 3);
        this.transform.parent.GetComponent<Rigidbody>().drag = 0;
        //this.transform.parent.GetComponent<Rigidbody>().AddForce(-(Mathf.Sin(direction) * 45.5f), 0, -(Mathf.Cos(direction) * 45.5f), ForceMode.Impulse);
        yield return new WaitForSeconds(2 * durée / 3);
        this.transform.parent.GetComponentInChildren<ContrôleBallonV2>().enabled = true;    //réactiver le controle du ballon
        this.GetComponentInParent<MouvementPlayer>().enabled = true;    //réactiver le mouvement du player
        this.transform.parent.GetComponent<Rigidbody>().isKinematic = true;
        estEnMouvementPlacage = false;
    }
    IEnumerator AttendreDéactivationScriptPlaqué(float durée, float direction)
    {
        JoueurÀPlaquer.GetComponentInChildren<ContrôleBallonV2>().enabled = false;    //désactiver le controle du ballon du player attaqué
        JoueurÀPlaquer.GetComponent<MouvementPlayer>().enabled = false;    //désactiver le mouvement du player attaqué
        yield return new WaitForSeconds(durée / 3);
        //CHANGER LE DRAG DU JOUEUR PLAQUÉ AUSSI
        JoueurÀPlaquer.GetComponent<Rigidbody>().AddForce(-(Mathf.Sin(direction) * 35), 0, -(Mathf.Cos(direction) * 35), ForceMode.Impulse);
        yield return new WaitForSeconds(2 * durée / 3);
        JoueurÀPlaquer.GetComponentInChildren<ContrôleBallonV2>().enabled = true;    //réactiver le controle du ballon du player attaqué
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
