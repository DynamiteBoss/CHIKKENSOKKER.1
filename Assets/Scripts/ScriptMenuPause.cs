using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScriptMenuPause : MonoBehaviour
{
    bool menuOuvert = false;
    bool peutOuvrirMenu = true;

    float compteur = 0;

    GameObject[] JoueursPhysiques { get; set; }
    Rigidbody[] JoueursPhysique { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        JoueursPhysiques = GameObject.FindGameObjectsWithTag("Player");
    }

    void DésactiverMouvement()
    {
        for (int i = 0; i < JoueursPhysiques.Length; i++)
        {
            JoueursPhysiques[i].GetComponentInChildren<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            JoueursPhysiques[i].transform.Find("ZoneContrôle").gameObject.SetActive(false);
            JoueursPhysiques[i].GetComponentInChildren<MouvementPlayer>().enabled = false;

        }
    }

    void RéactiverMouvement()
    {
        for (int i = 0; i < JoueursPhysiques.Length; i++)
        {
            JoueursPhysiques[i].GetComponentInChildren<Rigidbody>().constraints = (RigidbodyConstraints)116;
            
            JoueursPhysiques[i].transform.Find("ZoneContrôle").gameObject.SetActive(true);
            JoueursPhysiques[i].GetComponent<MouvementPlayer>().enabled = true;
        }
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
                }
                else
                {
                    SceneManager.UnloadSceneAsync("SceneMenuPause");
                    menuOuvert = false;
                    peutOuvrirMenu = false;
                    RéactiverMouvement();
                }
            }
        }
    }
}
