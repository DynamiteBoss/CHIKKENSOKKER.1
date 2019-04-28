using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Networking;

public class MouvementPlayer : NetworkBehaviour
{
    const float VitesseModeFurax = 1.3f;
    const int LIMITE_HAUT = 17;
    const int LIMITE_BAS = -17;
    const int LIMITE_GAUCHE = -39;
    const int LIMITE_DROITE= 39;


    string[] Controles = new string[] { "w", "a", "s", "d" };
    string[] ControlesInversés = new string[] { "w", "a", "s", "d" };
    string[] ControlesOriginaux = new string[] { "d", "s", "w", "a" };
    bool joueur1EstPris = false;
    [SerializeField]
    public bool modeGlace;

    [SerializeField]
    public bool modeSaoul;

    [SerializeField]
    public bool modeFurax;

    [SerializeField]
    public bool modePluie;

    [SerializeField]
    public bool modeInvincible;

    int compteur;

    bool autreValeur;
    string Nom { get; set; }
    float valVitDiago = 5.5f / Mathf.Sqrt(2);
    void Start()
    {
        Nom = name;
    }
    void Update()
    {

        //transform.position = new Vector3(transform.position.x, -1, transform.position.z);
        if (!isLocalPlayer)
        { return; }
        //if (modeInvincible) { GetComponentsInChildren<Rigidbody>()[1].isKinematic = true; }           
        //else { GetComponentsInChildren<Rigidbody>()[1].isKinematic = false; }                 //<-- Its breaky the fokine game, mon boy - Change moé ça OPC

        if (tag == "Player" && name.StartsWith("Joueur1"))
        {
            if (!modeGlace && !modePluie)
            {
                DéplacerManette(1);
                DépacerAWSD();
            }
            else
            {
                if (modePluie) { this.GetComponentInChildren<Rigidbody>().drag = 3; }

                else { this.GetComponentInChildren<Rigidbody>().drag = 0.75f; }

                DéplacerModeGlace();
            }
            joueur1EstPris = true;
        }
        if (tag == "Player" && name.StartsWith("Joueur2"))
        {
            DéplacerFlèche();
            DéplacerManette(2);
        }

        else { }


        if (compteur++ == 10)
        {
            GetComponentInChildren<Rigidbody>().isKinematic = !(modeGlace || modePluie);
            Controles = modeSaoul ? ControlesOriginaux : ControlesInversés;
        }
    }
    private bool EstHorsLimite(int limite, float coordonnéeÀVéfifier)
    {
        if(limite>0)
        {
            return (coordonnéeÀVéfifier + (2) * valVitDiago * Time.deltaTime * (modeFurax ? VitesseModeFurax : 1f) > limite);
        }
        else
        {
            return (coordonnéeÀVéfifier + (2) * valVitDiago * Time.deltaTime * (modeFurax ? VitesseModeFurax : 1f) < limite);
        }
        
    }
    void DéplacerManette(int number)
    {
        float k = Input.GetAxis("LeftJoystickHorizontal" + number.ToString());
        {
            if (k > 0)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(new Vector3(1, 0)), 0.1f);
            }
            else
            {
                if (k < 0)
                {
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(new Vector3(-1, 0)), 0.1f);
                }
            }
            transform.Translate(new Vector3(2 * k, 0, 0) * 5.5f * Time.deltaTime, Space.World);
        }

        float j = Input.GetAxis("LeftJoystickVertical" + number.ToString());
        {
            if (j > 0)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(new Vector3(0, 0, 1)), 0.1f);
            }
            else
            {
                if (j < 0)
                {
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(new Vector3(0, 0, -1)), 0.1f);
                }
            }
            transform.Translate(new Vector3(0, 0, 2 * j) * 5.5f * Time.deltaTime, Space.World);
        }
    }
    void DépacerAWSD()
    {
        autreValeur = false;
        if (Input.GetKey(Controles[0]))
        {
            if (Input.GetKey(Controles[1]) || Input.GetKey(Controles[2]) || Input.GetKey(Controles[3]))
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.forward), 0.1f);
                if (EstHorsLimite(LIMITE_HAUT,transform.position.z)) return;
                transform.Translate(new Vector3(0, 0, 2) * valVitDiago * Time.deltaTime * (modeFurax ? VitesseModeFurax : 1f), Space.World);
            }
            else
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.forward), 0.1f);
                if (EstHorsLimite(LIMITE_HAUT, transform.position.z)) return;
                transform.Translate(new Vector3(0, 0, 2) * 5.5f * Time.deltaTime * (modeFurax ? VitesseModeFurax : 1f), Space.World);
                autreValeur = true;
            }
        }
        if (Input.GetKey(Controles[1]))
        {
            if (autreValeur)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(-Vector3.right), 0.1f);
                if (EstHorsLimite(LIMITE_GAUCHE, transform.position.x)) return;
                transform.Translate(new Vector3(-2, 0, 0) * valVitDiago * Time.deltaTime * (modeFurax ? VitesseModeFurax : 1f), Space.World);
            }
            else
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(-Vector3.right), 0.1f);
                if (EstHorsLimite(LIMITE_GAUCHE, transform.position.x)) return;
                transform.Translate(new Vector3(-2, 0, 0) * 5.5f * Time.deltaTime * (modeFurax ? VitesseModeFurax : 1f), Space.World);
                autreValeur = true;
            }
        }
        if (Input.GetKey(Controles[2]))
        {
            if (autreValeur)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(-Vector3.forward), 0.1f);
                if (EstHorsLimite(LIMITE_BAS, transform.position.z)) return;
                transform.Translate(new Vector3(0, 0, -2) * valVitDiago * Time.deltaTime * (modeFurax ? VitesseModeFurax : 1f), Space.World);
            }
            else
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(-Vector3.forward), 0.1f);
                if (EstHorsLimite(LIMITE_BAS, transform.position.z)) return;
                transform.Translate(new Vector3(0, 0, -2) * 5.5f * Time.deltaTime * (modeFurax ? VitesseModeFurax : 1f), Space.World);
                autreValeur = true;
            }

        }
        if (Input.GetKey(Controles[3]))
        {
            if (autreValeur)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.right), 0.1f);
                if (EstHorsLimite(LIMITE_DROITE, transform.position.x)) return;
                transform.Translate(new Vector3(2, 0, 0) * valVitDiago * Time.deltaTime * (modeFurax ? VitesseModeFurax : 1f), Space.World);
            }
            else
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.right), 0.1f);
                if (EstHorsLimite(LIMITE_DROITE, transform.position.x)) return;
                transform.Translate(new Vector3(2, 0, 0) * 5.5f * Time.deltaTime * (modeFurax ? VitesseModeFurax : 1f), Space.World);
                autreValeur = true;
            }

        }
    }
    void DéplacerFlèche()
    {
        autreValeur = false;
        if (Input.GetKey("up"))
        {
            if (Input.GetKey("left") || Input.GetKey("down") || Input.GetKey("right"))
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.forward), 0.1f);
                transform.Translate(new Vector3(0, 0, 2) * valVitDiago * Time.deltaTime * (modeFurax ? VitesseModeFurax : 1f), Space.World);
            }
            else
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.forward), 0.1f);
                transform.Translate(new Vector3(0, 0, 2) * 5.5f * Time.deltaTime * (modeFurax ? VitesseModeFurax : 1f), Space.World);
                autreValeur = true;
            }
        }
        if (Input.GetKey("left"))
        {
            if (autreValeur == true)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(-Vector3.right), 0.1f);
                transform.Translate(new Vector3(-2, 0, 0) * valVitDiago * Time.deltaTime * (modeFurax ? VitesseModeFurax : 1f), Space.World);
            }
            else
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(-Vector3.right), 0.1f);
                transform.Translate(new Vector3(-2, 0, 0) * 5.5f * Time.deltaTime * (modeFurax ? VitesseModeFurax : 1f), Space.World);
                autreValeur = true;
            }
        }
        if (Input.GetKey("down"))
        {
            if (autreValeur == true)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(-Vector3.forward), 0.1f);
                transform.Translate(new Vector3(0, 0, -2) * valVitDiago * Time.deltaTime * (modeFurax ? VitesseModeFurax : 1f), Space.World);
            }
            else
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(-Vector3.forward), 0.1f);
                transform.Translate(new Vector3(0, 0, -2) * 5.5f * Time.deltaTime * (modeFurax ? VitesseModeFurax : 1f), Space.World);
                autreValeur = true;
            }

        }
        if (Input.GetKey("right"))
        {
            if (autreValeur == true)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.right), 0.1f);
                transform.Translate(new Vector3(2, 0, 0) * valVitDiago * Time.deltaTime * (modeFurax ? VitesseModeFurax : 1f), Space.World);
            }
            else
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.right), 0.1f);
                transform.Translate(new Vector3(2, 0, 0) * 5.5f * Time.deltaTime * (modeFurax ? VitesseModeFurax : 1f), Space.World);
                autreValeur = true;
            }

        }
    }
    void DéplacerModeGlace()
    {
        if (Input.GetKey(Controles[0]))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.forward), 0.1f);
            this.GetComponentInChildren<Rigidbody>().AddForce((modeFurax ? 17.5f * VitesseModeFurax : 17.5f) * Vector3.forward, ForceMode.Acceleration);
        }
        if (Input.GetKey(Controles[1]))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.left), 0.1f);
            this.GetComponentInChildren<Rigidbody>().AddForce((modeFurax ? 17.5f * VitesseModeFurax : 17.5f) * Vector3.left, ForceMode.Acceleration);
        }
        if (Input.GetKey(Controles[2]))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.back), 0.1f);
            this.GetComponentInChildren<Rigidbody>().AddForce((modeFurax ? 17.5f * VitesseModeFurax : 17.5f) * Vector3.back, ForceMode.Acceleration);
        }
        if (Input.GetKey(Controles[3]))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.right), 0.1f);
            this.GetComponentInChildren<Rigidbody>().AddForce((modeFurax ? 17.5f * VitesseModeFurax : 17.5f) * Vector3.right, ForceMode.Acceleration);
        }
    }
}
