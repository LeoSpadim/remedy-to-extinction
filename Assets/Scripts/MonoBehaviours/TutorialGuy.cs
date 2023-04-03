using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class TutorialGuy : MonoBehaviour
{
    [SerializeField] private SpecialDialogue startDialogue;

    [SerializeField] private SpecialDialogueByBool spawnGerarReceitaDialogue;

    [SerializeField] private SpecialDialogue acertouBotaoDialogue, 
    esqueceuUmBotaoDialogue, ultimoDialogo, loopDialogo;

    [SerializeField] Animator jornalAnim, tutoAnim;

    [SerializeField] GameObject blackOut, botao;

    private PacienteAnimHandler handler;
    void StartDialog(DialogueObject fdc) 
    {
        startDialogue.StartDialog();
    }

    private void ActivateGerarReceitaButton() 
    {
        LevelManager.Singleton.b_gerarReceita.gameObject.SetActive(true);
    }

    void AcabaTutorial() 
    {
        blackOut.SetActive(true);
        botao.SetActive(true);
        jornalAnim.gameObject.SetActive(true);
        tutoAnim.Play("paciente_Exit");
    } 

    public void MudaCena() 
    {
        tutoAnim.Play("Jornal_exit");
        SceneManager.LoadScene(1);
    }

    void EsqueceuUmBotao() 
    {
        spawnGerarReceitaDialogue.SetLiberaTrue();
        esqueceuUmBotaoDialogue.StartDialog();
    }

    void ComecaDialogoAcertouBotao(bool conseguiuBotoes) 
    {
        spawnGerarReceitaDialogue.SetLiberaTrue();
      //  if(conseguiuBotoes) 
       // {
           loopDialogo.StopDialog();
           esqueceuUmBotaoDialogue.StopDialog();
            esqueceuUmBotaoDialogue.nextDialogue.StopDialog();
            acertouBotaoDialogue.StartDialog();
        //}
        // else 
        // {
        //     esqueceuUmBotaoDialogue.nextDialogue.StopDialog();
        //     errouBotaoDialogue.StartDialog();
        // }
    }

    void OnEnable() 
    {
        handler = GetComponent<PacienteAnimHandler>();
        handler.OnChegouNaSala += StartDialog;
        if(TutorialManager.Singleton)
         {
             TutorialManager.Singleton.onGerouReceita.AddListener(ComecaDialogoAcertouBotao);
             TutorialManager.Singleton.onEsqueceuUmBotao += EsqueceuUmBotao;
         }
        spawnGerarReceitaDialogue.onEndedSpecialDialogue += ActivateGerarReceitaButton;
        ultimoDialogo.onEndedSpecialDialogue += AcabaTutorial;
    }

    void OnDisable() 
    {
        handler.OnChegouNaSala -= StartDialog;
        if(TutorialManager.Singleton) 
        {
            TutorialManager.Singleton.onGerouReceita.RemoveListener(ComecaDialogoAcertouBotao);
            TutorialManager.Singleton.onEsqueceuUmBotao -= EsqueceuUmBotao;
        }
        spawnGerarReceitaDialogue.onEndedSpecialDialogue -= ActivateGerarReceitaButton;
        ultimoDialogo.onEndedSpecialDialogue += AcabaTutorial;
    }

}
