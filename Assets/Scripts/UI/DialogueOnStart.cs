using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueOnStart : MonoBehaviour
{
    [SerializeField] private DialogueObject dialogo;
    void Start() 
    {
        DialogueUI.Singleton.ShowDialogue(dialogo);
    }
}
