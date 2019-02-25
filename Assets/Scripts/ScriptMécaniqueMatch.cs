using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScriptMécaniqueMatch : MonoBehaviour
{
    [SerializeField]
    const float DuréeMatch = 300f;
    [SerializeField]
    const int NbFramesUpdate = 10;

    Text TxtTimer { get; set; }

    float Timer { get; set; }
    int compteur = 0;


    // Start is called before the first frame update
    void Start()
    {
        Timer = DuréeMatch;
        TxtTimer = GameObject.Find("Interface").gameObject.transform.Find("PnlPrincipal").transform.Find("PnlScore").transform.Find("Temps").gameObject.GetComponentInChildren<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        ++compteur;
        Timer -= Time.deltaTime;
        if(compteur == NbFramesUpdate + 1)
        {
            compteur = 0;
            if (Timer > 0)
            {
                FaireProgresserMatchUnPas();
            }
            else
            {

                TerminerMatch();
            }

        }

    }

    private void FaireProgresserMatchUnPas()
    {
        TxtTimer.text = String.Format("{0:m} : {1} ", (((int)Timer) / 60).ToString(), ((int)Timer % 60).ToString().Length == 1 ? "0" + ((int)Timer % 60).ToString() : ((int)Timer % 60).ToString());
    }

    private void TerminerMatch()
    {
        throw new NotImplementedException();
    }
}
