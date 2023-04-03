using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotaoComecaDia : MonoBehaviour
{
    [SerializeField] private GameObject botaoMenu;

    void Update()
    {
        if(LevelManager.Singleton.perdeu) 
        {
            botaoMenu.SetActive(true);
            gameObject.SetActive(false);
        }
    }

}
