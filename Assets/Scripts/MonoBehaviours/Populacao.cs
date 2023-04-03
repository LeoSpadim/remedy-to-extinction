using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Populacao
{
    const int POPULACAOMAX = 150000;

    public int populacaoInfectada {get; private set;}

    public readonly int populacaoMinimaAEstarInfectada, populacaoMaximaAEstarInfectada;

    public Populacao(float porcentagemInicialinfectados, float minInfectedPercent, float maxInfectedPercent) 
    {
        populacaoInfectada = (int)((POPULACAOMAX * porcentagemInicialinfectados) / 100);
        populacaoMinimaAEstarInfectada = PorcentagemPop(minInfectedPercent);
        populacaoMaximaAEstarInfectada = PorcentagemPop(maxInfectedPercent);
    }

    public int NumeroTotalPacientes() 
    {
        return LevelManager.Singleton.pacientesPrefabs.Length;
    }

    public void AddDoentes(float novosDoentesPercent) 
    {
        populacaoInfectada += PorcentagemPop(novosDoentesPercent);
    }

    public bool IsAbove() 
    {
        if(populacaoInfectada > populacaoMaximaAEstarInfectada) {return true;}
        return false;
    }

    public bool IsBelow() 
    {
        if(populacaoInfectada < populacaoMinimaAEstarInfectada) {return true;}
        return false;
    }

    public void SubtractDoentes(float doentesRemovidosPercent) 
    {
        populacaoInfectada -= PorcentagemPop(doentesRemovidosPercent);
    }

    public float GetPopulacaoInfectadaPercent() 
    {
        return (100 * populacaoInfectada) / POPULACAOMAX;
    }

    public int PorcentagemInfecs(float percent) 
    {
        return (int)((populacaoInfectada * percent) / 100);
    }

    public static int PorcentagemPop(float percent) 
    {
        return (int)((POPULACAOMAX * percent) / 100);
    }
}
