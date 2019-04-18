using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class JoueurV2
{
    public string NomJoueur { get; set; }

    public string NomÉquipe { get; set; }
    public GameObject Prefab { get; set; }

    public JoueurV2(string nom, string équipe,GameObject prefab)
    {
        NomJoueur = nom;
        NomÉquipe = équipe;
        Prefab = prefab;
    }
}
