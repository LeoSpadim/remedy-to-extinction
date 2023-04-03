using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Fiscal : MonoBehaviour
{
    public DialogueObject dialogoSuprimentos, dialogoEventoPopulacao;
    private PacienteAnimHandler handler;

    public static event Action onSaiuFiscal;

    void Awake()
    {
        handler = GetComponent<PacienteAnimHandler>();
    }

    void OnEnable() // toda vez que for ativado vai trocar o dialogo para
    { // o de suprimentos (inicial) e inscrever a funcao para sempre que terminarem os dialogos
        DialogueUI.onStoppedDialog += FazerAoTerminarDialogo;
        PacienteAnimHandler.onLeftRoom += DestruirFiscal;
        handler.SetDialogo(dialogoSuprimentos);
    }

    void OnDisable()
    {
        DialogueUI.onStoppedDialog -= FazerAoTerminarDialogo;
        PacienteAnimHandler.onLeftRoom -= DestruirFiscal;
    }

    private void DestruirFiscal()
    {
        Destroy(gameObject);
    }

    private void FazerAoTerminarDialogo()
    {
            Reestoca();
        // dar suprimentos caso o dialogo ainda seja o de suprimentos
            if(LevelManager.Singleton.IsPovoInsatisfeito() && !LevelManager.Singleton.isOnRevoltaEvent)
            { // se o povo estiver insatisfeito
                // pra nao entrar aqui de novo no fim do dialogo
                DialogueUI.Singleton.ShowDialogue(dialogoEventoPopulacao);
                // mostra o dialogo de evento
                Debug.LogError("EVENTO REVOLTA COMECOU!");
                LevelManager.Singleton.StartRevoltaEvent();
                // avisa que iniciou o evento
                return;
            }
            else if(!LevelManager.Singleton.IsPovoInsatisfeito() && LevelManager.Singleton.isOnRevoltaEvent)
            { // caso nao estiver insatisfeito, so desativa o evento por booleana
                LevelManager.Singleton.EndRevoltaEvent();
                // so desativa o evento
            }
            SaiFiscal();
    }

    private void Reestoca()
    {
        RemedioButtons[] r = LevelManager.Singleton.remedioButtons;
        foreach (var re in r)
        {
            if(re.remedio.getNivelDoenca > LevelManager.Singleton.nivelDoencas) {continue;}
            if(re.remedio.isTipoEspecial)
            {
                if(re.GetAmountRemedio() <= 0)
                {
                    re.AumentaRemedio(3);
                }
                continue;
            }
            if(re.remedio.getNivelDoenca == LevelManager.Singleton.nivelRodizio
            || re.GetAmountRemedio() <= 0)
            {
                re.AumentaRemedio((6 + LevelManager.Singleton.dia) - (re.remedio.getNivelDoenca * 2));
            }
        }
    }

    private void SaiFiscal()
    {
        handler.PlayExitPaciente(); // o fiscal vaza
        onSaiuFiscal?.Invoke();
    }

}
