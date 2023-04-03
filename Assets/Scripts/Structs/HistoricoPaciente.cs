using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct HistoricoPaciente
{
    public List<string> doencas {get; private set;}
    public List<string> remedios {get; private set;}
    public List<bool> efetividade {get; private set;}

    public void AddHistorico(string doencas, string remedios, bool efetividade)
    {
        if(this.doencas == null) 
        {
            this.doencas = new List<string>();
            this.remedios = new List<string>();
            this.efetividade = new List<bool>();
        }
        this.doencas.Add(doencas);
        this.remedios.Add(remedios);
        this.efetividade.Add(efetividade);
    }
}