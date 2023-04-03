using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OutroController : MonoBehaviour
{
    public DialogueObject dialogue;

    void Start()
    {
        DialogueUI.Singleton.ShowDialogue(dialogue);
    }

    public void LevarParaInicio()
    {
        AudioManager.Singleton.StopAllSounds();

        SceneManager.LoadScene(0);
    }

    void OnEnable()
    {
        DialogueUI.onStoppedDialog += LevarParaInicio;
    }

    void OnDisable()
    {
        DialogueUI.onStoppedDialog -= LevarParaInicio;
    }
}
