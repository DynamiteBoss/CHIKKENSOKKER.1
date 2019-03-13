using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Networking;

public class ScriptBut : NetworkBehaviour
{
    string NomBut1 = "But1";
    string NomBut2 = "But2";

    [SyncVar]
    int NbButsA = 0;
    [SyncVar]
    int NbButsB = 0;

    [SerializeField]
    bool estÉquipeA = true;

    Text InterfaceScore { get; set; }
    GameObject Ballon { get; set; }
    // Start is called before the first frame update
    void Start()
    {
        InterfaceScore = GameObject.Find("Interface").gameObject.transform.Find("PnlPrincipal").transform.Find("PnlScore").transform.Find("Score").gameObject.GetComponentInChildren<Text>();
        Ballon = this.gameObject;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name.StartsWith("But"))
        {
            Ballon.transform.position = new Vector3(0, 1, 0);
            Ballon.GetComponent<Rigidbody>().isKinematic = true;
            Ballon.transform.parent = null;

            if (other.name == NomBut1)
                ++NbButsA;
            else if (other.name == NomBut2)
                ++NbButsB;

            InterfaceScore.text = (NbButsB).ToString() + "  -  " + (NbButsA).ToString();
        }

        // Ajouter un "Point" à l'équipe 1
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.name.StartsWith("But"))
            Ballon.GetComponent<Rigidbody>().isKinematic = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
