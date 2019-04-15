﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ScriptMouvementAI : NetworkBehaviour
{
    Vector3 PointÀAller { get; set; }

    Rigidbody Ballon { get; set; }

    GameObject But { get; set; }

    [SerializeField]
    int NbFramesAvantUpdate = 10;

    [SerializeField]
    float déplacementParSeconde = 0.1f;

    int compteurFrames = 0;
    float deltaPosition = 0.5f;
    bool aLeBallon;
    string ModePositionnement;

    const float VitDeplacement = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        Ballon = GameObject.FindGameObjectWithTag("Balle").GetComponentInChildren<Rigidbody>();
        But = GameObject.Find("But1");  //changer pour le but à rechercher
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
                return new Vector3(20 * (this.transform.GetComponent<TypeÉquipe>().estÉquipeA ? 1 : -1) + UnityEngine.Random.Range(-5f, 5f), this.transform.position.y, this.transform.position.z);
                break;
            case "Revenir":
                return new Vector3(-20 * (this.transform.GetComponent<TypeÉquipe>().estÉquipeA ? 1 : -1) + UnityEngine.Random.Range(-5f,5f), this.transform.position.y, this.transform.position.z);
                break;
            default:
                return Ballon.transform.position;
                
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other == Ballon)
        {
            this.transform.parent.GetComponentInChildren<ActionsPlayer>().enabled = true;
            this.transform.parent.GetComponentInChildren<MouvementPlayer>().enabled = true;
            this.transform.parent.GetComponentInChildren<MouvementManette>().enabled = true;
            this.enabled = false;
        }

    }

    private Vector3 GérerPositionsDef()              //       À MODIFIER
    {
        return new Vector3(-20 * (this.transform.GetComponent<TypeÉquipe>().estÉquipeA ? 1 : -1) + UnityEngine.Random.Range(-5f, 5f), this.transform.position.y, this.transform.position.z);
    }
    private Vector3 GérerPositionsAtt()              //       À MODIFIER
    {
        return new Vector3(20 * (this.transform.GetComponent<TypeÉquipe>().estÉquipeA ? 1 : -1) + UnityEngine.Random.Range(-5f, 5f), this.transform.position.y, this.transform.position.z);
    }

    private void DéplacerJoueurVersPoint(Vector3 pointDéplacement)
    {
        if ((this.transform.position - Ballon.transform.position).magnitude > 1)
        {
            this.transform.position += (pointDéplacement - this.transform.position).normalized * VitDeplacement * Time.deltaTime;
        }
    }

    private string TrouverCorportementDéplacement()
    {
        if (Ballon.transform.parent != null)
        {
            if (Ballon.transform.position.x * (this.transform.GetComponent<CombinerMeshPlayer>().estÉquipeA ? 1 : -1) <= 0)
            {
                if (Ballon.transform.parent.GetComponent<CombinerMeshPlayer>().estÉquipeA == this.transform.parent.GetComponent<CombinerMeshPlayer>().estÉquipeA)
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
                if (Ballon.transform.parent.GetComponent<CombinerMeshPlayer>().estÉquipeA == this.transform.parent.GetComponent<CombinerMeshPlayer>().estÉquipeA)
                {
                    return "Attaquer";
                }
                else
                {
                    return "Revenir";
                }
            }
        }
        else
        {
            return "Default";
        }

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
