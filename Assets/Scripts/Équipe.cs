using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Équipe
{
    const int TAILLE = 5;
    Joueur[] Joueurs { get; set; }
    public int NbJoueurs { get; private set; }
    GameObject joueur = Resources.Load<GameObject>("Prefabs/Joueur");
    GameObject gardien = Resources.Load<GameObject>("Prefabs/Gardien");

    public Joueur this[int indice]
    {
        get
        {
            return Joueurs[indice];
        }
    }
    /// <summary>
    /// Équipe créée lorsqu'un joueur se connecte
    /// </summary>
    /// <param name="teamTag"></param>
    public Équipe(char teamTag)
    {

        NbJoueurs = TAILLE;
        Joueurs = new Joueur[TAILLE];
        for(int i=0; i != TAILLE;i++)
        {
            Joueurs[i] = new Joueur("Joueur-" + i+1 + "-" + teamTag, teamTag, joueur);
            //  Joueurs[i] = new Joueur($"Joueur-{i+1}-{teamTag}", teamTag);
            if (i == TAILLE-1)
            {
                Joueurs[i] = new Joueur("Gardien-" + teamTag, teamTag, gardien);
                // Joueurs[i] = new Joueur($"Gardien-{teamTag}", teamTag);
            }
        }
    }

}
