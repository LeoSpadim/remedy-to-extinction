using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue/Special/Timer Dialogue")]
public class TimerDialogue : SpecialDialogue
{
    [SerializeField] private float timerForNextDialogue;

    protected override void DoOnStart() 
    {

    }

    protected override void DoOnEndDialog() 
    {

    }
    protected override void DoOnStartDialog() 
    {
        
    }
    protected override void DoForNextDialog() 
    {
        StartCoroutineByMaster(CountDownForNext());
    }

    protected IEnumerator CountDownForNext() 
    {
        yield return new WaitForSeconds(timerForNextDialogue);
        NextDialog();
    }

}
