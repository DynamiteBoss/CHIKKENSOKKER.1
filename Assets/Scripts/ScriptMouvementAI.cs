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

    Vector3 positionTactique;
    Vector3 posBalle { get; set; }

    Vector3 Positionement { get; set; }

    Rigidbody Ballon { get; set; }

    GameObject But { get; set; }
    Vector3 PositionDéfenseDéfaut { get; set; }
    Vector3 PositionDéfenseFill { get; set; }
    List<GameObject> ListeProximitéA { get; set; }
    List<GameObject> ListeProximitéB { get; set; }

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
    int [] LimitesZ { get; set; }
    const int LIMITE_HAUT = 17;
    const int LIMITE_BAS = -17;
    const int LIMITE_GAUCHE = -39;
    const int LIMITE_DROITE = 39;

    const float VitDeplacement = 7.5f;
    // Start is called before the first frame update
    void Start()
    {
        estDansZoneFill = false;
        PointÀAller = new Vector3();
        LimitesX = new int[2] { LIMITE_GAUCHE, LIMITE_DROITE };
        LimitesZ = new int[2] { LIMITE_BAS, LIMITE_HAUT };
        ListeProximitéA = new List<GameObject>();
        ListeProximitéB = new List<GameObject>();
        posBalle = GameObject.FindGameObjectWithTag("Balle").GetComponent<Transform>().position;
        Ballon = GameObject.FindGameObjectWithTag("Balle").GetComponentInChildren<Rigidbody>();
        But = GameObject.Find("But1");  //changer pour le but à rechercher
        noComportement = int.Parse(this.name[this.name.Length - 2].ToString());
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
        PositionDéfenseFill = new Vector3();
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
                break;
            case "Défendre":
                return GérerPositionsDef();
                break;
            case "Avancer":
                return new Vector3(20 * constÉquipe + UnityEngine.Random.Range(-5f, 5f), this.transform.position.y, this.transform.position.z);
                break;
            case "Revenir":
                return DéterminerPosRevenir();
                break;
            default:
                if (GameObject.FindGameObjectsWithTag("AI").OrderBy(x => (x.transform.position - Ballon.transform.position).magnitude).Where(x => x.GetComponentInChildren<ScriptMouvementAI>().enabled && x.GetComponent<TypeÉquipe>().estÉquipeA == this.transform.GetComponent<TypeÉquipe>().estÉquipeA).First().transform == this.transform)
                    return Ballon.transform.position;
                else
                    return positionTactique * constÉquipe - Vector3.right * 20 * constÉquipe;
                
        }
    }

    private Vector3 GérerPositionsDef()
    {
        Vector3 posCible = Vector3.one;
        //if (EstPasSeulDansZone(GetComponent<TypeÉquipe>().estÉquipeA ? ListeProximitéB : ListeProximitéA, transform.position))
        //{
        //    posCible = transform.position;
        //}
        //else
        //{
        //    posCible = PositionDéfenseDéfaut;//(PositionDéfenseDéfaut + 0.25f * posBalle);
        //}
        return posCible ;
    }

    private Vector3 DéterminerPosRevenir()
    {
        //PositionDéfenseDéfaut = posCible;
        if (tag == "AI")
        {
            GameObject[] tabJoueurs = GameObject.FindGameObjectsWithTag("Player");
            GameObject[] tabAI = GameObject.FindGameObjectsWithTag("AI");
            GameObject[] tabTous = tabJoueurs.Concat(tabAI).ToArray();
            List<GameObject> listeJoueursA = new List<GameObject>();
            List<GameObject> listeJoueursB = new List<GameObject>();
            foreach (GameObject x in tabTous)
            {
                if (x.GetComponent<TypeÉquipe>().estÉquipeA)
                {
                    ListeProximitéA.Add(x);
                }
                else ListeProximitéB.Add(x);
            }
            foreach(GameObject x in tabJoueurs)
            {
                if (x.GetComponent<TypeÉquipe>().estÉquipeA)
                {
                    listeJoueursA.Add(x);
                }
                else listeJoueursB.Add(x);
            }
            if (GetComponent<TypeÉquipe>().estÉquipeA)
            {
                if (EstPasSeulDansZone(listeJoueursA, PositionDéfenseDéfaut))
                {
                    PositionDéfenseFill = RelocaliserJoueurDef();
                    estDansZoneFill = true;
                }
                else estDansZoneFill = false;
            }
            else
            {
                if (EstPasSeulDansZone(listeJoueursB, PositionDéfenseDéfaut))
                {
                    PositionDéfenseFill = RelocaliserJoueurDef();
                    estDansZoneFill = true;
                }
                else estDansZoneFill = false;
            }
            if (estDansZoneFill)
            {
                return PositionDéfenseFill;
            }
            else return PositionDéfenseDéfaut;
        }
        //PositionDéfenseDéfaut = posCible;
        return PositionDéfenseDéfaut;
    }
    private Vector3 RelocaliserJoueurDef()
    {
        return new Vector3(10 - DÉCALLAGE_DEMI_TERRAIN * constÉquipe, transform.position.y, -10);
    }

    private bool EstPasSeulDansZone(List<GameObject> listeJoueur,Vector3 milieuZone)
    {
        bool estPasSeul = false;
        for (int i = 0; i != listeJoueur.Count; i++)
        {
            if (EstDansPérimètre(listeJoueur[i].transform,milieuZone))
            {
                estPasSeul = true;
                
            }
        }
        return estPasSeul;
    }
    private bool EstDansPérimètre(Transform joueur, Vector3 milieu)
    {
        return (joueur.position.x < milieu.x + rayonZoneJoueur && joueur.position.x > milieu.x - rayonZoneJoueur &&
                joueur.position.z < milieu.z + rayonZoneJoueur && joueur.position.z > milieu.z - rayonZoneJoueur);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other == Ballon)
        {
            this.transform.GetComponentInChildren<ActionsPlayer>().enabled = true;
            this.transform.GetComponentInChildren<MouvementPlayer>().enabled = true;
            this.transform.GetComponentInChildren<MouvementManette>().enabled = true;
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

    //private int TrouverComportementDéfense()
    //{
    //    if ()
    //}
    private Vector3 GérerPositionsAtt()              //       À MODIFIER
    {
        //return new Vector3(20 * constÉquipe + UnityEngine.Random.Range(-5f, 5f), this.transform.position.y, this.transform.position.z);
        PointÀAller = (GameObject.FindGameObjectsWithTag("Player").Where(x => x.GetComponent<TypeÉquipe>().estÉquipeA == this.transform.GetComponent<TypeÉquipe>().estÉquipeA)
            .OrderByDescending(x => x.transform.position.x * constÉquipe).First().transform.position);
        PointÀAller += (positionTactique + new Vector3(constÉquipe /** (10/PointÀAller.x)*/, 0, 0));
        VérifierCible();
        return PointÀAller;
        
    }

    private void VérifierCible()
    {
        float nouveauX = PointÀAller.x;
        float nouveauZ = PointÀAller.z ;
        if(PointÀAller.x > LIMITE_DROITE)
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

    private string TrouverCorportementDéplacement()
    {

        if (Ballon.transform.parent != null)
        {
            if (Ballon.transform.parent.position.x * constÉquipe <= 0)
            {
                if (Ballon.transform.parent.GetComponent<TypeÉquipe>().estÉquipeA == this.transform.GetComponent<TypeÉquipe>().estÉquipeA)
                {
                    return "Avancer";
                }
                else
                {
                    return "Défendre";
                }
            }
            else
            {
                if (Ballon.transform.parent.GetComponent<TypeÉquipe>().estÉquipeA == this.transform.GetComponent<TypeÉquipe>().estÉquipeA)
                {
                    return "Attaquer";
                }
                else
                {
                    return "Revenir";
                }
            }
        }
        return "Default";
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
