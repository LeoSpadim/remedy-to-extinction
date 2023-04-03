using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroController : MonoBehaviour
{
    public DialogueObject dialogue;

    void Start()
    {
        DialogueUI.Singleton.ShowDialogue(dialogue);
    }

    public void LevarParaTutorial()
    {
        AudioManager.Singleton.StopAllSounds();

        SceneManager.LoadScene(3);
    }

    void OnEnable()
    {
        DialogueUI.onStoppedDialog += LevarParaTutorial;
    }

    void OnDisable()
    {
        DialogueUI.onStoppedDialog -= LevarParaTutorial;
    }

}
