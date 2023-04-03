using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public Dropdown resolutionsDropdown;
    public GameObject optionsPanel;
    public GameObject[] credits;
    public Image muteMusic, muteSound;
    public Sprite musicOn, musicOff, soundOn, soundOff;
    public Slider musicSlider, soundSlider;
    //public AudioMixer audioMixer;
    public AudioSource musicEmitter {get; private set;}
    [HideInInspector] public List<AudioSource> soundEmitters;
    public MenuSaves save;

    Resolution[] resolutions;
    [SerializeField] private bool isCutScene = false;
    //string musicVolume = "MusicVolume", soundVolume = "SFX_Volume";
    //float unMutedSoundVolume;

    public void StartResolutions()
    {
        if(isCutScene){return;}
        int currentResolutionIndex = 0;
        resolutions = Screen.resolutions;
        List<string> resolutionsList = new List<string>();
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height;
            resolutionsList.Add(option);

            if(resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }
        resolutionsDropdown.ClearOptions();
        resolutionsDropdown.AddOptions(resolutionsList);
        resolutionsDropdown.value = currentResolutionIndex;
        resolutionsDropdown.RefreshShownValue();
    }

    public void OptionsButton()
    {
        AudioManager.Singleton.PlaySound("sfx_btn");

        optionsPanel.SetActive(!optionsPanel.activeSelf);
    }

    public void SetResolution(int resolutionIndex)
    {
        Screen.SetResolution(resolutions[resolutionIndex].width, resolutions[resolutionIndex].height, Screen.fullScreen);
        save.ResolutionIndex = resolutionIndex;
    }

    public void SetFullScreen(bool isOn)
    {
        AudioManager.Singleton.PlaySound("sfx_btn");

        Screen.fullScreen = isOn;
        save.FullScreen = isOn;
    }

    public void SetVSync(bool isOn)
    {
        AudioManager.Singleton.PlaySound("sfx_btn");

        QualitySettings.vSyncCount = Convert.ToInt32(isOn);
        save.Vsync = isOn;
    }

    private void ChangeMuteUI(AudioSource source, Slider slider, Image UIButton, Sprite on, Sprite off)
    {
        if(source.mute || slider.value <= 0)
        {
            UIButton.sprite = off;
        }
        else
        {
            UIButton.sprite = on;
        }
    }

    public void MuteMusic()
    {
        AudioManager.Singleton.PlaySound("sfx_btn");

        musicEmitter.mute = !musicEmitter.mute;
        save.MusicMute = musicEmitter.mute;

        ChangeMuteUI(musicEmitter, musicSlider, muteMusic, musicOn, musicOff);
    }

    public void MusicSlider()
    {
        //audioMixer.SetFloat(musicVolume, musicSlider.value);
        musicEmitter.volume = musicSlider.value;
        save.MusicVolume = musicSlider.value;

        ChangeMuteUI(musicEmitter, musicSlider, muteMusic, musicOn, musicOff);
    }

    public void MuteSound() //Vou ter que mudar esse código
    {
        AudioManager.Singleton.PlaySound("sfx_btn");

        foreach (AudioSource soundEmitter in soundEmitters)
        {
            soundEmitter.mute = !soundEmitter.mute;
        }
        save.SoundMute = soundEmitters[0].mute;

        ChangeMuteUI(soundEmitters[0], soundSlider, muteSound, soundOn, soundOff);
    }

    public void SoundSlider()
    {
        //audioMixer.SetFloat(soundVolume, soundSlider.value);
        foreach (AudioSource soundEmitter in soundEmitters)
        {
            soundEmitter.volume = soundSlider.value;
        }
        save.SoundVolume = soundSlider.value;

        if(soundEmitters != null)
            ChangeMuteUI(soundEmitters[0], soundSlider, muteSound, soundOn, soundOff);
    }

    public void ExitButton()
    {
        AudioManager.Singleton.PlaySound("sfx_btn");

        Application.Quit();
    }

    IEnumerator CreditsManager()
    {
        credits[4].transform.position = gameObject.transform.position;
        foreach (GameObject page in credits)
        {
            yield return new WaitUntil(() => Input.GetMouseButtonUp(0));
            page.transform.position = gameObject.transform.position;
            yield return null;
            //AudioManager.Singleton.PlaySoundEffect(0);
        }
        foreach (GameObject page in credits)
        {
            page.transform.position = gameObject.transform.position + new Vector3(-8 * gameObject.transform.position.x, 0, 0);
        }
    }

    public void CreditsButton()
    {
        AudioManager.Singleton.PlaySound("sfx_btn");

        StartCoroutine("CreditsManager");
    }

    protected void MenuSaveLoader()
    {
        if(isCutScene){return;}
        Screen.fullScreen = save.FullScreen;
        Screen.SetResolution(resolutions[save.ResolutionIndex].width, resolutions[save.ResolutionIndex].height, Screen.fullScreen);
        musicEmitter.mute = save.MusicMute;
        foreach (AudioSource soundEmitter in soundEmitters)
        {
            soundEmitter.mute = save.SoundMute;
        }
        musicSlider.value = save.MusicVolume;
        MusicSlider();
        soundSlider.value = save.SoundVolume;
        SoundSlider();
        QualitySettings.vSyncCount = Convert.ToInt32(save.Vsync);
    }

    public void StartGame()
    {
        AudioManager.Singleton.PlaySound("sfx_btn");
        AudioManager.Singleton.ChangeMenuMusic(save.MusicVolume);

        SceneManager.LoadScene(4);
    }

    private void Start()
    {
        StartResolutions();
        foreach (GameObject page in credits)
        {
            page.transform.position = gameObject.transform.position + new Vector3(-8 * gameObject.transform.position.x, 0, 0);
        }
        musicEmitter = AudioManager.Singleton.musicEmitter;
        soundEmitters = AudioManager.Singleton.soundEmitters;
        MenuSaveLoader();
    }
}