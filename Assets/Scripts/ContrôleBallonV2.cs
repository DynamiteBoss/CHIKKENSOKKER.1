using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ContrôleBallonV2 : NetworkBehaviour
{
    const float TEMPS_MIN = 1f;
    const float FORCE = 60f;

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
        GameObject balle = GameObject.FindGameObjectWithTag("Balle");
        if (!transform.GetComponent<NetworkIdentity>().isLocalPlayer)
        {
            return;
        }
        compteur1 += Time.deltaTime;


        if (Input.GetKeyDown(KeyCode.Space) && compteur1 >= TEMPS_MIN)
        {
            CmdTirerBalle();
        }
    }

    [Command]
    void CmdTirerBalle()
    {
        RpcTirerBalle();

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

    [ClientRpc]
    void RpcTirerBalle()
    {
        GameObject balle = GameObject.FindGameObjectWithTag("Balle");
        balle.transform.GetComponent<Rigidbody>().isKinematic = false;
        balle.GetComponent<SphereCollider>().enabled = true;
        Vector3 direction = new Vector3(balle.transform.position.x - ZoneContrôle.transform.position.x, 0, balle.transform.position.z - ZoneContrôle.transform.position.z).normalized;
        balle.transform.parent = null;
        balle.GetComponent<Rigidbody>().AddForce(direction * FORCE, ForceMode.Impulse);
        AttendrePourDistanceBallon(0.4f, balle);
    }


    IEnumerator AttendrePourDistanceBallon(float durée, GameObject balle)
    {
        balle.GetComponent<SphereCollider>().isTrigger = false;
        yield return new WaitForSeconds(durée);
        balle.GetComponent<SphereCollider>().isTrigger = true;
        balle.GetComponent<PlacerBalle>().enabled = true;
    }
}
