using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoueurV2 : MonoBehaviour
{
    public string NomJoueur { get; set; }

    public string NomÉquipe { get; set; }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public JoueurV2(string nom, string équipe)
    {
        NomJoueur = nom;
        NomÉquipe = équipe;
    }
}
