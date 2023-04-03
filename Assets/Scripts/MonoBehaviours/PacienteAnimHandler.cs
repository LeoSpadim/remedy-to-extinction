using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class PacienteAnimHandler : MonoBehaviour
{
    private Animator anim;
    public static event Action onLeftRoom, onStartedLeavingRoom;
    public Action<DialogueObject> OnChegouNaSala;

    public static event Action onChegouSalaNormal;

    public DialogueObject dialogo {get; private set;}

    private void OnEnable()
    {
        if(LevelManager.Singleton.isTutorial) {return;}
        DialogueUI.Singleton.SubscribeFunction(this);
    }

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnDisable()
    {
        if(LevelManager.Singleton && LevelManager.Singleton.isTutorial) {return;}
        if(DialogueUI.Singleton)
        DialogueUI.Singleton.UnSubscribeFunction(this);
    }

    public void PlayEnterPaciente()
    {
        anim.Play("paciente_Enter");
    }

    public void DoOnChegouSala()
    {
        /*Paciente paciente = gameObject.GetComponentInChildren<Paciente>();
        paciente.RandomPhrase();
        OnChegouNaSala?.Invoke(Paciente.randomPhrase);*/
        if(gameObject.GetComponentInChildren<Paciente>() != null) 
        {
            Paciente paciente = gameObject.GetComponentInChildren<Paciente>();
            dialogo = paciente.RandomPhrase();
        }
        onChegouSalaNormal?.Invoke();
        OnChegouNaSala?.Invoke(dialogo);
    }

    public void PlayExitPaciente()
    {
        onStartedLeavingRoom?.Invoke();
        anim.Play("paciente_Exit");
    }

    public void DoOnLeftRoom()
    {
       // if(gameObject.GetComponentInChildren<Paciente>() == null) {return;}
        onLeftRoom?.Invoke();
    }


    public void SetDialogo(DialogueObject d) 
    {
        dialogo = d;
    }

}
