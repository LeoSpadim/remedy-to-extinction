using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue/Special/Activate By Bool Dialogue")]
public class SpecialDialogueByBool : SpecialDialogue
{
    [System.NonSerialized]
    private bool libera = false;

    [SerializeField] private float extraTime;

    [SerializeField] private float timeForAlternative;

    [SerializeField] private SpecialDialogue alternativeDialog;

    [System.NonSerialized]
    private Coroutine alternativeRoutine;
    protected override void DoOnStart() 
    {
        alternativeRoutine = null;
        libera = false;
    }

    protected override void DoOnEndDialog() 
    {

    }
    protected override void DoOnStartDialog() 
    {

    }
    protected override void DoForNextDialog() 
    {
        StartCoroutineByMaster(DoForNextRoutine());
        if(alternativeDialog != null) {alternativeRoutine = StartCoroutineByMaster(WaitForAlternative());}
    }

    private IEnumerator WaitForAlternative() 
    {
        yield return new WaitForSeconds(timeForAlternative);
        alternativeDialog.StartDialog();
    }

    private IEnumerator DoForNextRoutine() 
    {
        yield return new WaitUntil(() => libera);
        Debug.LogError("passou 1");

        if(alternativeRoutine != null) {StopCoroutineByMaster(alternativeRoutine);}

        if(alternativeDialog != null) {alternativeDialog.StopDialog();}

        yield return new WaitForSeconds(extraTime);
        Debug.LogError("passou 2");
        NextDialog();
    }

    public void SetLiberaTrue() 
    {
        libera = true;
    }
}
