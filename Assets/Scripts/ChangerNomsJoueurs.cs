using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ChangerNomsJoueurs : NetworkBehaviour
{
    int cmptJA = 1;
    int cmptJB = 1;
    int cmptGA = 1;
    int cmptGB = 1;
    int cmptAIA = 1;
    int cmptAIB = 1;
    GameObject[] Player { get; set; }
    GameObject[] Gardien { get; set; }
    GameObject[] AI { get; set; }
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectsWithTag("Player");
        Gardien = GameObject.FindGameObjectsWithTag("Gardien");
        AI = GameObject.FindGameObjectsWithTag("AI");

        ChangerName("Joueur", cmptJA, cmptJB, Player);
        ChangerName("Gardien", cmptGA, cmptGB, Gardien);
        ChangerName("AI", cmptAIA, cmptAIB, AI);
    }

    // Update is called once per frame
    void Update()
    {

    }

   
    void ChangerName(string nom,int cmpt1,int cmpt2,GameObject[] liste)
    {
        foreach (GameObject x in liste)
        {
            if (x.GetComponent<TypeÉquipe>().estÉquipeA)
            {
                x.transform.name = nom + cmpt1 + "A";
                cmpt1++;
            }
            else
            {
                x.transform.name = nom + cmpt2 + "B";
                cmpt2++;
            }
        }
    }
  
   
}
  
    

