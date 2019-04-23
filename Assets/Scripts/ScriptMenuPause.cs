using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScriptMenuPause : MonoBehaviour
{
    public Vector3 velocité;
    public Vector3 angularVelocité;
    GameObject Balle { get; set; }
    GameObject[] liste = new GameObject[10];
    List<GameObject> listeCommune = new List<GameObject>();
    string[] tags = new string[] { "Player", "AI", "Gardien" };
    bool menuOuvert = false;
    bool peutOuvrirMenu = true;

    public bool enPause;
    float compteur = 0;

    GameObject[] JoueursPhysiques { get; set; }
    Rigidbody[] JoueursPhysique { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        JoueursPhysiques = GameObject.FindGameObjectsWithTag("Player");
        Balle = GameObject.FindGameObjectWithTag("Balle");
    }

    public void DésactiverMouvement()
    {
        foreach(string x in tags)
        {
            liste = GameObject.FindGameObjectsWithTag(x);
            foreach(GameObject z in liste)
            {
                listeCommune.Add(z);
            }
        }
        foreach(GameObject x in listeCommune)
        {
            x.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            x.GetComponent<ContrôleBallonV2>().enabled = false;
            if(x.tag == tags[0])
            {
                x.GetComponent<MouvementPlayer>().enabled = false;
            }
            else if(x.tag == tags[1])
            {
                x.GetComponent<ScriptMouvementAI>().enabled = false;
            }
            else if(x.tag == tags[2])
            {
                x.GetComponent<ContrôleGardien>().enabled = false;
            }
        }
        velocité = Balle.GetComponent<Rigidbody>().velocity;
        angularVelocité = Balle.GetComponent<Rigidbody>().angularVelocity;

        Balle.GetComponent<Rigidbody>().velocity = Vector3.zero;
        Balle.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;


        Debug.Log("Velo = " + velocité);
        
        /*
        for (int i = 0; i < JoueursPhysiques.Length; i++)
        {
            JoueursPhysiques[i].GetComponentInChildren<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            JoueursPhysiques[i].transform.Find("ZoneContrôle").gameObject.SetActive(false);
            JoueursPhysiques[i].GetComponentInChildren<MouvementPlayer>().enabled = false;

        }
        */
    }

    public void RéactiverMouvement()
    {
        foreach (string x in tags)
        {
            liste = GameObject.FindGameObjectsWithTag(x);
            foreach (GameObject z in liste)
            {
                listeCommune.Add(z);
            }
        }
        foreach (GameObject x in listeCommune)
        {
            x.GetComponent<ContrôleBallonV2>().enabled = true;
            x.GetComponent<Rigidbody>().constraints = (RigidbodyConstraints)116;
            if (x.tag == tags[0])
            {
                x.GetComponent<MouvementPlayer>().enabled = true;
            }
            else if (x.tag == tags[1])
            {
                x.GetComponent<ScriptMouvementAI>().enabled = true;
            }
            else if (x.tag == tags[2])
            {
                x.GetComponent<ContrôleGardien>().enabled = true;
            }
        }
        Balle.GetComponent<Rigidbody>().velocity = velocité;
        Balle.GetComponent<Rigidbody>().angularVelocity = angularVelocité;
        /*
        for (int i = 0; i < JoueursPhysiques.Length; i++)
        {
            JoueursPhysiques[i].GetComponentInChildren<Rigidbody>().constraints = (RigidbodyConstraints)116;
            
            JoueursPhysiques[i].transform.Find("ZoneContrôle").gameObject.SetActive(true);
            JoueursPhysiques[i].GetComponent<MouvementPlayer>().enabled = true;
        }
        */
    }

    // Update is called once per frame
    void Update()
    {
        if (!peutOuvrirMenu)
        {
            compteur += Time.deltaTime;
            if (compteur >= 1)
                peutOuvrirMenu = true;
        }
        else
        {
            if ((Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown("OptionsBtn1") || Input.GetButtonDown("OptionsBtn2")))               // ALEX CHANGE LE POUR LE BOUTON PAUSE
            {
                if (!menuOuvert)
                {
                    SceneManager.LoadSceneAsync("SceneMenuPause", LoadSceneMode.Additive);
                    menuOuvert = true;
                    peutOuvrirMenu = false;
                    DésactiverMouvement();
                    enPause = true;
                }
                else
                {
                    SceneManager.UnloadSceneAsync("SceneMenuPause");
                    menuOuvert = false;
                    peutOuvrirMenu = false;
                    RéactiverMouvement();
                    enPause = false;
                }
            }
        }
    }
}
