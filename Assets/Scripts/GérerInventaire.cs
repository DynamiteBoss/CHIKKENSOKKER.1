using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GérerInventaire : MonoBehaviour
{
    void Start()
    {

    }
    void Update()
    {
        AfficherInventaire('A', 1);
        //AfficherInventaire('B', 1);    //temporaire en attendant parce que anyway on va pa pouvoir utiliser d'item si ya juste un player sur le jeu (quan Joueur1B existe pas ca fok)
        AfficherInventaire('A', 2);
        //AfficherInventaire('B', 2);     //temporaire en attendant parce que anyway on va pa pouvoir utiliser d'item si ya juste un player sur le jeu (quan Joueur1B existe pas ca fok)
    }

    private void AfficherInventaire(char équipe, int position)
    {
        if (GameObject.Find("Joueur" + "1" + équipe).GetComponent<ScriptItems>().Inventaire.Count > 0)
        {
            switch (GameObject.Find("Joueur" + "1" + équipe).GetComponent<ScriptItems>().Inventaire[position - 1])    //FAIRE QUE LE PLAYER 2 PUISSE UTILISER LES MEME ITEMS QUE LE PLAYER 1
            {
                case 0:
                    AfficherSprite(position, équipe, "crotte3");
                    return;
                case 1:
                    AfficherSprite(position, équipe, "crotte5");
                    return;
                case 2:
                    AfficherSprite(position, équipe, "oeufXL");
                    return;
                case 3:
                    AfficherSprite(position, équipe, "oeuf3");
                    return;
                case 4:
                    AfficherSprite(position, équipe, "torpille");
                    return;
                case 5:
                    AfficherSprite(position, équipe, "bombe");
                    return;
                case 6:
                    AfficherSprite(position, équipe, "brouillé");
                    return;
                case 7:
                    AfficherSprite(position, équipe, "versdeterre");
                    return;
                default:
                    return;
            }
        }
    }
    private void AfficherSprite(int position, char équipe, string nomSprite)
    {
        GameObject.Find("Objet" + position + équipe).GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Image/" + nomSprite);
    }
}
