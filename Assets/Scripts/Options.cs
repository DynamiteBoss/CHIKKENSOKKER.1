using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Options
{

    int nbObjetsMax_ = 3;
    float fréquencePluie_ = 0.5f;
    float fréquenceOrage_ = 0.5f;
    float fréquenceObjets_ = 0.5f;
    //EN RAJOUTER D'AUTRES PLOX
    //[ex : Mod Contrôles-->    if(Input.GetKeyDown() == Options.ToucheTirer)    ]

    public Options()
    {
        NbObjetsMax = nbObjetsMax_;
        FréquencePluie = fréquencePluie_;
        FréquenceOrage = fréquenceOrage_;
        FréquenceObjets = fréquenceObjets_;

    }
    public Options(int nbObjMax, float freqObj, float freqPluie, float freqOrage)
    {
        NbObjetsMax = nbObjMax;
        FréquencePluie = freqPluie;
        FréquenceOrage = freqOrage;
        FréquenceObjets = freqObj;

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
            if (value >= 0 && value <= 1)
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

}

//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class Options
//{

//    int nbObjetsMax_ = 3;
//    float fréquencePluie_ = 0.5f;
//    float fréquenceOrage_ = 0.5f;
//    float fréquenceObjets_ = 0.5f;
//    //EN RAJOUTER D'AUTRES PLOX
//    //[ex : Mod Contrôles-->    if(Input.GetKeyDown() == Options.ToucheTirer)    ]

//    public Options()
//    {
//        NbObjetsMax = nbObjetsMax_;
//        FréquencePluie = fréquencePluie_;
//        FréquenceOrage = fréquenceOrage_;
//        FréquenceObjets = fréquenceObjets_;

//    }
//    public static float FréquenceOrage
//    {
//        get
//        {
//            return FréquenceOrage;
//        }
//        set
//        {
//            if (value >= 0 && value <= 1)
//                FréquenceOrage = value;
//        }
//    }

//    public static float FréquencePluie
//    {
//        get
//        {
//            return FréquencePluie;
//        }
//        set
//        {
//            if (value >= 0 && value <= 1)
//                FréquencePluie = value;

//        }
//    }

//    public static float FréquenceObjets
//    {
//        get
//        {
//            return FréquenceObjets;
//        }
//        set
//        {
//            if (value >= 0 && value <= 1)
//                FréquenceObjets = value;
//        }
//    }
//    public static int NbObjetsMax
//    {
//        get
//        {
//            return NbObjetsMax;
//        }
//        set
//        {
//            if (value >= 0 && value <= 10)
//                NbObjetsMax = value;
//        }
//    }

//}
