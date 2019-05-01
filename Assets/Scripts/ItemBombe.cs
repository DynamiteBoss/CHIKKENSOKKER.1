using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class ItemBombe : NetworkBehaviour
{
    GameObject zoneExplosion;
    private bool explosée;
    private bool changementGrandeurZone;
    float scaleMax;
    float scale = 0.5f;
    float tempsExplosion = 45; //le temps ou le scale augmente, durant les premieres frames (30)
    float tempsFade = 127.5f;
    float transparence = 254;
    float transparenceMin = 0;
    float transparenceMax = 255;
    static bool changerTrajectoireLive;
    int compteur;

    void Start()
    {
        changementGrandeurZone = false;
        changerTrajectoireLive = false;
    }
    void Update()
    {
        if (this.transform.position.y >= 7)
        {
            GetComponent<Rigidbody>().AddForce(Vector3.zero, ForceMode.Force);
            GetComponent<Rigidbody>().AddForce((transform.up * 100) + (transform.forward * 200), ForceMode.Force);
        }
        if (changementGrandeurZone)
        {
            if (scale < scaleMax)
            {
                zoneExplosion.transform.localScale = zoneExplosion.transform.localScale + new Vector3(scaleMax / tempsExplosion, scaleMax / tempsExplosion, scaleMax / tempsExplosion);
                scale = zoneExplosion.transform.localScale.x;
            }
            else
            {
                //CA VA PA DANS CE CODE LA (POUR LA TRANSPAREMNCE) DONC CA NE DESTROY PA
                if (transparence > transparenceMin)
                {
                    zoneExplosion.transform.GetComponentInChildren<Renderer>().material.color = new Color(zoneExplosion.transform.GetComponentInChildren<Renderer>().material.color.r, zoneExplosion.transform.GetComponentInChildren<Renderer>().material.color.g, zoneExplosion.transform.GetComponentInChildren<Renderer>().material.color.b, (transparence - (transparenceMax / tempsFade)));
                    transparence = zoneExplosion.transform.GetComponentInChildren<Renderer>().material.color.a;
                }
                else
                {
                    changementGrandeurZone = false;
                    Destroy(zoneExplosion);
                }
            }
        }
    }

    public static void FaireEffetItem(GameObject item)
    {
        item.GetComponent<Rigidbody>().AddForce((item.transform.up * 100) + (item.transform.forward * -225), ForceMode.Force);
        // OEUF BOMBE QUI FAIT UN ARC DE CERCLE AVANT DEXPLOSER A TERRE 
        // ON TRIGGER ENTER, LAUTRE JOUEUR 
    }
    public void OnTriggerEnter(Collider other)
    {
        
        if (other.name == ("Terrain"))
        {
            this.GetComponentInChildren<MeshRenderer>().enabled = false;
            this.GetComponentInChildren<SphereCollider>().enabled = false;
            this.GetComponentInChildren<GestionAudio>().FaireJouerSon(this.GetComponents<AudioSource>().Where(x => x.clip.name.StartsWith("Oeuf - Son")).First());
            Explose(50);
            StartCoroutine("DeleteBombe");
        }
    }
    IEnumerable DeleteBombe()
    {
        yield return new WaitForSeconds(.1f);
        Destroy(this.transform.gameObject);
    }
    private void Explose(float scale) 
    {
        this.GetComponentInChildren<GestionAudio>().FaireJouerSon(this.GetComponents<AudioSource>().Where(x => x.clip.name.StartsWith("explo")).First());

        zoneExplosion = Instantiate(Item.RetournerItemListe(16).ItemPhysique, this.transform.position + Vector3.up * 2, Quaternion.identity);
        changementGrandeurZone = true;
        scaleMax = scale;

        // créer la zone d'explosion avec le rayon donné
        // agrandir la zone d'explosion avec le temps
    }
}
