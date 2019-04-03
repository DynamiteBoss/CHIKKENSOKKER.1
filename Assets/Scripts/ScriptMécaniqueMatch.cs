using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class ScriptMécaniqueMatch : MonoBehaviour
{
    [SerializeField]
    const float DuréeMatch = 300f;

    [SerializeField]
    const float DuréePluie = 30f;

    [SerializeField]
    const float DuréeGlace = 30f;

    [SerializeField]
    const float DuréeÉclairs = 20f;

    [SerializeField]
    const float DuréeNuit = 20f;
    [SerializeField]
    bool EstEnModeNuit;
    const int OpacitéMaxPannel = 175;
    const int IntensitéMaxLumiere = 100;
    const float VitesseJourNuit = 100f;

    [SerializeField]
    const float FrequenceObjet = 600f; //10 secondes

    [SerializeField]
    const int NbFramesUpdate = 10;

    const float DimTerrainX = 42f;
    const float DimTerrainZ = 20f;
    const int NbOeufMax = 3;

    Text TxtTimer { get; set; }
    GameObject PnlNuit { get; set; }
    Light LumierePrincipale { get; set; }

    float Timer { get; set; }
    int compteur = 0;
    int compteur2 = 0;

    public int nbOeufs = 0;

    bool ajusteLumiere = false;
    bool modeNuitLocal;


    // Start is called before the first frame update
    void Start()
    {
        Timer = DuréeMatch;
        TxtTimer = GameObject.Find("Interface").transform.Find("PnlPrincipal").transform.Find("PnlScore").transform.Find("Temps").gameObject.GetComponentInChildren<Text>();
        PnlNuit = GameObject.Find("Interface").transform.Find("PnlNuit").gameObject;

        LumierePrincipale = GameObject.Find("LumierePrincipale").GetComponentInChildren<Light>();
        modeNuitLocal = EstEnModeNuit;
    }

    // Update is called once per frame
    void Update()
    {
        ++compteur;
        ++compteur2;
        Timer -= Time.deltaTime;
        if (compteur == NbFramesUpdate + 1)
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
            if (ajusteLumiere)
                AjusterModeNuit();

        }
        if (compteur2 % 20 == 0)
        {
            if (EstEnModeNuit != modeNuitLocal)
            {
                modeNuitLocal = EstEnModeNuit;
                ajusteLumiere = true;
            }
            if (compteur2 == FrequenceObjet)
            {
                compteur2 = 0;
                FaireApparaitreObjet();
            }
        }

    }

    private void FaireApparaitreObjet()
    {
        Vector3 positionObj = new Vector3(UnityEngine.Random.Range(-DimTerrainX, DimTerrainX), 1, UnityEngine.Random.Range(-DimTerrainZ, DimTerrainZ));
        if (nbOeufs < NbOeufMax)
        {
            GameObject OeufHasard = Instantiate((GameObject)Resources.Load("Prefab/Item"), positionObj, Quaternion.identity);
            nbOeufs++;
        }
    }

    private void FaireProgresserMatchUnPas()
    {
        TxtTimer.text = String.Format("{0:m} : {1} ", (((int)Timer) / 60).ToString(), ((int)Timer % 60).ToString().Length == 1 ? "0" + ((int)Timer % 60).ToString() : ((int)Timer % 60).ToString());
    }

    private void TerminerMatch()
    {
        GetComponent<ScriptMenuPause>().DésactiverMouvement();
    }
    void AjusterModeNuit()
    {
        List<GameObject> joueurs = GameObject.FindGameObjectsWithTag("Player")/*.OrderBy(x => int.Parse(x.name[9].ToString()))*/.ToList();

        if (!EstEnModeNuit)
        {
            for (int i = 0; i < joueurs.Count; i++)
            {
                joueurs[i].GetComponentInChildren<Light>().intensity -= 100f * Time.deltaTime * VitesseJourNuit;
            }
            LumierePrincipale.intensity += (1f) * Time.deltaTime * VitesseJourNuit;
            PnlNuit.GetComponentInChildren<Image>().color -= new Color(0, 0, 0, (.585f * Time.deltaTime) * VitesseJourNuit);
        }
        else
        {
            for (int i = 0; i < joueurs.Count; i++)
            {
                joueurs[i].GetComponentInChildren<Light>().intensity += 100f * Time.deltaTime * VitesseJourNuit;
            }
            LumierePrincipale.intensity -= (1f) * Time.deltaTime * VitesseJourNuit;
            PnlNuit.GetComponentInChildren<Image>().color += new Color(0, 0, 0, (.585f * Time.deltaTime) * VitesseJourNuit);
        }

        if (LumierePrincipale.intensity >= 1 || LumierePrincipale.intensity <= 0)
        {
            ajusteLumiere = false;
            //PnlNuit.SetActive(EstEnModeNuit);
            for (int i = 0; i < joueurs.Count; i++)
            {
                joueurs[i].GetComponentInChildren<Light>().intensity = EstEnModeNuit ? 100 : 0;
            }
        }

    }
}
