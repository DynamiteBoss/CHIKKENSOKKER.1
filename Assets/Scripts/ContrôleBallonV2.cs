using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ContrôleBallonV2 : NetworkBehaviour
{
    const float TEMPS_MIN = 1f;
    const float FORCE = 60f;
    GameObject Gardien { get; set; }
    GameObject ZoneC { get; set; }
    BoxCollider [] Liste { get; set; }
    GameObject Balle { get; set; }
    Transform ZoneContrôle { get; set; }
    Vector3 Courbe { get; set; }

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
      
        if (tag == "Player")
        {
            if (Balle.transform.parent != null)
            {
                if (Balle.transform.parent == transform)
                {
                    if(name.StartsWith("Joueur1"))
                    {
                        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("CircleBtn1")) && compteur1 >= TEMPS_MIN)
                        {
                            CmdTirerBalle1();
                            
                            CmdTirerBalle();
                            
                        }
                    }
                    else
                    {
                        if ((Input.GetKeyDown(KeyCode.Keypad3) || Input.GetButtonDown("CircleBtn2")) && compteur1 >= TEMPS_MIN)
                        {
                            CmdTirerBalle1();
                           
                            CmdTirerBalle();
                          
                        }
                    }
                }
            }
        }
   
        
    }

   [Command]
    void CmdTirerBalle()
    {
        RpcTirer1();
        
    }
   [ClientRpc]
    void RpcTirer1()
    {
        Vector3 direction = new Vector3(Balle.transform.position.x - ZoneContrôle.transform.position.x, 0, Balle.transform.position.z - ZoneContrôle.transform.position.z).normalized;
        if (Input.GetKey(KeyCode.X) && Input.GetKeyDown(KeyCode.Space))
        {
            Balle.GetComponent<Rigidbody>().AddForce(direction * FORCE, ForceMode.Impulse);
            Balle.GetComponent<Rigidbody>().AddForce(Vector3.up * FORCE, ForceMode.Acceleration);
            
        }
        else if (Input.GetKey(KeyCode.Z) && Input.GetKeyDown(KeyCode.Space))
        {
            Balle.GetComponent<Rigidbody>().AddForce(direction * FORCE, ForceMode.Impulse);
            Balle.GetComponent<Rigidbody>().AddForce(Vector3.up * FORCE, ForceMode.Acceleration);
            
        }
        else
        {
           
           Balle.GetComponent<Rigidbody>().AddForce(direction.normalized * FORCE, ForceMode.Impulse);
        }

    }

    [Command]
    void CmdTirerBalle1()
    {
        RpcTirer();
       
    }
    [ClientRpc]
    void RpcTirer()
    {
        GameObject gardien = Balle.transform.parent.gameObject;
        Balle.GetComponent<PlacerBalle>().dernierPosseseur = this.gameObject.name;
        Liste = GetComponentsInChildren<BoxCollider>();
        foreach (BoxCollider x in Liste)
        {
            if (x.transform.tag == "ZoneC")
            {
                x.enabled = false;
            }
        }
       
        Balle.transform.parent = null;
        
      
        Balle.GetComponent<Rigidbody>().isKinematic = false;
        
        Balle.GetComponent<PlacerBalle>().estPlacer = false;
        Balle.GetComponent<SphereCollider>().enabled = true;
        Invoke("AttendrePourDistanceBallon", 0.5f);
        
        Balle.GetComponent<PlacerBalle>().positionJouer = this.transform.position;

        if (Balle.GetComponent<PlacerBalle>().AncienGardien == "Gardien")
        {

            Gardien = gardien;

            GameObject[] listeAI = new GameObject[8];
            List<GameObject> listeAIMonÉquipe = new List<GameObject>();
            listeAI = GameObject.FindGameObjectsWithTag("AI");
            string équipe;

            foreach (GameObject x in listeAI)
            {
                if (gardien.GetComponent<TypeÉquipe>().estÉquipeA == x.GetComponent<TypeÉquipe>().estÉquipeA)
                {
                    listeAIMonÉquipe.Add(x);
                }
            }
            if (gardien.GetComponent<TypeÉquipe>().estÉquipeA)
            {
                équipe = "A";
            }
            else
            {
                équipe = "B";
            }
            GameObject joueur = listeAIMonÉquipe[listeAIMonÉquipe.Count-1];
            ChangerJoueurÀGardien(joueur,gardien, équipe);
            Balle.GetComponent<PlacerBalle>().dernierPosseseur = GameObject.Find("Joueur1" + équipe).name;
           
            
        }
       
    }
    void RéactiverSave()
    {
        Gardien.GetComponentInChildren<GérerProbabilitéArrêt>().enabled = true;
        Gardien.transform.Find("ZoneArrêt").GetComponent<BoxCollider>().enabled = true;
    }
    void ChangerJoueurÀGardien(GameObject joueur,GameObject gardien,string équipe)
    {
        joueur.name = "Joueur1" + équipe;
        joueur.GetComponent<ScriptMouvementAI>().enabled = false;
        joueur.GetComponent<MouvementPlayer>().enabled = true;
        joueur.tag = "Player";

        gardien.name = "Gardien1" + équipe;
        gardien.transform.Find("ZoneArrêt").GetComponent<GérerProbabilitéArrêt>().enabled = false;
        gardien.GetComponent<MouvementPlayer>().enabled = false;
        gardien.GetComponent<ContrôleGardien>().enabled = true;
        gardien.GetComponentInChildren<ActionPlaquageGardien>().enabled = true;
        
        gardien.tag = "Gardien";

        Balle.GetComponent<PlacerBalle>().AncienGardien = "PasGardien";
        Invoke("RéactiverSave", 0.5f);
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
       
        Balle.GetComponent<PlacerBalle>().enabled = true;
       
        foreach (BoxCollider x in Liste)
        {
            if (x.transform.tag == "ZoneC")
            {
                x.enabled = true;
            }
        }
      
    }
}
