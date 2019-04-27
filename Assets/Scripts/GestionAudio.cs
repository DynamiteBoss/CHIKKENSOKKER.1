using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GestionAudio : MonoBehaviour
{
    bool sonActivé;
    const string CheminAccesPartielOpts = "Assets/Resources/Options/Options.txt";
    // Start is called before the first frame update
    void Start()
    {
        /*if (Parent)
            Son = Parent.GetComponentInChildren<AudioSource>();
        else
            Son = this.transform.GetComponentInChildren<AudioSource>();*/
    }

    public void FaireJouerSon(AudioSource Son)
    {
        Son.volume = LireVolumeSon();
        Son.Play();
    }
    public void FaireJouerMusique(AudioSource Son)
    {
        Son.volume = LireVolumeSon();
        Son.loop = true;
        Son.Play();
    }
    public void ArrêterSon(AudioSource Son)
    {
        Son.Stop();
    }

    // Update is called once per frame
    void Update()
    {

    }
    float LireVolumeSon()
    {
        float valeurÀRetourner;
        using (StreamReader streamReader = new StreamReader(CheminAccesPartielOpts))
        {
            for (int i = 0; i < 9; i++)
                streamReader.ReadLine();
            float.TryParse(streamReader.ReadLine().ToString(), out valeurÀRetourner);
            streamReader.Close();
        }
        return valeurÀRetourner;
    }
}
