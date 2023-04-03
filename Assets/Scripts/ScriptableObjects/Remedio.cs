using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Novo Remedio", menuName = "Objects/Remedio")]
public class Remedio : ScriptableObject
{
    [Range(0 ,2)]
    [SerializeField] private int nivelDoenca;

    public enum REMEDIOTYPE {PLACEBO, CURA, MUTAGENICO}

    [SerializeField]
    private REMEDIOTYPE tipoRemedio;

    public string nome;

    public REMEDIOTYPE getTipoRemedio {get {return tipoRemedio;}}

    public int getNivelDoenca { get {return nivelDoenca;}}

    public bool isTipoEspecial {get 
    {
        return tipoRemedio == REMEDIOTYPE.PLACEBO || tipoRemedio == REMEDIOTYPE.MUTAGENICO;
    }}
}
