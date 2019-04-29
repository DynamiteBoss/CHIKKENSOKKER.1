using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ScriptMouvementAI : NetworkBehaviour
{
    bool estDansZoneFill { get; set; }
    Vector3 PointÀAller { get; set; }
    GameObject Balle { get; set; }
    Vector3 positionTactique;
    Vector3 PosBalleTerrain { get; set; }

    Vector3 Positionement { get; set; }
    GameObject GardienAllié { get; set; }

    Rigidbody Ballon { get; set; }
    GameObject[] TabJoueurs { get; set; }
    GameObject[] TabAI { get; set; }
    GameObject[] TabTous { get; set; }
    List<GameObject> ListeJoueursA { get; set; }
    List<GameObject> ListeJoueursB { get; set; }
    List<GameObject> ListeAttaquantsProximité { get; set; }
    GameObject But { get; set; }
    Vector3 PositionDéfenseDéfaut { get; set; }
    Vector3 PositionDéfenseFill { get; set; }
    Vector3 PositionDéfenseActuelle { get; set; }
    List<GameObject> ListeTousA { get; set; }
    List<GameObject> ListeTousB { get; set; }
    string Comportement { get; set; }
    [SerializeField]
    int NbFramesAvantUpdate = 10;

    [SerializeField]
    float déplacementParSeconde = 1f;

    int compteurFrames = 0;
    float deltaPosition = 0.5f;
    const int DÉCALLAGE_DEMI_TERRAIN = 20;
    int rayonZoneJoueur = 10;
    bool aLeBallon;
    string ModePositionnement;
    int noComportement;
    short constÉquipe;
    int[] LimitesX { get; set; }
    int[] LimitesZ { get; set; }
    const int LIMITE_HAUT = 17;
    const int LIMITE_BAS = -17;
    const int LIMITE_GAUCHE = -39;
    const int LIMITE_DROITE = 39;

    const float VitDeplacement = 7.5f;
    // Start is called before the first frame update
    void Start()
    {
        InitialiserRéférences();
        if (noComportement == 3)
        {
            positionTactique = new Vector3(7, 0, 12);
        }
        else if (noComportement == 4)
        {
            positionTactique = new Vector3(7, 0, -12);
        }
        else if (noComportement == 2)
        {
            positionTactique = new Vector3(-7, 0, 5);
        }
        else
        {
            positionTactique = new Vector3(-7, 0, -5);
        }
        //Debug.Log(noComportement);


        constÉquipe = (short)(this.transform.GetComponent<TypeÉquipe>().estÉquipeA ? 1 : -1);
        TrouverPositionDefDeBase();
        PositionDéfenseFill = new Vector3(10 - DÉCALLAGE_DEMI_TERRAIN * constÉquipe, transform.position.y, -10);
    }
    private void InitialiserRéférences()
    {
        PositionDéfenseActuelle = new Vector3();
        TabJoueurs = GameObject.FindGameObjectsWithTag("Player");
        TabAI = GameObject.FindGameObjectsWithTag("AI");
        GardienAllié = GameObject.FindGameObjectsWithTag("Gardien").Where(x => x.GetComponent<TypeÉquipe>().estÉquipeA == this.gameObject.GetComponent<TypeÉquipe>()).First();
        TabTous = TabJoueurs.Concat(TabAI).ToArray();
        ListeJoueursA = new List<GameObject>();
        ListeJoueursB = new List<GameObject>();
        Balle = GameObject.FindGameObjectWithTag("Balle");
        estDansZoneFill = false;
        PointÀAller = new Vector3();
        LimitesX = new int[2] { LIMITE_GAUCHE, LIMITE_DROITE };
        LimitesZ = new int[2] { LIMITE_BAS, LIMITE_HAUT };
        ListeTousA = new List<GameObject>();
        ListeTousB = new List<GameObject>();
        ListeAttaquantsProximité = new List<GameObject>();
        Ballon = GameObject.FindGameObjectWithTag("Balle").GetComponentInChildren<Rigidbody>();
        But = GameObject.Find("But1");  //changer pour le but à rechercher
        noComportement = int.Parse(this.name[this.name.Length - 2].ToString());
    }
    private void TrouverPositionDefDeBase()
    {
        int abscisse = 1;
        int ordonnée = 1;
        if (noComportement == 2 || noComportement == 3)
        {
            abscisse = -1;
        }
        if (noComportement == 3 || noComportement == 4)
        {
            ordonnée = -1;
        }
        PositionDéfenseDéfaut = new Vector3((10 * abscisse - DÉCALLAGE_DEMI_TERRAIN * constÉquipe), this.transform.position.y, 10 * ordonnée);
    }
    // Update is called once per frame
    void Update()
    {
        ++compteurFrames;
        DéplacerJoueur();
        RotaterJoueur();
        DéplacerJoueurVersPoint(PointÀAller);
        if (compteurFrames++ == 17)
        {
            compteurFrames = 0;
            PointÀAller = TrouverPointDéplacement(TrouverCorportementDéplacement());
            Debug.DrawLine(this.transform.position + Vector3.up, PointÀAller, Color.gray, 17f / 60f);
            /*
            Debug.Log(TrouverCorportementDéplacement());
            */
        }
    }
    private Vector3 TrouverPointDéplacement(string comportement)
    {
        switch (comportement)
        {
            case "Attaquer":
                return GérerPositionsAtt();
            case "Défendre":
                DéterminerPosRevenir();
                return GérerPositionsDef();
            case "Avancer":
                return new Vector3(20 * constÉquipe + UnityEngine.Random.Range(-5f, 5f), this.transform.position.y, this.transform.position.z);
            case "Revenir":
                DéterminerPosRevenir();
                return PositionDéfenseActuelle;
            default:
                if (GameObject.FindGameObjectsWithTag("AI").OrderBy(x => (x.transform.position - Ballon.transform.position).magnitude).Where(x => x.GetComponentInChildren<ScriptMouvementAI>().enabled && x.GetComponent<TypeÉquipe>().estÉquipeA == this.transform.GetComponent<TypeÉquipe>().estÉquipeA).First().transform == this.transform)
                    return Ballon.transform.position;
                else
                    return positionTactique * constÉquipe - Vector3.right * 20 * constÉquipe;
        }
    }
    private GameObject TrouverAttaquantProche()
    {

        List<GameObject> listeAttaquants = (transform.GetComponent<TypeÉquipe>().estÉquipeA ? ListeTousB : ListeTousA);
        foreach(GameObject x in listeAttaquants)
        {
            if (EstDansPérimètre(x.transform, PositionDéfenseActuelle))
            {
                ListeAttaquantsProximité.Add(x);
            }
        }
        return ListeAttaquantsProximité[0];

    }
    private Vector3 GérerPositionsDef()
    {
        Vector3 posCible = Vector3.one;
        if (Balle.transform.parent != null)
        {
            PosBalleTerrain = Balle.transform.parent.GetComponent<Transform>().position;
            if (EstPasSeulDansZone(GetComponent<TypeÉquipe>().estÉquipeA ? ListeTousB : ListeTousA, transform.position))
            {
                posCible = PositionDéfenseActuelle;
                if (!EstDansPérimètre(Balle.transform.parent.transform, PositionDéfenseActuelle))
                {
                    posCible = (TrouverAttaquantProche().transform.position - GardienAllié.transform.position).normalized * 2*(this.transform.position - GardienAllié.transform.position).magnitude/3 + GardienAllié.transform.position;
                }
                posCible = PositionDéfenseActuelle;
            }
            else
            {
                posCible.x = PositionDéfenseActuelle.x + ((PosBalleTerrain.x + DÉCALLAGE_DEMI_TERRAIN * constÉquipe) * 0.5f);
                posCible.y = PositionDéfenseActuelle.y + (PosBalleTerrain.y * 0.5f);
                //posCible = PositionDéfenseActuelle;
            }
        }
        return posCible;
    }


    //private Vector3 DéterminerPositionBaseActuelleDéfense()
    //{

    //}

    private void DéterminerPosRevenir()
    {
        //PositionDéfenseDéfaut = posCible;
        Vector3 posCible = new Vector3();
        foreach (GameObject x in TabTous)
        {
            if (x.GetComponent<TypeÉquipe>().estÉquipeA)
            {
                ListeTousA.Add(x);
            }
            else ListeTousB.Add(x);
        }
        foreach (GameObject x in TabJoueurs)
        {
            if (x.GetComponent<TypeÉquipe>().estÉquipeA)
            {
                ListeJoueursA.Add(x);
            }
            else ListeJoueursB.Add(x);
        }
        if (GetComponent<TypeÉquipe>().estÉquipeA)
        {
            if (EstPasSeulDansZone(ListeJoueursA, PositionDéfenseDéfaut))
            {
                estDansZoneFill = true;
            }
            else estDansZoneFill = false;
        }
        else
        {
            if (EstPasSeulDansZone(ListeJoueursB, PositionDéfenseDéfaut))
            {
                estDansZoneFill = true;
            }
            else estDansZoneFill = false;
        }
        if (estDansZoneFill)
        {
            PositionDéfenseActuelle = PositionDéfenseFill;
            posCible = PositionDéfenseFill;
        }
        else
        {
            PositionDéfenseActuelle = PositionDéfenseDéfaut;
            posCible = PositionDéfenseDéfaut;
        }
        
    }




    private bool EstPasSeulDansZone(List<GameObject> listeJoueur, Vector3 milieuZone)
    {
        bool estPasSeul = false;
        for (int i = 0; i != listeJoueur.Count; i++)
        {
            if (EstDansPérimètre(listeJoueur[i].transform, milieuZone))
            {
                estPasSeul = true;

            }
        }
        return estPasSeul;
    }
    private bool EstDansPérimètre(Transform objectÀVérifier, Vector3 milieu)
    {
        return (objectÀVérifier.position.x < milieu.x + rayonZoneJoueur && objectÀVérifier.position.x > milieu.x - rayonZoneJoueur &&
                objectÀVérifier.position.z < milieu.z + rayonZoneJoueur && objectÀVérifier.position.z > milieu.z - rayonZoneJoueur);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other == Ballon)
        {
            this.transform.GetComponentInChildren<ActionsPlayerV2>().enabled = true;
            this.transform.GetComponentInChildren<MouvementPlayer>().enabled = true;
            //this.transform.GetComponentInChildren<MouvementManette>().enabled = true;
            this.enabled = false;
            Ballon.isKinematic = true;
        }
    }

    //private Vector3 GérerComportementDef()              //       À MODIFIER
    //{
    //    //return new Vector3(-20 * constÉquipe + UnityEngine.Random.Range(-5f, 5f), this.transform.position.y, this.transform.position.z);
    //    switch (TrouverComportementDéfense())
    //    {
    //        case 1:

    //        case 2:

    //            break;
    //        case 3:
    //            return GérerComportementDef();
    //            break;
    //        case 4:
    //            return new Vector3(20 * (-constÉquipe) + UnityEngine.Random.Range(-5f, 5f), this.transform.position.y, this.transform.position.z);

    //            break;
    //    }
    //}
    private Vector3 GérerPositionsAtt()              //       À MODIFIER
    {
        GameObject balle = GameObject.FindGameObjectWithTag("Balle");
        if (balle.GetComponent<PlacerBalle>().estPlacer)
        {
            PointÀAller = balle.transform.parent.transform.position;
            //return new Vector3(20 * constÉquipe + UnityEngine.Random.Range(-5f, 5f), this.transform.position.y, this.transform.position.z);


            /*PointÀAller = (GameObject.FindGameObjectsWithTag("Player").Where(x => x.GetComponent<TypeÉquipe>().estÉquipeA == this.transform.GetComponent<TypeÉquipe>().estÉquipeA)
                .OrderByDescending(x => x.transform.position.x * constÉquipe).First().transform.position);*/
            PointÀAller += (positionTactique + new Vector3(constÉquipe /** (10/PointÀAller.x)*/, 0, 0));


            VérifierCible();
        }

        return PointÀAller;

    }

    private void VérifierCible()
    {
        float nouveauX = PointÀAller.x;
        float nouveauZ = PointÀAller.z;
        if (PointÀAller.x > LIMITE_DROITE)
        {
            nouveauX = LIMITE_DROITE;
        }
        if (PointÀAller.x < LIMITE_GAUCHE)
        {
            nouveauX = LIMITE_GAUCHE;
        }
        if (PointÀAller.z > LIMITE_HAUT)
        {
            nouveauZ = LIMITE_HAUT;
        }
        if (PointÀAller.z < LIMITE_BAS)
        {
            nouveauZ = LIMITE_BAS;
        }
        PointÀAller = new Vector3(nouveauX, transform.position.y, nouveauZ);
    }
    private void DéplacerJoueurVersPoint(Vector3 pointDéplacement)
    {
        if ((this.transform.position - (Ballon.transform.position + Vector3.up)).magnitude > 1)
        {
            this.transform.position += new Vector3(pointDéplacement.x - this.transform.position.x, 0, pointDéplacement.z - this.transform.position.z).normalized * VitDeplacement * Time.deltaTime;
        }
    }

    private String TrouverCorportementDéplacement()
    {
        string comportement;
        if (Ballon.transform.parent != null)
        {
            if (Ballon.transform.parent.position.x * constÉquipe <= 0)
            {
                if (Ballon.transform.parent.GetComponent<TypeÉquipe>().estÉquipeA == this.transform.GetComponent<TypeÉquipe>().estÉquipeA)
                {
                    comportement = "Avancer";
                }
                else
                {
                    comportement = "Défendre";
                }
            }
            else
            {
                if (Ballon.transform.parent.GetComponent<TypeÉquipe>().estÉquipeA == this.transform.GetComponent<TypeÉquipe>().estÉquipeA)
                {
                    comportement = "Attaquer";
                }
                else
                {
                    comportement = "Revenir";
                }
            }
        }
        else
        {
            comportement = "Default";
        }
        Comportement = comportement;
        return comportement;
    }

    private void EffectuerMiseÀJour()
    {
        CalculerNouvellePosition();
        if (this.transform.Find("Balle"))
        {
            aLeBallon = true;
            PointÀAller = new Vector3(But.transform.position.x, this.transform.position.y, But.transform.position.z);
        }
        else
        {
            aLeBallon = false;
        }
    }

    private void RotaterJoueur()
    {
        if (!aLeBallon)
        {
            Vector3 orientation = Ballon.transform.position;
            orientation.y = -90;
            this.transform.LookAt(new Vector3(orientation.x, this.transform.position.y, orientation.z));
        }
        else
        {
            Vector3 orientation = But.transform.position;
            orientation.y = -90;
            this.transform.LookAt(new Vector3(orientation.x, this.transform.position.y, orientation.z));
        }

        //if (!aLeBallon)
        //{
        //    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Ballon.transform.position - this.transform.position), 0.1f);
        //}
        //else
        //{
        //    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(But.transform.position - this.transform.position), 0.1f);
        //}
    }

    private void TirerBallon()
    {

        Ballon.isKinematic = false;
        Ballon.GetComponentInChildren<SphereCollider>().enabled = true;
        Ballon.transform.parent = null;
        Ballon.AddForce((But.transform.position - this.transform.position).normalized * 5, ForceMode.Impulse);
        aLeBallon = false;
    }

    private void CalculerNouvellePosition()
    {
        //  Soit:
        // - "PosAllié_BalleAllié"
        // - "PosAllié_BalleEnnemie"
        // - "PosEnnemie_BalleAlliée"
        // - "PosEnnemie_BalleEnnemie"
        // En fonction de la position du ballon
        PointÀAller = new Vector3(Ballon.position.x, this.transform.position.y, Ballon.position.z);
    }

    private void DéplacerJoueur()
    {
        switch (ModePositionnement)
        {
            case "PosAllié_BalleAllié":

                break;
            case "PosAllié_BalleEnnemie":

                break;
            case "PosEnnemie_BalleAlliée":

                break;
            case "PosEnnemie_BalleEnnemie":

                break;
            default:
                /*if (Ballon.velocity.magnitude <= 2)
                    PointÀAller = Ballon.position;
                else*/
                //PointÀAller = new Vector3(PointÀAller.x + UnityEngine.Random.Range(-deltaPosition, deltaPosition), PointÀAller.y, PointÀAller.z + UnityEngine.Random.Range(-deltaPosition, deltaPosition));

                this.transform.position += (PointÀAller - this.transform.position).normalized * Time.deltaTime * déplacementParSeconde;

                break;
        }
        //BougerJoueur();

    }

    private void BougerJoueur()
    {

    }
}
