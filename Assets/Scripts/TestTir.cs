using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TestTir : NetworkBehaviour
{
    GameObject Balle { get; set; }
    // Start is called before the first frame update
    void Start()
    {
        Balle = GameObject.FindGameObjectWithTag("Balle");
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

    private void OnCollisionEnter(Collision other)
    {
        if(other.transform.name == "ZoneTemp")
        {
            transform.parent = other.transform;
            transform.localScale = Vector3.one;
            this.transform.localPosition = new Vector3(0,0,0);
            transform.GetComponent<Rigidbody>().isKinematic = true;
        }
    }


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
