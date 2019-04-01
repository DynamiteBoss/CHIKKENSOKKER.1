using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ContrôleBallonV2 : NetworkBehaviour
{
    const float TEMPS_MIN = 1f;
    const float FORCE = 60f;


    GameObject ZoneC { get; set; }
    BoxCollider [] Liste { get; set; }
    GameObject Balle { get; set; }
    Transform ZoneContrôle { get; set; }
    float compteur1 = 0f;
    // Start is called before the first frame update
    void Start()
    {
        Balle = GameObject.FindGameObjectWithTag("Balle");
        ZoneContrôle = this.transform.Find("ZoneContrôle");
    }

    // Update is called once per frame
    void Update()
    {
        Balle = GameObject.FindGameObjectWithTag("Balle");


        if (!isLocalPlayer)
        {
            return;
        }

        compteur1 += Time.deltaTime;
        //if(isLocalPlayer)
        //{
            if(Balle.transform.parent != null)
            {
                if(Balle.transform.parent == transform)
                {
                    if(Input.GetKeyDown(KeyCode.Space) && compteur1 >= TEMPS_MIN)
                    {
                        CmdTirerBalle1();
                    Invoke("CmdTirerBalle",0.01f);
                        //CmdTirerBalle();
                    }
                }
            }
        //}
        
    }

   [Command]
    void CmdTirerBalle()
    {
        Tirer1();
        //Vector3 direction = new Vector3(Balle.transform.position.x - ZoneContrôle.transform.position.x, 0, Balle.transform.position.z - ZoneContrôle.transform.position.z).normalized;
        
        //Balle.GetComponent<Rigidbody>().AddForce(direction*FORCE, ForceMode.Impulse);
        /* if (GameObject.Find("Balle") != null)
         {
             GameObject balle = GameObject.Find("Balle");
             balle.transform.parent  = null;
             /*GameObject balle = GameObject.Find("Balle");
             balle.GetComponent<PlacerBalle>().enabled = false;
             balle.transform.GetComponent<Rigidbody>().isKinematic = false;
             StartCoroutine(AttendrePourDistanceBallon(0.4f, balle));
             balle.GetComponent<SphereCollider>().enabled = true;
             balle.transform.parent = null;
             Vector3 direction = new Vector3(balle.transform.position.x - ZoneContrôle.transform.position.x, 0, balle.transform.position.z - ZoneContrôle.transform.position.z).normalized;
             balle.GetComponent<Rigidbody>().AddForce(direction * FORCE, ForceMode.Impulse);
             balle.GetComponent<NetworkIdentity>().localPlayerAuthority = false;

         }
         */
    }
   
    void Tirer1()
    {
        Vector3 direction = new Vector3(Balle.transform.position.x - ZoneContrôle.transform.position.x, 0, Balle.transform.position.z - ZoneContrôle.transform.position.z).normalized;
        Debug.Log(direction);
        Balle.GetComponent<Rigidbody>().AddForce(direction * FORCE, ForceMode.Impulse);
    }

    [Command]
    void CmdTirerBalle1()
    {
        RpcTirer();
        //Liste = GetComponentsInChildren<BoxCollider>();
        //foreach(BoxCollider x in Liste)
        //{
        //    if(x.transform.tag == "ZoneC")
        //    {
        //        x.enabled = false;
        //    }
        //}
        //ZoneC = GameObject.FindGameObjectWithTag("ZoneC");
        //ZoneC.GetComponent<BoxCollider>().enabled = false;
        //Balle.transform.parent = null;
       
        //foreach (GameObject x in Liste)
        //{
        //    if (x.tag == "ZoneC")
        //    {
        //        x.GetComponent<BoxCollider>().enabled = false;
        //    }
        //}
        //Balle.GetComponent<NetworkTransform>().enabled = true;
        //Balle.GetComponent<NetworkTransform>().enabled = false;
        //Balle.transform.GetComponent<Rigidbody>().isKinematic = false;
        //Balle.GetComponent<SphereCollider>().enabled = true;


        //Vector3 direction = new Vector3(Balle.transform.position.x - ZoneContrôle.transform.position.x, 0, Balle.transform.position.z - ZoneContrôle.transform.position.z).normalized;
        //Balle.GetComponent<Rigidbody>().AddForce(direction * FORCE, ForceMode.Impulse);



        //Balle.GetComponent<SphereCollider>().isTrigger = false;

        //Balle.GetComponent<PlacerBalle>().estPlacer = false;
      
        //Invoke("AttendrePourDistanceBallon", 0.1f);
        //AttendrePourDistanceBallon1(4, balle);
    }
    [ClientRpc]
    void RpcTirer()
    {
        Liste = GetComponentsInChildren<BoxCollider>();
        foreach (BoxCollider x in Liste)
        {
            if (x.transform.tag == "ZoneC")
            {
                x.enabled = false;
            }
        }
        //ZoneC = GameObject.FindGameObjectWithTag("ZoneC");
        //ZoneC.GetComponent<BoxCollider>().enabled = false;
        Balle.transform.parent = null;

        //foreach (GameObject x in Liste)
        //{
        //    if (x.tag == "ZoneC")
        //    {
        //        x.GetComponent<BoxCollider>().enabled = false;
        //    }
        //}
        //Balle.GetComponent<NetworkTransform>().enabled = true;
        //Balle.GetComponent<NetworkTransform>().enabled = false;
        Balle.GetComponent<Rigidbody>().isKinematic = false;
        //Balle.GetComponent<SphereCollider>().enabled = true;


        //Vector3 direction = new Vector3(Balle.transform.position.x - ZoneContrôle.transform.position.x, 0, Balle.transform.position.z - ZoneContrôle.transform.position.z).normalized;
        //Balle.GetComponent<Rigidbody>().AddForce(direction * FORCE, ForceMode.Impulse);



        //Balle.GetComponent<SphereCollider>().isTrigger = false;

        //Balle.GetComponent<PlacerBalle>().estPlacer = false;

        Invoke("AttendrePourDistanceBallon", 0.5f);
        //AttendrePourDistanceBallon1(4, balle);
    }


    IEnumerator AttendrePourDistanceBallon1(float durée, GameObject balle)
    {
        balle.GetComponent<SphereCollider>().isTrigger = false;
        yield return new WaitForSeconds(durée);
        balle.GetComponent<SphereCollider>().isTrigger = true;
        balle.GetComponent<PlacerBalle>().enabled = true;
    }

    void AttendrePourDistanceBallon()
    {
        //Balle.GetComponent<NetworkTransform>().enabled = true;
        //Balle.GetComponent<SphereCollider>().isTrigger = true;
        Balle.GetComponent<PlacerBalle>().enabled = true;
        //foreach (GameObject x in Liste)
        //{
        //    if (x.tag == "ZoneC")
        //    {
        //        x.GetComponent<BoxCollider>().enabled = true;
        //    }
        //}
        foreach (BoxCollider x in Liste)
        {
            if (x.transform.tag == "ZoneC")
            {
                x.enabled = true;
            }
        }
        Balle.GetComponent<Rigidbody>().isKinematic = false;
        //ZoneC.GetComponent<BoxCollider>().enabled = true;
    }
}
