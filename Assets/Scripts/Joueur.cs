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
            return joueurPhysique;
        }
        set
        {
            joueurPhysique = value;
        }
    }




    public Joueur(string nomJoueur, char teamTag,GameObject joueurPhysique)
    {
        NomJoueur = nomJoueur;
        Équipe = teamTag;
        JoueurPhysique = joueurPhysique;
    }

    public Joueur(Joueur référenceSurUnObjetExistant)
    {
        NomJoueur = référenceSurUnObjetExistant.NomJoueur;
        Équipe = référenceSurUnObjetExistant.Équipe;
    }
}
