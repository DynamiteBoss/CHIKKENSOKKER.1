using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Joueur
{
    public string NomJoueur { get; private set; }
    public char Équipe { get; private set; }
    public GameObject joueurPhysique;

    public GameObject JoueurPhysique
    {
        get
        {
            return this.joueurPhysique;
        }
        set
        {

        }
    }




    public Joueur(string nomJoueur, char teamTag)
    {
        NomJoueur = nomJoueur;
        Équipe = teamTag;
    }

    public Joueur(Joueur référenceSurUnObjetExistant)
    {
        NomJoueur = référenceSurUnObjetExistant.NomJoueur;
        Équipe = référenceSurUnObjetExistant.Équipe;
    }
}
