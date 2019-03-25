using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Networking;

public class MouvementPlayer : NetworkTransform
{
    bool autreValeur;
    string Nom { get; set; }
    float valVitDiago = 5.5f / Mathf.Sqrt(2);
    void Start()
    {
        Nom = name;
    }
    void Update()
    {
        if (!isLocalPlayer)
        { return; }
        DépacerAWSD();
    }
    void DépacerAWSD()
    {
        autreValeur = false;
        if (Input.GetKey("w"))
        {
            if (Input.GetKey("a") || Input.GetKey("s") || Input.GetKey("d"))
            {
                NetworkTransform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.forward), 0.04f);
                transform.Translate(new Vector3(0, 0, 2) * valVitDiago * Time.deltaTime, Space.World);
            }
            else
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.forward), 0.04f);
                transform.Translate(new Vector3(0, 0, 2) * 5.5f * Time.deltaTime, Space.World);
                autreValeur = true;
            }
        }
        if (Input.GetKey("a"))
        {
            if (autreValeur)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(-Vector3.right), 0.04f);
                transform.Translate(new Vector3(-2, 0, 0) * valVitDiago * Time.deltaTime, Space.World);
            }
            else
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(-Vector3.right), 0.04f);
                transform.Translate(new Vector3(-2, 0, 0) * 5.5f * Time.deltaTime, Space.World);
                autreValeur = true;
            }
        }
        if (Input.GetKey("s"))
        {
            if (autreValeur)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(-Vector3.forward), 0.04f);
                transform.Translate(new Vector3(0, 0, -2) * valVitDiago * Time.deltaTime, Space.World);
            }
            else
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(-Vector3.forward), 0.04f);
                transform.Translate(new Vector3(0, 0, -2) * 5.5f * Time.deltaTime, Space.World);
                autreValeur = true;
            }

        }
        if (Input.GetKey("d"))
        {
            if (autreValeur)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.right), 0.04f);
                transform.Translate(new Vector3(2, 0, 0) * valVitDiago * Time.deltaTime, Space.World);
            }
            else
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.right), 0.04f);
                transform.Translate(new Vector3(2, 0, 0) * 5.5f * Time.deltaTime, Space.World);
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
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.forward), 0.04f);
                transform.Translate(new Vector3(0, 0, 2) * valVitDiago * Time.deltaTime, Space.World);
            }
            else
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.forward), 0.04f);
                transform.Translate(new Vector3(0, 0, 2) * 5.5f * Time.deltaTime, Space.World);
                autreValeur = true;
            }
        }
        if (Input.GetKey("left"))
        {
            if (autreValeur == true)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(-Vector3.right), 0.04f);
                transform.Translate(new Vector3(-2, 0, 0) * valVitDiago * Time.deltaTime, Space.World);
            }
            else
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(-Vector3.right), 0.04f);
                transform.Translate(new Vector3(-2, 0, 0) * 5.5f * Time.deltaTime, Space.World);
                autreValeur = true;
            }
        }
        if (Input.GetKey("down"))
        {
            if (autreValeur == true)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(-Vector3.forward), 0.04f);
                transform.Translate(new Vector3(0, 0, -2) * valVitDiago * Time.deltaTime, Space.World);
            }
            else
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(-Vector3.forward), 0.04f);
                transform.Translate(new Vector3(0, 0, -2) * 5.5f * Time.deltaTime, Space.World);
                autreValeur = true;
            }

        }
        if (Input.GetKey("right"))
        {
            if (autreValeur == true)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.right), 0.04f);
                transform.Translate(new Vector3(2, 0, 0) * valVitDiago * Time.deltaTime, Space.World);
            }
            else
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.right), 0.04f);
                transform.Translate(new Vector3(2, 0, 0) * 5.5f * Time.deltaTime, Space.World);
                autreValeur = true;
            }

        }
    }
}
