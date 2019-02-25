using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouvementManette : MonoBehaviour
{
    const string NOM_PLAYER_1 = "Player (1)";
    const string NOM_PLAYER_2 = "Player (2)";

    string Nom { get; set; }

    bool autreValeur;
    float valVitDiago = 5.5f / Mathf.Sqrt(2);
    void Start()
    {
        Nom = name;
    }
    void Update()
    {
        // autreValeur = false;
        if (Nom == NOM_PLAYER_1)
            DéplacerManette(1);
        else
        {
            if (Nom == NOM_PLAYER_2)
                DéplacerManette(2);
            else { }
        }
    }
    void DéplacerManette(int number)
    {
        float k = Input.GetAxis("LeftJoystickHorizontal" + number.ToString());
        {
            if (k > 0)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(new Vector3(1, 0)), 0.1f);
            }
            else
            {
                if (k < 0)
                {
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(new Vector3(-1, 0)), 0.1f);
                }
            }
            transform.Translate(new Vector3(2 * k, 0, 0) * 5.5f * Time.deltaTime, Space.World);
        }

        float j = Input.GetAxis("LeftJoystickVertical" + number.ToString());
        {
            if (j > 0)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(new Vector3(0, 0, 1)), 0.1f);
            }
            else
            {
                if (j < 0)
                {
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(new Vector3(0, 0, -1)), 0.1f);
                }
            }
            transform.Translate(new Vector3(0, 0, 2 * j) * 5.5f * Time.deltaTime, Space.World);
        }
    }
}
