using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    public void Desativa()
    {
        gameObject.SetActive(false);
    }

    public void Ativa()
    {
        gameObject.SetActive(true);
    }

    public void AtivaBotao(ButtonController b)
    {
        b.Ativa();
    }

    public void DesativaBotao(ButtonController b)
    {
        b.Desativa();
    }
}
