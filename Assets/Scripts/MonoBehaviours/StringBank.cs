using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StringBank : MonoBehaviour
{
    private static StringBank Instance;

    public static StringBank Singleton 
    {
        get
         {
            if(Instance == null) {Instance = FindObjectOfType<StringBank>();}

            return Instance;    
        }
    }
    private List<string> nomesUsados = new List<string>(), descricoesUsadas = 
    new List<string>();

    private  List<Ocupacao> ocupacoesUsadas = new List<Ocupacao>();

    private List<PersonalityDialogue> personalidadesUsadas = new List<PersonalityDialogue>();

    public const int MAXTRIES = 80;

    public bool NomeJaUsado(string nome) 
    {
        if(nomesUsados.Contains(nome)) {return true;}
        nomesUsados.Add(nome);
        return false;
    }

    public bool DescricaoJaUsada(string descricao) 
    {
        if(descricoesUsadas.Contains(descricao)) {return true;}
        descricoesUsadas.Add(descricao);
        return false;
    }

    public bool OcupacaoJaUsada(Ocupacao ocupacao) 
    {
        if(ocupacoesUsadas.Contains(ocupacao)) {return true;}
        ocupacoesUsadas.Add(ocupacao);
        return false;
    }

    public bool PersonalidadeJaUsada(PersonalityDialogue personalidade) 
    {
        if(personalidadesUsadas.Contains(personalidade)) {return true;}
        personalidadesUsadas.Add(personalidade);
        return false;
    }

}
