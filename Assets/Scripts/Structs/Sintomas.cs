using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Sintomas
{
    const int dorDeCabeca = 1;
    const int tosse = 1 << 1;
    const int nausea = 1 << 2;
    const int febre = 1 << 3;
    const int coriza = 1 << 4;
    const int fadiga = 1 << 5;
    const int hematomas = 1 << 6;
    const int desmaio = 1 << 7;

    public enum SINTOMASTYPE {Dor_de_cabeça, Tosse, Nausea, Febre, Coriza, Fadiga, Hematomas, 
    Desmaio}

    public SINTOMASTYPE[] tiposSintomas;

    private int res;

    public int resultado 
    {
        get 
    {
        if(res != 0) {return res;}
        int r = 0;

        foreach (var sintoma in tiposSintomas)
        {
            switch(sintoma) 
            {
                case SINTOMASTYPE.Dor_de_cabeça:
                    r |= dorDeCabeca;
                break;
                case SINTOMASTYPE.Tosse:
                    r |= tosse;
                break;
                case SINTOMASTYPE.Nausea:
                    r |= nausea;
                break;
                case SINTOMASTYPE.Febre:
                    r |= febre;
                break;
                case SINTOMASTYPE.Coriza:
                    r |= coriza;
                break;
                case SINTOMASTYPE.Fadiga:
                    r |= fadiga;
                break;
                case SINTOMASTYPE.Hematomas:
                    r |= hematomas;
                break;
                case SINTOMASTYPE.Desmaio:
                    r |= desmaio;
                break;
            }
        }
        res = r;
        return res;
    }
    }
}
