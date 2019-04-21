using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.Networking;
using UnityEngine;
using UnityEngine.UI;

public class ScriptGestionOpts : MonoBehaviour
{
    int compteur = 25;

    int Xbox_One_Controller = 0;
    int PS4_Controller = 0;

    //string cheminAcces = Application.dataPath + "/Resources/Options.txt"; //https://stackoverflow.com/questions/50716171/unity-read-text-file-from-resources-folder

    Options Opts { get; set; }

    Transform PnlOptsNonMod { get; set; }
    Transform PnlOptsMod { get; set; }

    Toggle TogRéseau { get; set; }
    Toggle TogManette { get; set; }

    Text TxtNbManettes { get; set; }
    Text TxtAdresseIP { get; set; }

    Slider SldFréquenceObjets { get; set; }
    Slider SldFréquencePluie { get; set; }
    Slider SldFréquenceOrages { get; set; }
    Slider SldNbMaxObjets { get; set; }

    Text TxtValeurNbObj { get; set; }
    Text TxtValeurFreqPluie { get; set; }
    Text TxtValeurFreqOrages { get; set; }
    Text TxtValeurFreqObj { get; set; }

    Text TxtCheminAcces { get; set; }


    // Start is called before the first frame update
    void Start()
    {
        Opts = new Options();
        PnlOptsNonMod = this.transform.Find("PnlOptsNonMod");
        PnlOptsMod = this.transform.Find("PnlOptsMod");

        TogManette = PnlOptsNonMod.Find("TogManette").gameObject.GetComponentInChildren<Toggle>();
        TogRéseau = PnlOptsNonMod.Find("TogRéseau").gameObject.GetComponentInChildren<Toggle>();
        TxtNbManettes = PnlOptsNonMod.Find("TxtNbManettes").gameObject.GetComponentInChildren<Text>();
        TxtAdresseIP= PnlOptsNonMod.Find("TxtAdresseIP").gameObject.GetComponentInChildren<Text>();

        SldFréquenceObjets = PnlOptsMod.Find("SldFréquenceObjets").GetComponentInChildren<Slider>();
        SldFréquencePluie = PnlOptsMod.Find("SldFréquencePluie").GetComponentInChildren<Slider>();
        SldFréquenceOrages = PnlOptsMod.Find("SldFréquenceOrages").GetComponentInChildren<Slider>();
        SldNbMaxObjets = PnlOptsMod.Find("SldNbMaxObjets").GetComponentInChildren<Slider>();

        TxtValeurFreqObj = SldFréquenceObjets.transform.Find("TxtValeur").gameObject.GetComponent<Text>();
        TxtValeurFreqPluie = SldFréquencePluie.transform.Find("TxtValeur").gameObject.GetComponent<Text>();
        TxtValeurFreqOrages = SldFréquenceOrages.transform.Find("TxtValeur").gameObject.GetComponent<Text>();
        TxtValeurNbObj = SldNbMaxObjets.transform.Find("TxtValeur").gameObject.GetComponent<Text>();


        SldFréquenceObjets.onValueChanged.AddListener((x) => ModifierFréquenceObjets(x));
        SldFréquencePluie.onValueChanged.AddListener((x) => ModifierFréquencePluie(x));
        SldFréquenceOrages.onValueChanged.AddListener((x) => ModifierFréquenceObjets(x));
        SldNbMaxObjets.onValueChanged.AddListener((x) => ModifierNbObjetsMax((int)x));

        TxtCheminAcces = PnlOptsNonMod.transform.Find("TxtCheminAcces").gameObject.GetComponent<Text>();
    }

    private void ModifierFréquenceObjets(float valeur)
    {
        Opts.FréquenceObjets = valeur;
        TxtValeurFreqObj.text = Math.Round(valeur, 2).ToString();
        ÉcrireFichierOpts(Opts);
    }
    private void ModifierFréquencePluie(float valeur)
    {
        Opts.FréquencePluie = valeur;
        TxtValeurFreqPluie.text = Math.Round(valeur, 2).ToString();
        ÉcrireFichierOpts(Opts);
    }
    private void ModifierFréquenceOrages(float valeur)
    {
        Opts.FréquenceOrage = valeur;
        TxtValeurFreqOrages.text = Math.Round(valeur, 2).ToString();
        ÉcrireFichierOpts(Opts);
    }
    private void ModifierNbObjetsMax(int valeur)
    {
        Opts.NbObjetsMax = valeur;
        TxtValeurNbObj.text = valeur.ToString();
        ÉcrireFichierOpts(Opts);
    }
    private void ÉcrireFichierOpts(Options opts)
    {
        using (StreamWriter streamWriter = File.AppendText(Application.dataPath + "/Resources/Options.txt"))
        {
            //A REVOIR
            streamWriter.WriteLine("FréqObj : " + Environment.NewLine + "{0}" + Environment.NewLine +
                                   "NbMaxObj : " + Environment.NewLine + "{1}" + Environment.NewLine +
                                   "FréqPluie : " + Environment.NewLine + "{2}" + Environment.NewLine +
                                   "FréqOrage : " + Environment.NewLine + "{3}");
        }

    }

    void Awake()
    {
        //TxtCheminAcces.text = "Chemin d'accès du jeu : " + Application.dataPath;
    }

    // Update is called once per frame
    void Update()
    {
        ++compteur;
        if(compteur == 60)
        {
            if(Application.internetReachability == NetworkReachability.NotReachable)
            {
                TogRéseau.isOn = true;
                TxtAdresseIP.text = "Adresse IP : " + NetworkManager.singleton.networkAddress;
            }
            else
            {
                TogRéseau.isOn = false;
                TxtAdresseIP.enabled = false;
            }

        }
        else if (compteur == 120)
        {
            compteur = 0;

            if(VérifierManette())
            {
                TogManette.isOn = true;
                TxtNbManettes.enabled = true;
                TxtNbManettes.text = string.Format("{0} Manette de Xbox  |  {1} Manette de PS4", Xbox_One_Controller, PS4_Controller);
            }
            else
            {
                TogManette.isOn = false;
                TxtNbManettes.enabled = false;
            }

        }
    }

    private bool VérifierManette()  //https://forum.unity.com/threads/detecting-controllers.263483/
    {

        Xbox_One_Controller = 0;
        PS4_Controller = 0;

        string[] names = Input.GetJoystickNames();
        for (int x = 0; x < names.Length; x++)
        {
            print(names[x].Length);
            if (names[x].Length == 19)
            {
                PS4_Controller += 1;
            }
            if (names[x].Length == 33)
            {
                Xbox_One_Controller += 1;
            }
        }
        if (Xbox_One_Controller >= 1 || PS4_Controller >= 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
        
}
