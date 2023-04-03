using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class SpecialDialogue : ScriptableObject
{
    [SerializeField] protected DialogueObject dialogue;
    public SpecialDialogue nextDialogue;

    
    [System.NonSerialized]
    private bool isDialogueClosed = false;

    [System.NonSerialized]
    private List<Coroutine> allRoutines = new List<Coroutine>();
    public event Action onEndedSpecialDialogue;

    private void DoOnEndDialogSelf() 
    {
        DoOnEndDialog();
        onEndedSpecialDialogue?.Invoke();
        isDialogueClosed = true;
        DialogueUI.onStoppedDialog -= DoOnEndDialogSelf; // remove pois esse dialogo (era DoOnEndDialog)
        // acabou
    }

    protected abstract void DoOnEndDialog();

    protected abstract void DoOnStartDialog();

    protected abstract void DoForNextDialog();

    protected abstract void DoOnStart();

    protected void CallNextDialog() 
    {
        StartCoroutineByMaster(NextRoutine());
    }

    protected IEnumerator NextRoutine() 
    {
        yield return new WaitUntil(() => isDialogueClosed);
        DoForNextDialog();
    }

    private void DoDialog() 
    {
        DoOnStartDialog();
        if(dialogue != null) DialogueUI.Singleton.ShowDialogue(dialogue);
        if(nextDialogue != null) CallNextDialog();
    }

    protected void NextDialog() 
    {
        nextDialogue.StartDialog();
    }

    protected Coroutine StartCoroutineByMaster(IEnumerator e) 
    {
        allRoutines.Add(SpecialDialogueMaster.Singleton.StartChildCoroutine(e));
        return allRoutines[allRoutines.Count - 1];
    }

    protected void StopAllRoutines() 
    {
        foreach (var routine in allRoutines)
        {
            if(routine == null) {continue;}
            StopCoroutineByMaster(routine);
        }
        allRoutines.Clear();
    }

    protected void StopCoroutineByMaster(Coroutine c) 
    {
        SpecialDialogueMaster.Singleton.StopChildrenCoroutine(c);
    }

    public void StopDialog() 
    {
    //    DialogueUI.Singleton.CloseDialogueBox();
        StopAllRoutines();
    }

    public void StartDialog() 
    {
        isDialogueClosed = false;
        allRoutines = new List<Coroutine>();
        // Debug.LogError("tamanho rotinas: " + allRoutines.Count);
        DoOnStart();
        DialogueUI.onStoppedDialog += DoOnEndDialogSelf;
        DoDialog();
    }

}
