using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonMaster : MonoBehaviour
{
        public ButtonController[] buttons;
    public void DesativaBotoes() 
    {
        if(buttons == null) {return;}

        foreach (var obj in buttons)
        {
            obj.Desativa();
        }
    }

    public void AtivaBotoes() 
    {
        if(buttons == null) {return;}

        foreach (var obj in buttons)
        {
            obj.Ativa();
        }
    }
}
