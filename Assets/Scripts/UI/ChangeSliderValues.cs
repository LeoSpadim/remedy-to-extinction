using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeSliderValues : MonoBehaviour
{
    [SerializeField] private GameObject proximoDiaBtn;
    private Slider slider;

    private IEnumerator ShowMoneyProgress()
    {
        AudioManager.Singleton.PlaySound("sfx_money_progression");

        slider = gameObject.GetComponent<Slider>();
        slider.maxValue = LevelManager.META_DINHEIRO;
        slider.value = LevelManager.Singleton.dinheiroTotal;
        yield return new WaitUntil(() => !proximoDiaBtn.activeSelf);
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        StartCoroutine(ShowMoneyProgress());
    }
}
