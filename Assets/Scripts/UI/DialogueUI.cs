using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueUI : MonoBehaviour
{
    public static DialogueUI Singleton
    {
        get
        {
            if(dialogueUI == null)
            {
                dialogueUI = FindObjectOfType<DialogueUI>();
            }
            return dialogueUI;
        }
    }
    private static DialogueUI dialogueUI;
    public GameObject dialogueBox;
    [SerializeField] private TMP_Text txtLbl;
    [SerializeField] private float typeWriterSpeed = 30f;
    private TypeWriterEffect typeWriterEffect;

    public static event Action onStoppedDialog;

    public bool isShowingDialogue{get; private set;}

    public void ShowDialogue(DialogueObject dialogueObject)
    {
        dialogueBox.SetActive(true); //C'mon man i know ur better tahn that just do some animation bro
        StartCoroutine(StepThroughDialogue(dialogueObject, typeWriterSpeed));

        AudioManager.Singleton.PlaySound("sfx_typewriter");

        isShowingDialogue = true;
    }

    public void CloseDialogueBox()
    {
        dialogueBox.SetActive(false); //I dunno man, do a animation or some shit
        txtLbl.text = string.Empty;

        AudioManager.Singleton.StopSound("sfx_typewriter");

        onStoppedDialog?.Invoke();
        isShowingDialogue = false;
    }

    private IEnumerator StepThroughDialogue(DialogueObject dialogueObject, float typeWriterSpeed)
    {
        foreach (string dialogue in dialogueObject.Dialogue)
        {
            yield return RunTypingEffect(dialogue, typeWriterSpeed);
            txtLbl.text = dialogue;
            yield return null;
            AudioManager.Singleton.PauseSound("sfx_typewriter");
            yield return new WaitUntil(() => Input.GetMouseButtonDown(1) && !PauseMenu.PAUSED);
            AudioManager.Singleton.ResumeSound("sfx_typewriter");
        }

        CloseDialogueBox();
    }

    private IEnumerator RunTypingEffect(string dialogue, float typeWriteSpeed)
    {
        typeWriterEffect.Run(dialogue, txtLbl, typeWriterSpeed);

        while (typeWriterEffect.isRunning)
        {
            yield return null;

            if(Input.GetMouseButtonDown(1) && !PauseMenu.PAUSED)
            {
                typeWriterEffect.Stop();
            }
        }
    }

    public void SubscribeFunction(PacienteAnimHandler pacienteAnimHandler)
    {
        pacienteAnimHandler.OnChegouNaSala += ShowDialogue;
    }

    public void UnSubscribeFunction(PacienteAnimHandler pacienteAnimHandler)
    {
        pacienteAnimHandler.OnChegouNaSala -= ShowDialogue;
    }

    private void Awake()
    {
        typeWriterEffect = GetComponent<TypeWriterEffect>();
       // CloseDialogueBox();
    }

    private void OnDisable()
    {
        PacienteAnimHandler[] pacienteEvents = FindObjectsOfType<PacienteAnimHandler>();
        foreach (PacienteAnimHandler pacienteAnimHandler in pacienteEvents)
        {
            pacienteAnimHandler.OnChegouNaSala -= ShowDialogue;
        }
    }
}
