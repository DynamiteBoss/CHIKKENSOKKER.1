using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Options
{

    int nbObjetsMax_ = 3;
    float fréquencePluie_ = 0.5f;
    float fréquenceOrage_ = 0.5f;
    float fréquenceObjets_ = 0.5f;
    float volumeSon_ = 1f;


    public Options()
    {
        NbObjetsMax = nbObjetsMax_;
        FréquencePluie = fréquencePluie_;
        FréquenceOrage = fréquenceOrage_;
        FréquenceObjets = fréquenceObjets_;
        VolumeSon = volumeSon_;
    }
    public Options(int nbObjMax, float freqObj, float freqPluie, float freqOrage, float volumeSon)
    {
        NbObjetsMax = nbObjMax;
        FréquencePluie = freqPluie;
        FréquenceOrage = freqOrage;
        FréquenceObjets = freqObj;
        VolumeSon = volumeSon;
    }
    public float FréquenceOrage
    {
        get
        {
            return fréquenceOrage_;
        }
        set
        {
            if (value >= 0 && value <= 1)
                fréquenceOrage_ = value;
        }
    }

    public float FréquencePluie
    {
        get
        {
            return fréquencePluie_;
        }
        set
        {
            if (value >= 0 && value <= 1)
                fréquencePluie_ = value;

        }
    }

    public float FréquenceObjets
    {
        get
        {
            return fréquenceObjets_;
        }
        set
        {
            if (value > 0 && value <= 1)
                fréquenceObjets_ = value;
        }
    }
    public int NbObjetsMax
    {
        get
        {
            return nbObjetsMax_;
        }
        set
        {
            if (value >= 0 && value <= 10)
                nbObjetsMax_ = value;
        }
    }
    public float VolumeSon
    {
        get
        {
            return volumeSon_;
        }
        set
        {
            if (value >= 0 && value <= 1)
                volumeSon_ = value;
        }
    }
}


