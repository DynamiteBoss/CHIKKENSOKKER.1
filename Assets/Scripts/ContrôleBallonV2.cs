using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ContrôleBallonV2 : NetworkBehaviour
{
    const float TEMPS_MIN = 1f;
    const float FORCE = 100f;

    GameObject Balle { get; set; }
    Transform ZoneContrôle { get; set; }
    float compteur1 = 0f;
    // Start is called before the first frame update
    void Start()
    {
        Balle = GameObject.FindGameObjectWithTag("Balle");
        ZoneContrôle = this.transform;
    }

    // Update is called once per frame
    void Update()
    {
        //if(!transform.parent.GetComponent<NetworkIdentity>().isLocalPlayer)
        //{
        //    return;
        //}
        compteur1 += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Space) && compteur1 >= TEMPS_MIN)
        {
            TirerBalle();
        }
       /* if (Balle.transform.parent != null)
        {
            if(Input.GetKeyDown("space") && compteur1 >= TEMPS_MIN)
            {
                CmdTirerBalle();
            }
        }
        */
    }

    
    void TirerBalle()
    {
        GameObject balle = GameObject.FindGameObjectWithTag("Balle");
        //balle.GetComponent<NetworkIdentity>().AssignClientAuthority(this.GetComponent<NetworkIdentity>().connectionToClient);
        balle.transform.parent = null;
        //balle.GetComponent<NetworkIdentity>().RemoveClientAuthority(this.GetComponent<NetworkIdentity>().connectionToClient);
        AttendrePourDistanceBallon(0.4f, balle);
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


    IEnumerator AttendrePourDistanceBallon(float durée, GameObject balle)
    {
        balle.GetComponent<SphereCollider>().isTrigger = false;
        yield return new WaitForSeconds(durée);
        balle.GetComponent<SphereCollider>().isTrigger = true;
        balle.GetComponent<PlacerBalle>().enabled = true;
    }
}
