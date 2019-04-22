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
        if(name.StartsWith("Player"))
        {
            tag = "Player";
            GetComponent<MouvementPlayer>().enabled = true;
            GetComponent<ScriptMouvementAI>().enabled = false;
        }
        else
        {
            if(name.StartsWith("AI"))
            {
                tag = "AI";
                GetComponent<ScriptMouvementAI>().enabled = true;
                GetComponent<MouvementPlayer>().enabled = false;
            }
            else
            {
                if(name.StartsWith("Gardien"))
                {
                    tag = "Gardien";

                }
            }
        }
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
                    if(name == "Joueur2A")
                    {
                        capsule.GetComponent<MeshRenderer>().material.color = Color.red;
                    }
                   
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
                    if(name == "Joueur2B")
                    {
                        capsule.GetComponent<MeshRenderer>().material.color = Color.yellow;
                    }
                    
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
