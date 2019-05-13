using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Text;

public class ScriptAnimationPattes : MonoBehaviour
{
    [SerializeField]
    GameObject PatteGauche;

    [SerializeField]
    GameObject PatteDroite;
    int compteur = 0;

    const float VitessePattes = 3f;
    const float AmplitudePattes = 36f;
    const float ThresholdMvtPattes = 0.05f;

    Vector3 positionAvant { get; set; }

  
    void Start()
    {
      
        positionAvant = this.transform.position;
    }


    // Update is called once per frame
    void Update()
    {
        ++compteur;
        if (compteur % 2 == 0)
        {
            if ((positionAvant - this.transform.position).magnitude >= ThresholdMvtPattes)
            {
                compteur = compteur + 1 <= 360 ? compteur + 1 : -360;
                PatteDroite.transform.localRotation = Quaternion.Euler(this.GetComponentInChildren<MouvementPlayer>().modeFurax ? AmplitudePattes* 1.25f * Mathf.Sin(2 * Mathf.PI / 90 * VitessePattes * (compteur)) : AmplitudePattes * Mathf.Sin(2 * Mathf.PI / 180 * VitessePattes * (compteur)), 0, 0);
                PatteGauche.transform.localRotation = Quaternion.Euler(-PatteDroite.transform.localRotation.eulerAngles.x, 0, 0);//Quaternion.Euler(this.GetComponent<MouvementPlayer>().modeFurax ? Time.deltaTime * 2f * AmplitudePattes * Mathf.Sin(VitessePattes * compteur) : Time.deltaTime * AmplitudePattes * Mathf.Sin(VitessePattes * compteur), 0, 0);
                                                                                                                                 //PatteDroite.transform.rotation = Quaternion.Euler(Time.deltaTime * AmplitudePattes * Mathf.Sin(2*Mathf.PI/180 * VitessePattes * compteur) - 2 * AmplitudePattes, 0, 0);
                                                                                                                                 //PatteGauche.transform.rotation = Quaternion.Euler(-PatteDroite.transform.localRotation.eulerAngles.x, 0, 0);
            }
            else
            {
                compteur = 0;
                PatteDroite.transform.localRotation = Quaternion.Euler(Vector3.zero);
                PatteGauche.transform.localRotation = Quaternion.Euler(Vector3.zero);
            }
            positionAvant = this.transform.position;
        }
    }
}
