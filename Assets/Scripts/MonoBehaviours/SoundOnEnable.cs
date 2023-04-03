using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundOnEnable : MonoBehaviour
{
    public string soundName = "";
    private void OnEnable()
    {
        if(soundName != "")
            AudioManager.Singleton.PlaySound(soundName);
    }
}
