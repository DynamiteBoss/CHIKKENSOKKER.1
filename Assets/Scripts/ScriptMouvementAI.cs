using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ScriptMouvementAI : NetworkBehaviour
{
    Vector3 PointÀAller { get; set; }

    Vector3 positionTactique;

    Vector3 Positionement { get; set; }

    Rigidbody Ballon { get; set; }

    GameObject But { get; set; }
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

    const float VitDeplacement = 7.5f;
    // Start is called before the first frame update
    void Start()
    {
        ListeProximitéA = new List<GameObject>();
        ListeProximitéB = new List<GameObject>();
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
        Vector3 posCible = new Vector3();
        if (EstPasSeulDansZone(ListeProximitéA, transform.position))
        {
            posCible = transform.position;
        }
        return Vector3.one;
    }

    private Vector3 DéterminerPosRevenir()
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
        Vector3 posCible = new Vector3((10 * abscisse - DÉCALLAGE_DEMI_TERRAIN * constÉquipe), this.transform.position.y, 10 * ordonnée);
        if (tag == "AI")
        {
            GameObject[] listeJoueurs = GameObject.FindGameObjectsWithTag("Player");
            GameObject[] listeAI = GameObject.FindGameObjectsWithTag("AI");
            GameObject[] listeTous = listeJoueurs.Concat(listeAI).ToArray();
            foreach (GameObject x in listeJoueurs)
            {
                if (x.GetComponent<TypeÉquipe>().estÉquipeA)
                {
                    ListeProximitéA.Add(x);
                }
                else ListeProximitéB.Add(x);
            }
            if (GetComponent<TypeÉquipe>().estÉquipeA)
            {
                if (EstPasSeulDansZone(ListeProximitéA, posCible))
                {
                    posCible = RelocaliserJoueurDef();
                }
            }
            else
            {
                if (EstPasSeulDansZone(ListeProximitéB, posCible))
                {
                    posCible = RelocaliserJoueurDef();
                }
            }
        }
        return posCible;
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
            else
            {
                estPasSeul = false;
               
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
        return PointÀAller;
        
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
