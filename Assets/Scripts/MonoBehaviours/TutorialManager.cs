using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] TMP_Text pranchetaTxt;

    [SerializeField] Doenca doenca;
    private static TutorialManager Instance;

    public static TutorialManager Singleton {get {
        if(Instance == null) {Instance = FindObjectOfType<TutorialManager>();}

 return Instance;   }}

    public event Action onEsqueceuUmBotao;

    public EventoBool onGerouReceita;

    bool conseguiu = false;

    void Start()
    {
        pranchetaTxt.text = "Sintomas:";
        for (int i = 0; i < doenca.getSintomas.tiposSintomas.Length; i++)
        {
            pranchetaTxt.text += "\n";
            pranchetaTxt.text += doenca.getSintomas.tiposSintomas[i];
            pranchetaTxt.text = pranchetaTxt.text.Replace("_", " ");
        }
    }

    public void LevarParaMain()
    {
        SceneManager.LoadScene(1);
    }

    public void GerarReceita()
    {
        if(conseguiu || DialogueUI.Singleton.isShowingDialogue) {return;}
        if(LevelManager.Singleton.doencaSelecionada == null || LevelManager.Singleton.remedioSelecionado == null
        || DialogueUI.Singleton.isShowingDialogue)
        {
            Debug.LogError("nada selecionado");
            onEsqueceuUmBotao?.Invoke();
            return;
        }

        // if(LevelManager.Singleton.doencaSelecionada == doenca &&
        //  LevelManager.Singleton.remedioSelecionado.getNivelDoenca == LevelManager.
        //  Singleton.doencaSelecionada.getNivelDoenca)
        //  {
            conseguiu = true;
       //  }


        onGerouReceita?.Invoke(conseguiu);
        LevelManager.Singleton.doencaSelecionada = null;
        LevelManager.Singleton.remedioSelecionado = null;
    }
}

[System.Serializable]
public class EventoBool : UnityEvent<bool>
{

}
