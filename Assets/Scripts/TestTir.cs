using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TestTir : NetworkBehaviour
{
    public bool estPlacer;
    GameObject Balle { get; set; }
    // Start is called before the first frame update
    void Start()
    {
        Balle = GameObject.FindGameObjectWithTag("Balle");
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.parent.tag != "Player")
        {

        }
        else
        {
            MettreBalleEnfant(other);
        }
    }
    // Update is called once per frame
    void Update()
    {

        if (!isLocalPlayer)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            CmdTire();
        }

        if(Input.GetKeyDown(KeyCode.M))
        {
            CmdTirer();
        }
    }

    private void MettreBalleEnfant(Collider other)
    {
        //changer pour pas qu'on puisse prendre le ballon  aquelquun qui la deja
        if (other.tag == "ZoneC")
        {
            estPlacer = true;
            //GetComponent<NetworkTransform>().enabled = false;
            this.transform.parent = other.transform.parent;
            transform.localScale = Vector3.one;

            this.transform.localPosition = new Vector3(0, 1.5f, 2);
            Debug.Log(transform.localPosition);
            transform.GetComponent<Rigidbody>().isKinematic = true;


        }
    }

    //private void OnCollisionEnter(Collision other)
    //{
    //       transform.parent = other.transform;
    //        transform.localScale = Vector3.one;
    //        this.transform.localPosition = new Vector3(0,0,0);
    //        transform.GetComponent<Rigidbody>().isKinematic = true;
    //}


    [Command]
    void CmdTire()
    {
        Balle.GetComponent<Rigidbody>().AddForce(new Vector3(0, 0, 10), ForceMode.Impulse);
        //RpcTire();
    }
    [ClientRpc]
    void RpcTire()
    {
        Balle.GetComponent<Rigidbody>().AddForce(new Vector3(0, 0, 10), ForceMode.Impulse);
    }

    [Command]
    void CmdTirer()
    {
        Balle.GetComponent<Rigidbody>().AddForce(new Vector3(0, 0, -10), ForceMode.Impulse);
        //RpcTirer();
    }
    [ClientRpc]
    void RpcTirer()
    {
        Balle.GetComponent<Rigidbody>().AddForce(new Vector3(0, 0, -10), ForceMode.Impulse);
    }
}
