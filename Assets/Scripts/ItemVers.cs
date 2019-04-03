using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemVers : MonoBehaviour
{
    void Start()
    {
        
    }
    void Update()
    {
        
    }
    public static void FaireEffetItem(GameObject joueur)  //y faut un joueur qui se fait donner le mode invincible
    {
        joueur.GetComponent<MouvementPlayer>().modeInvincible = true;
        AttendrePourDésactivation(6, joueur);   // LE DÉSACTIVER APRES UN CERTAIN MOMENT (APRES UNE COROUTINE PEUT ETRE)
    }
    static IEnumerator AttendrePourDésactivation(float durée, GameObject joueur)
    {
        yield return new WaitForSeconds(durée);
        joueur.GetComponent<MouvementPlayer>().modeInvincible = false;
    }
}
