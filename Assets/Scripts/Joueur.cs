using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Joueur
{
    public string NomJoueur { get; private set; }
    public char Équipe { get; private set; }

    public Joueur(string nom, char teamTag)
    {
        NomJoueur = nom;
        Équipe = teamTag;
    }




    public Joueur(Joueur référenceSurUnObjetExistant)
    {
        NomJoueur = référenceSurUnObjetExistant.NomJoueur;
        Équipe = référenceSurUnObjetExistant.Équipe;
    }
}
