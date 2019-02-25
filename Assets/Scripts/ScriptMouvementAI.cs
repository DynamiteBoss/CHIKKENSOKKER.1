using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptMouvementAI : MonoBehaviour
{
    Vector3 PointÀAller { get; set; }

    Rigidbody Ballon { get; set; }

    GameObject But { get; set; }

    [SerializeField]
    int NbFramesAvantUpdate = 10;

    [SerializeField]
    float déplacementParSeconde = 0.1f;

    int compteurFrames = 0;
    float deltaPosition = 0.5f;
    bool aLeBallon;
    string ModePositionnement;
    // Start is called before the first frame update
    void Start()
    {
        Ballon = GameObject.Find("Balle").GetComponentInChildren<Rigidbody>();
        But = GameObject.Find("But1");  //changer pour le but à rechercher
    }

    // Update is called once per frame
    void Update()
    {
        ++compteurFrames;
        DéplacerJoueur();
        RotaterJoueur();
        if (compteurFrames == NbFramesAvantUpdate)
        {
            if(!aLeBallon)
            {
                
                compteurFrames = 0;
                EffectuerMiseÀJour();
            }
            else
            {
                if ((But.transform.position - this.transform.position).magnitude <= 12.5f) //À retirer : mettre qu'on prend le contrôle
                {
                    TirerBallon();
                }
                compteurFrames = 0;

            }

        }
            
    }

    private void EffectuerMiseÀJour()
    {
        CalculerNouvellePosition();
        if(this.transform.Find("Balle"))
        {
            aLeBallon = true;
            PointÀAller = new Vector3(But.transform.position.x, this.transform.position.y, But.transform.position.z);
        }
        else
        {
            aLeBallon = false;
        }
    }

    private void RotaterJoueur()
    {
        if(!aLeBallon)
        {
            Vector3 orientation = Ballon.transform.position;
            orientation.y = -90;
            this.transform.LookAt(new Vector3(orientation.x, this.transform.position.y, orientation.z));
        }
        else
        {
            Vector3 orientation = But.transform.position;
            orientation.y = -90;
            this.transform.LookAt(new Vector3(orientation.x, this.transform.position.y, orientation.z));
        }

        //if (!aLeBallon)
        //{
        //    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Ballon.transform.position - this.transform.position), 0.1f);
        //}
        //else
        //{
        //    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(But.transform.position - this.transform.position), 0.1f);
        //}
    }

    private void TirerBallon()
    {
        
        Ballon.isKinematic = false;
        Ballon.GetComponentInChildren<SphereCollider>().enabled = true;
        Ballon.transform.parent = null;
        Ballon.AddForce((But.transform.position - this.transform.position).normalized * 5, ForceMode.Impulse);
        aLeBallon = false;
    }

    private void CalculerNouvellePosition()
    {
        //  Soit:
        // - "PosAllié_BalleAllié"
        // - "PosAllié_BalleEnnemie"
        // - "PosEnnemie_BalleAlliée"
        // - "PosEnnemie_BalleEnnemie"
        // En fonction de la position du ballon
        PointÀAller = new Vector3(Ballon.position.x, this.transform.position.y, Ballon.position.z);
    }

    private void DéplacerJoueur()
    {
        switch (ModePositionnement)
        {
            case "PosAllié_BalleAllié":
                
                break;
            case "PosAllié_BalleEnnemie":
                
                break;
            case "PosEnnemie_BalleAlliée":
                
                break;
            case "PosEnnemie_BalleEnnemie":
                
                break;
            default:
                /*if (Ballon.velocity.magnitude <= 2)
                    PointÀAller = Ballon.position;
                else*/
                //PointÀAller = new Vector3(PointÀAller.x + UnityEngine.Random.Range(-deltaPosition, deltaPosition), PointÀAller.y, PointÀAller.z + UnityEngine.Random.Range(-deltaPosition, deltaPosition));
                
                this.transform.position += (PointÀAller - this.transform.position).normalized * Time.deltaTime * déplacementParSeconde;

                break;
        }
        //BougerJoueur();

    }

    private void BougerJoueur()
    {
        
    }
}
