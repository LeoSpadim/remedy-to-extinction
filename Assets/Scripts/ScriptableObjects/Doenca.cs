using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Nova Doenca", menuName = "Objects/Doenca")]
public class Doenca : ScriptableObject
{
    [SerializeField] private Sintomas sintomas;

    [Range(0 ,2)]
    [SerializeField] private int nivelDoenca;

    public Sintomas getSintomas {get{return sintomas;}}

    public string nome;

    public int getNivelDoenca { get {return nivelDoenca;}}

    public int pagamento {get {
        switch(nivelDoenca) 
        {
            case 0 :
            return 10;
            break;
            case 1:
            return 20;
            break;
            case 2:
            return 30;
            break;
            default:
            return 0;
            break;
        }
    }}

}
