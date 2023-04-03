using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialDialogueMaster : MonoBehaviour
{
    private static SpecialDialogueMaster Instance;

    public static SpecialDialogueMaster Singleton {get {
        if(Instance == null) {Instance = FindObjectOfType<SpecialDialogueMaster>();}

return Instance;    }}

    public Coroutine StartChildCoroutine(IEnumerator coroutineMethod)
    {
        return StartCoroutine(coroutineMethod);
    }

    public void StopChildrenCoroutine(Coroutine routine) 
    {
        StopCoroutine(routine);
    }
}
