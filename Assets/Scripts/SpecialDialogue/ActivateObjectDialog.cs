using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue/Special/Activate Object Dialogue")]
public class ActivateObjectDialog : SpecialDialogue
{
    [SerializeField] private float extraTime;

    [SerializeField] private float timeForAlternative;

    [SerializeField] string parent_objectToActivate_s;
    [SerializeField] string parent_objectToWaitFor_s;
    [SerializeField] string objectToWaitFor_s;
    [SerializeField] string objectToActivate_s;

    [SerializeField] private SpecialDialogue alternativeDialog;
    
    [System.NonSerialized]
    private Coroutine alternativeRoutine;
    protected override void DoOnStart() 
    {
        alternativeRoutine = null;
    }

    protected override void DoOnEndDialog() 
    {

    }
    protected override void DoOnStartDialog() 
    {
        if(string.IsNullOrEmpty(objectToActivate_s)) {return;}
        GameObject objectToActivate = GameObject.Find(parent_objectToActivate_s).
        transform.Find(objectToActivate_s).gameObject;

        objectToActivate.SetActive(true);
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
        GameObject objectToWaitFor = GameObject.Find(parent_objectToWaitFor_s).
        transform.Find(objectToWaitFor_s).gameObject;

        yield return new WaitUntil(() => objectToWaitFor.activeInHierarchy);

        if(alternativeRoutine != null) {StopCoroutineByMaster(alternativeRoutine);}

        if(alternativeDialog != null) {alternativeDialog.StopDialog();}

        yield return new WaitForSeconds(extraTime);
        NextDialog();
    }

}
