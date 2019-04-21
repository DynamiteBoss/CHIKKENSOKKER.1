using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScriptOptionMenu : MonoBehaviour
{
    private Button Bouton;
    bool menuOuvert = false;
    bool peutOuvrirMenu = true;

    float compteur = 0;

    void Start()
    {
        Bouton = this.GetComponent<Button>();
        Bouton.onClick.AddListener(() => GérerMenuOptions());
    }

    private void GérerMenuOptions()
    {
        if (!menuOuvert)
        {
            SceneManager.LoadSceneAsync("SceneOptionMenu", LoadSceneMode.Additive);
            menuOuvert = true;
            peutOuvrirMenu = false;
        }
        else
        {
            SceneManager.UnloadSceneAsync("SceneOptionMenu");
            menuOuvert = false;
            peutOuvrirMenu = false;
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
    }
    
}
