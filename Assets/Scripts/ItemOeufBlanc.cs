using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemOeufBlanc : MonoBehaviour
{
    void Start()
    {

    }
    void Update()
    {

    }

    public static void FaireEffetItem(int indice, GameObject[] listNbOeuf)
    {
        if (indice == 0) // DONC OEUFBLANCGROS
        {
            foreach(GameObject oeuf in listNbOeuf)
            {
                oeuf.transform.localScale = new Vector3(5, 5, 5);
            }
        }
        foreach(GameObject oeuf in listNbOeuf)
        {

        }

        // PAS DE ELSE PARCE QUE OEUFBLANC3 **ET** OEUFBLANCGROS VONT FAIRE LE RESTE DU SCRIPT

        // OEUF QUI VA SE PARTIR DE L'AVANT DU JOUEUR PI QUI VA REBONDIR SUR LES MURS (DONC ***PAS*** DE DRAG)
        // ON TRIGGER ENTER, LAUTRE JOUEUR SE FAIR RAPE SOLIDE      
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {

        }
    }
}
