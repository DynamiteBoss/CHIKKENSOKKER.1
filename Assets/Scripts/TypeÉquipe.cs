using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TypeÉquipe : NetworkBehaviour
{
    GameObject Balle { get; set; }
    GameObject Camera { get; set; }
    [SyncVar(hook = "OnEstÉquipeAChange")] public bool estÉquipeA;
    // Start is called before the first frame update
    void Start()
    {
        Balle = GameObject.FindGameObjectWithTag("Balle");
        Camera = GameObject.FindGameObjectWithTag("MainCamera");
    }
    bool EstEnPause()
    {
        return Camera.GetComponent<ScriptMécaniqueMatch>().enPause || Camera.GetComponent<ScriptMenuPause>().enPause || Balle.GetComponent<ScriptBut>().enPause;
    }
    [Command]
    void CmdPlacer()
    {
        RpcPlacer();
    }
    [ClientRpc]
    void RpcPlacer()
    {
        GameObject.Find("Corps").transform.rotation = new Quaternion(0, 0, 0, 0);
    }
    // Update is called once per frame
    void Update()
    {
        CmdPlacer();
        if (name.StartsWith("Player"))
        {
            tag = "Player";
            if (!EstEnPause())
            {
                GetComponent<MouvementPlayer>().enabled = true;
                GetComponent<ScriptMouvementAI>().enabled = false;
            }
        }
        else
        {
            if (name.StartsWith("AI"))
            {
                tag = "AI";
                if (!EstEnPause())
                {
                    GetComponent<ScriptMouvementAI>().enabled = true;
                    GetComponent<MouvementPlayer>().enabled = false;
                }
            }
            else
            {
                if (name.StartsWith("Gardien"))
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
                    capsule.GetComponent<MeshRenderer>().material.color = Color.red;
                }
                else
                {
                    if (name == "Joueur2A")
                    {
                        capsule.GetComponent<MeshRenderer>().material.color = Color.yellow;
                    }

                }

            }
            else
            {
                if (name == "Joueur1B")
                {
                    capsule.GetComponent<MeshRenderer>().material.color = Color.blue;
                }
                else
                {
                    if (name == "Joueur2B")
                    {
                        capsule.GetComponent<MeshRenderer>().material.color = Color.green;
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
