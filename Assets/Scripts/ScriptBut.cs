using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Networking;

public class ScriptBut : NetworkBehaviour
{
    const float TEMPS_MIN = 1f;
    string NomBut1 = "But1";
    string NomBut2 = "But2";

    [SyncVar(hook = "OnButChangeA")]
    public int NbButsA = 0;
    [SyncVar(hook = "OnButChangeB")]
    public int NbButsB = 0;

    [SyncVar(hook ="OnScoreChange")]
    public string score = 0 + "  -  " + 0;

    [SyncVar(hook = "OnTimeChange")]
    public float compteur = 0;

    [SerializeField]
    bool estÉquipeA = true;

    
    Text InterfaceScore { get; set; }

    GameObject Ballon { get; set; }
    // Start is called before the first frame update
    void Start()
    {
        InterfaceScore = GameObject.Find("Interface").transform.Find("PnlPrincipal").transform.Find("PnlScore").transform.Find("Score").GetComponent<Text>();
        Ballon = this.gameObject;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "But" && compteur >= TEMPS_MIN)
        {
            compteur = 0;
            Ballon.transform.position = new Vector3(0, 1, 0);
            Ballon.GetComponent<Rigidbody>().isKinematic = true;
            Ballon.transform.parent = null;

            if (other.name == NomBut1)
                ++NbButsA;
            else if (other.name == NomBut2)
                ++NbButsB;

            score = NbButsB.ToString() + "  -  " + NbButsA.ToString();

            compteur = 0;
        }

        // Ajouter un "Point" à l'équipe 1
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "But")
            Ballon.GetComponent<Rigidbody>().isKinematic = false;
    }

    void OnButChangeA(int but)
    {
        NbButsA = but;
       // InterfaceScore.text = (NbButsB).ToString() + "  -  " + (NbButsA).ToString();
    }
    void OnButChangeB(int but)
    {
        NbButsB = but;
       // InterfaceScore.text = (NbButsB).ToString() + "  -  " + (NbButsA).ToString();
    }
    void OnScoreChange(string change)
    {
        score = change;
        InterfaceScore.text = score;
    }
    void OnTimeChange(float temps)
    {
        compteur = temps;
    }
    // Update is called once per frame
    void Update()
    {
        compteur += Time.deltaTime;
    }
    void DéplacerÉquipe()
    {

    }
}
