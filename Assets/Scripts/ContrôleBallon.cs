﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ContrôleBallon : NetworkBehaviour
{
    const float FORCE = 40f;
    GameObject Balle { get; set; }
    Transform ZoneContrôle { get; set; }
    string Nom { get; set; }
    float compteur1 = 0;
    float compteur2 = 0;

    void Start()
    {
        Nom = transform.parent.name;
        ZoneContrôle = this.transform;
        Balle = GameObject.Find("Balle");
    }
   
    void Update()
    {
        compteur1 += Time.deltaTime;
        compteur2 += Time.deltaTime;

        if(Balle.transform.parent != null)
        {
            //Balle.transform.localPosition = new Vector3(0, 1.5f, 2);
            if (Input.GetKeyDown("space") && compteur1 >= 1f)
            {
                if (ZoneContrôle.parent.Find("Balle") != null)
                {
                    GameObject balle = ZoneContrôle.parent.Find("Balle").gameObject;
                    if (balle.transform.parent = ZoneContrôle.parent)
                    {
                        CmdTirerBallon();
                    }
                    compteur1 = 0;
                }
            }
            //if (Input.GetKeyDown(KeyCode.RightShift) && compteur2 >= 1f && Nom == NOM_PLAYER_2)
            //{
            //    GameObject balle = ZoneContrôle.parent.Find("Balle").gameObject;
            //    if (balle.transform.parent = ZoneContrôle.parent)
            //    {
            //        TirerBallon(balle);
            //    }
            //    compteur2 = 0;
            //}
        }
        
    }

   
    void CmdTirerBallon()
    {

        if (GameObject.Find("Balle") != null)
        {
            GameObject balle = GameObject.Find("Balle").gameObject;
            balle.transform.GetComponentInChildren<Rigidbody>().isKinematic = false;
            StartCoroutine(AttendrePourDistanceBallon(0.1f, balle));
            balle.GetComponent<SphereCollider>().enabled = true;
            balle.transform.parent = null;
            balle.GetComponentInChildren<Rigidbody>().AddForce(new Vector3(balle.transform.position.x - ZoneContrôle.transform.parent.position.x, 0, balle.transform.position.z - ZoneContrôle.transform.parent.position.z).normalized * FORCE, ForceMode.Impulse);
        }
        //RpcTirerBallon(balle);
    }
    
  
    private void RpcTirerBallon(GameObject balle)
    {
        if (balle != null)
        {
            balle.transform.GetComponentInChildren<Rigidbody>().isKinematic = false;
            StartCoroutine(AttendrePourDistanceBallon(0.1f, balle));
            balle.GetComponent<SphereCollider>().enabled = true;
            balle.transform.parent = null;
            balle.GetComponentInChildren<Rigidbody>().AddForce(new Vector3(balle.transform.position.x - ZoneContrôle.transform.parent.position.x, 0, balle.transform.position.z - ZoneContrôle.transform.parent.position.z).normalized * FORCE, ForceMode.Impulse);

        }
    }

    IEnumerator AttendrePourDistanceBallon(float durée,GameObject balle)
    {
        GetComponent<BoxCollider>().isTrigger = false;
        yield return new WaitForSeconds(durée);
        GetComponent<BoxCollider>().isTrigger = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.name == "Balle" && other.transform.parent == null)
        {
            MettreBalleEnfant(other);
            CalculerDistanceBalle();
        }
    }
    
    private void MettreBalleEnfant(Collider other)
    {
        //changer pour pas qu'on puisse prendre le ballon  aquelquun qui la deja
        if (other.transform.parent == null)
        {
            //faire en sorte que lautre player puisse pa faire bouger le ballon
            other.transform.parent = ZoneContrôle.parent;
            other.transform.localScale = Vector3.one;
            other.transform.GetComponent<Rigidbody>().isKinematic = true;
            other.GetComponent<SphereCollider>().enabled = false;
        }     
    }
 
    private void CalculerDistanceBalle()
    {
        Balle.transform.localPosition = new Vector3(0, 1.5f, 2);
        //mettre la balle vers le milieu de la zone de controle
    }
}
