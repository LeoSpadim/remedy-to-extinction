using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct BioPaciente
{
    public Ocupacao ocupacao {get; private set;}
    public string nome {get; private set;}
    public string descricao {get; private set;}
    public PersonalityDialogue personality {get; private set;}

    public Ocupacoes poolOcupacoes;
    public StringsInfo poolNomesPacientes;
    public StringsInfo poolDescricaoPacientes;
    public PersonalityPool poolPersonalidadePaciente;

    private int randomNumber(int length, int extraSeed = 0)
    {
        Random.InitState(System.DateTime.Now.Millisecond + extraSeed);
        int number = Random.Range(0, length);
        return number;
    }

    public void GerarBio()
    {
        Debug.LogError("Gerando bio! ");
        GerarOcupacao();
        GerarNome();
        GerarDescricao();
        GerarPersonalidade();
    }

    private void GerarOcupacao() 
    {
        this.ocupacao = poolOcupacoes.ocupacoes[randomNumber(poolOcupacoes.ocupacoes.Length)];
        int i = 0;
        while(StringBank.Singleton.OcupacaoJaUsada(ocupacao) && i < StringBank.MAXTRIES) 
        {
            this.ocupacao = poolOcupacoes.ocupacoes[randomNumber(poolOcupacoes.ocupacoes.Length, i)];
            i++;
        }
    }

    private void GerarNome() 
    {
        this.nome = poolNomesPacientes.infos[randomNumber(poolNomesPacientes.infos.Length)];
        int i = 0;
        while(StringBank.Singleton.NomeJaUsado(nome) && i < StringBank.MAXTRIES) 
        {
            this.nome = poolNomesPacientes.infos[randomNumber(poolNomesPacientes.infos.Length, i)];
            i++;
        }
    }

    private void GerarDescricao() 
    {
        this.descricao = poolDescricaoPacientes.infos[randomNumber(poolDescricaoPacientes.infos.Length)];
        int i = 0;
        while(StringBank.Singleton.DescricaoJaUsada(descricao) && i < StringBank.MAXTRIES) 
        {
            this.descricao = poolDescricaoPacientes.infos[randomNumber(poolDescricaoPacientes.infos.Length, i)];
            i++;
        }
    }

    private void GerarPersonalidade() 
    {
        this.personality = poolPersonalidadePaciente.personalities[randomNumber(poolPersonalidadePaciente.personalities.Length)];
        int i = 0;
        while(StringBank.Singleton.PersonalidadeJaUsada(personality) && i < StringBank.MAXTRIES) 
        {
            this.personality = poolPersonalidadePaciente.personalities[randomNumber(poolPersonalidadePaciente.personalities.Length, i)];
            i++;
        }
    }
}