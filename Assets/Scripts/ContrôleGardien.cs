using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContrôleGardien : MonoBehaviour
{
    const float MAX_DIST_BUT = 8f;
    const float MIN_DIST_BUT = -8f;

    const float VITESSE_DÉPLACEMENT = 0.09f;

    const float GRANDEUR_ARC = 15f;

    const float GRANDEUR_MARGE = 0.1f;

    const float CENTRE_ARC_Z = 42f;
    const float CENTRE_ARC_X = 64f;

    const string GARDIEN_1 = "Gardien (1)";
    const string GARDIEN_2 = "Gardien (2)";

    string Nom { get; set; }
    GameObject Balle { get; set; }
    Vector3 PositionBalle { get; set; }
    // Start is called before the first frame update
    void Start()
    {
        Balle = GameObject.FindGameObjectWithTag("Balle");
        Nom = name;
    }

    // Update is called once per frame
    void Update()
    {
        float positionGardienz = transform.position.z;
        PositionBalle = Balle.transform.position;
        PlacerGardienZ(positionGardienz);
        PlacerGardienX(positionGardienz);
        OrienterGardien();

    }
    void OrienterGardien()
    {
        var orientation = PositionBalle;
        orientation.y = transform.position.y;
        transform.LookAt(orientation);
    }
    void PlacerGardienZ(float positionGuardienz)
    {
        if(positionGuardienz + GRANDEUR_MARGE >= PositionBalle.z && positionGuardienz - GRANDEUR_MARGE <= PositionBalle.z)
        {
            
        }
        else
        {
            if (positionGuardienz > PositionBalle.z)
            {
                if (positionGuardienz < MIN_DIST_BUT)
                {

                }
                else
                    transform.Translate(new Vector3(0, 0, -VITESSE_DÉPLACEMENT), Space.World);
            }
            else
            {
                if (positionGuardienz > MAX_DIST_BUT)
                {

                }
                else
                    transform.Translate(new Vector3(0, 0, VITESSE_DÉPLACEMENT), Space.World);
            }
        }
        
    }
    void PlacerGardienX(float positionGuardienz)
    {
        if(Nom == GARDIEN_1)
        {
            float dist = Vector3.Distance(transform.position, PositionBalle);
            if (positionGuardienz > MAX_DIST_BUT || positionGuardienz < MIN_DIST_BUT)
            {

            }
            else
                transform.position = new Vector3(((((-Mathf.Sqrt(-Mathf.Pow(positionGuardienz, 2) + CENTRE_ARC_X)) * dist / GRANDEUR_ARC) + CENTRE_ARC_Z)), transform.position.y, transform.position.z);
        }
        else
        {
            if(Nom == GARDIEN_2)
            {
                float dist = Vector3.Distance(transform.position, PositionBalle);
                if (positionGuardienz > MAX_DIST_BUT || positionGuardienz < MIN_DIST_BUT)
                {

                }
                else
                    transform.position = new Vector3(((((Mathf.Sqrt(-Mathf.Pow(positionGuardienz, 2) + CENTRE_ARC_X)) * dist / GRANDEUR_ARC) - CENTRE_ARC_Z)), transform.position.y, transform.position.z);
            }

        }
    }

}
