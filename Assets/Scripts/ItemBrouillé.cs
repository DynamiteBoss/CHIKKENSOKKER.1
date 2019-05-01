using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ItemBrouillé : NetworkBehaviour
{
    int compteur = 0;
    static bool furax = false;
    static string name;

    void Start()
    {

    }
    void Update()
    {
        if (furax)
        {
            compteur++;
            if (compteur >= 360)
            {
                GameObject.Find(name).GetComponent<MouvementPlayer>().modeFurax = false;  //arreter mode furax
                furax = false;
                compteur = 0;
            }
        }
    }
    public static void FaireEffetItem(GameObject joueur)  //y faut un joueur qui se fait donner le mode furax
    {
        name = joueur.name;
        joueur.GetComponent<MouvementPlayer>().modeFurax = true;
        furax = true;
        AttendrePourDésactivation(6, joueur);   // LE DÉSACTIVER APRES UN CERTAIN MOMENT (APRES UNE COROUTINE PEUT-ETRE)
    }
    static IEnumerator AttendrePourDésactivation(float durée, GameObject joueur)
    {
        yield return new WaitForSeconds(durée);
        joueur.GetComponent<MouvementPlayer>().modeFurax = false;
        furax = false;
    }
}
