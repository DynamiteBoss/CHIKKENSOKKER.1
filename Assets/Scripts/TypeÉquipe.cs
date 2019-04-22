using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TypeÉquipe : NetworkBehaviour
{
    [SyncVar(hook ="OnEstÉquipeAChange")]public bool estÉquipeA;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GameObject capsule = transform.Find("CapsuleIdentité").gameObject;
        if (tag == "Player")
        {
            capsule.GetComponentInChildren<MeshRenderer>().enabled = true;
            if (GetComponent<TypeÉquipe>().estÉquipeA)
            {
                if (name == "Joueur1A")
                {
                    capsule.GetComponent<MeshRenderer>().material.color = Color.blue;
                }
                else
                {
                    capsule.GetComponent<MeshRenderer>().material.color = Color.red;
                }

            }
            else
            {
                if (name == "Joueur1B")
                {
                    capsule.GetComponent<MeshRenderer>().material.color = Color.green;
                }
                else
                {
                    capsule.GetComponent<MeshRenderer>().material.color = Color.yellow;
                }
            }
        }
        else
        {
            capsule.GetComponentInChildren<MeshRenderer>().enabled = false;
        }
    }

    void OnEstÉquipeAChange(bool changement)
    {
        estÉquipeA = changement;
    }
}
