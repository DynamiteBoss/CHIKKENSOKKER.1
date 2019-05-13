using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScriptGestionOpts : MonoBehaviour
{
    string CheminAccesPartielOpts = /*Application.dataPath.ToString() + */"Assets/Resources/Options/Options";

    int compteur = 25;

    int Xbox_One_Controller = 0;
    int PS4_Controller = 0;

    bool aÉtéModifié = false;
    bool estConnectéInternet = false;
    bool manetteConnectée = false;

    //string cheminAcces =   + "/Resources/Options.txt"; //https://stackoverflow.com/questions/50716171/unity-read-text-file-from-resources-folder

    Options Opts { get; set; }


    Transform PnlOptsNonMod { get; set; }
    Transform PnlOptsMod { get; set; }


    Toggle TogRéseau { get; set; }
    Toggle TogManette { get; set; }

    Button BtnAnnuler { get; set; }
    Button BtnAppliquer { get; set; }

    Slider SldFréquenceObjets { get; set; }
    Slider SldFréquencePluie { get; set; }
    Slider SldFréquenceOrages { get; set; }
    Slider SldNbMaxObjets { get; set; }
    Slider SldVolumeSon { get; set; }

    Text TxtNbManettes { get; set; }
    Text TxtAdresseIP { get; set; }
    Text TxtCheminAcces { get; set; }

    Text TxtValeurNbObj { get; set; }
    Text TxtValeurFreqPluie { get; set; }
    Text TxtValeurFreqOrages { get; set; }
    Text TxtValeurFreqObj { get; set; }
    Text TxtValeurVolumeSon { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        InitialiserRéférences();

        //Applique les listeners
        SldFréquenceObjets.onValueChanged.AddListener((x) => ModifierFréquenceObjets(x));
        SldFréquencePluie.onValueChanged.AddListener((x) => ModifierFréquencePluie(x));
        SldFréquenceOrages.onValueChanged.AddListener((x) => ModifierFréquenceOrages(x));
        SldNbMaxObjets.onValueChanged.AddListener((x) => ModifierNbObjetsMax((int)x));
        SldVolumeSon.onValueChanged.AddListener((x) => ModifierVolumeSon(x));
    
        BtnAppliquer.onClick.AddListener(() => Appliquer());
        BtnAnnuler.onClick.AddListener(() => Annuler());
    }

    private void Awake()
    {
        //Assigne le string du chemin d'accès du jeu au champ text du texte de chemin d'accès
        TxtCheminAcces.text = Application.dataPath;
    }

    //Initialise les références
    private void InitialiserRéférences()
    {
        Opts = new Options();
        PnlOptsNonMod = this.transform.Find("PnlOptsNonMod");
        PnlOptsMod = this.transform.Find("PnlOptsMod");

        TogManette = PnlOptsNonMod.Find("TogManette").gameObject.GetComponentInChildren<Toggle>();
        TogRéseau = PnlOptsNonMod.Find("TogRéseau").gameObject.GetComponentInChildren<Toggle>();

        BtnAnnuler = this.transform.Find("BtnAnnuler").GetComponent<Button>();
        BtnAppliquer = this.transform.Find("BtnAppliquer").GetComponent<Button>();

        TxtNbManettes = PnlOptsNonMod.Find("TxtNbManettes").gameObject.GetComponentInChildren<Text>();
        TxtAdresseIP = PnlOptsNonMod.Find("TxtAdresseIP").gameObject.GetComponentInChildren<Text>();

        SldFréquenceObjets = PnlOptsMod.Find("SldFréquenceObjets").GetComponentInChildren<Slider>();
        SldFréquencePluie = PnlOptsMod.Find("SldFréquencePluie").GetComponentInChildren<Slider>();
        SldFréquenceOrages = PnlOptsMod.Find("SldFréquenceOrages").GetComponentInChildren<Slider>();
        SldNbMaxObjets = PnlOptsMod.Find("SldNbMaxObjets").GetComponentInChildren<Slider>();
        SldVolumeSon = PnlOptsMod.Find("SldVolumeSon").GetComponentInChildren<Slider>();

        TxtValeurFreqObj = SldFréquenceObjets.transform.Find("TxtValeur").gameObject.GetComponent<Text>();
        TxtValeurFreqPluie = SldFréquencePluie.transform.Find("TxtValeur").gameObject.GetComponent<Text>();
        TxtValeurFreqOrages = SldFréquenceOrages.transform.Find("TxtValeur").gameObject.GetComponent<Text>();
        TxtValeurNbObj = SldNbMaxObjets.transform.Find("TxtValeur").gameObject.GetComponent<Text>();
        TxtValeurVolumeSon = SldVolumeSon.transform.Find("TxtValeur").gameObject.GetComponent<Text>();

        TxtCheminAcces = PnlOptsNonMod.transform.Find("TxtCheminAcces").gameObject.GetComponent<Text>();
    }

    //Modifie les propriétés de l'instance de la classe Option "Opts" locale
    private void ModifierFréquenceObjets(float valeur)
    {
        Opts.FréquenceObjets = valeur;
        TxtValeurFreqObj.text = (Math.Round(valeur, 3) * 100).ToString();
        ÉcrireFichierOpts(Opts);
    }
    private void ModifierFréquencePluie(float valeur)
    {
        Opts.FréquencePluie = valeur;
        TxtValeurFreqPluie.text = (Math.Round(valeur, 3) * 100).ToString();
        ÉcrireFichierOpts(Opts);
    }
    private void ModifierFréquenceOrages(float valeur)
    {
        Opts.FréquenceOrage = valeur;
        TxtValeurFreqOrages.text = (Math.Round(valeur, 3) * 100).ToString();
        ÉcrireFichierOpts(Opts);
    }
    private void ModifierNbObjetsMax(int valeur)
    {
        Opts.NbObjetsMax = valeur;
        TxtValeurNbObj.text = valeur.ToString();
        ÉcrireFichierOpts(Opts);
    }
    private void ModifierVolumeSon(float valeur)
    {
        Opts.VolumeSon = valeur;
        TxtValeurVolumeSon.text = (Math.Round(valeur, 3) * 100).ToString();
        ÉcrireFichierOpts(Opts);
    }


    //Écrit les Options dans le fichier temporaire qui servira à replacer le fichier d'options actuel
    private void ÉcrireFichierOpts(Options opts)
    {
        aÉtéModifié = true;
        using (StreamWriter streamWriter = new StreamWriter(CheminAccesPartielOpts + "Temporaires.txt", false))
        {
            streamWriter.WriteLine("FréqObj : " + Environment.NewLine + "{0}" + Environment.NewLine +
                                   "NbMaxObj : " + Environment.NewLine + "{1}" + Environment.NewLine +
                                   "FréqPluie : " + Environment.NewLine + "{2}" + Environment.NewLine +
                                   "FréqOrage : " + Environment.NewLine + "{3}" + Environment.NewLine +
                                   "VolumeSon : " + Environment.NewLine + "{4}",
                                   opts.FréquenceObjets, opts.NbObjetsMax, opts.FréquencePluie, opts.FréquenceOrage, opts.VolumeSon);
        }
    }


    void Update()
    {
        ++compteur;
        if(compteur == 30)
        {
            //Assigne la valeur booléenne du test de connexion internet afin de savoir si le système affiche le texte ou non
            TxtAdresseIP.transform.Find("TxtAdresseIPTitre").GetComponent<Text>().enabled = AUneConnexionInternet();

            //Vérifie si une valeur a été modifiée et si oui, il changera l'intéractabilité du bouton seulement s'il ne l'a pas déjà fait
            if(aÉtéModifié && !BtnAppliquer.IsInteractable())
                BtnAppliquer.interactable = true;
        }
        else if (compteur == 60)
        {
            compteur = 0;

            //Vérifie les manettes et change l'affichage du texte et du Toggle («checkbox») pour les manettes
            VérifierManette();
            TogManette.isOn = manetteConnectée;
            TxtNbManettes.enabled = manetteConnectée;

            TxtNbManettes.text = manetteConnectée ? string.Format("{0} Manette de Xbox  |  {1} Manette de PS4", Xbox_One_Controller, PS4_Controller) : string.Empty;

        }
    }

    //Vérifie la présence d'une connexion à internet
    private bool AUneConnexionInternet()
    {
        estConnectéInternet = Application.internetReachability != NetworkReachability.NotReachable;

        TogRéseau.isOn = estConnectéInternet;
        TxtAdresseIP.enabled = estConnectéInternet;
        return estConnectéInternet;
    }

    //Vérifie la connexion d'une manette (Modifié par Alexis GL)
    private void VérifierManette()  //https://forum.unity.com/threads/detecting-controllers.263483/
    {

        Xbox_One_Controller = 0;
        PS4_Controller = 0;

        //Vérifie s'il y a présence de manettes avec les noms des joysticks en fonction de leur longueur (Chaque manette a sa longueur)
        string[] noms = Input.GetJoystickNames();
        for (int x = 0; x < noms.Length; x++)
        {
         
            if (noms[x].Length == 19)
            {
                PS4_Controller += 1;
            }
            if (noms[x].Length == 33)
            {
                Xbox_One_Controller += 1;
            }
        }

        manetteConnectée = (Xbox_One_Controller >= 1 || PS4_Controller >= 1);
    }

    //Annule les changements --> Détruit le fichier temporaire
    public void Annuler()
    {
        File.Delete(CheminAccesPartielOpts + "Temporaires.txt");
        SceneManager.UnloadSceneAsync("SceneOptionMenu");
    }
    //Conserve les changements --> Écrase le fichier d'options avec le fichier temporaire pour le détruire par la suite
    public void Appliquer()
    {
        File.Replace(CheminAccesPartielOpts + "Temporaires.txt",   CheminAccesPartielOpts + ".txt",   CheminAccesPartielOpts + "BKP.txt", true);
        File.Delete(CheminAccesPartielOpts + "Temporaires.txt");
        SceneManager.UnloadSceneAsync("SceneOptionMenu");
    }
}
