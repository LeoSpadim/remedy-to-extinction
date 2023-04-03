using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Singleton;

    public AudioClip[] musics;
    public AudioSource musicEmitter;
    public float fadeSpeed = 1.76f;
    public AudioMixerGroup sfxGroup;
    public Sound[] sounds;
    public List<AudioSource> soundEmitters;
    [HideInInspector] public int musicIndex = 0;

    private List<AudioSource> soundsOnHold = new List<AudioSource>();

    public void ChangeMenuMusic(float volume)
    {
        musicIndex++;
        musicIndex = Mathf.Clamp(musicIndex, 0, musics.Length - 1);
        StartCoroutine(FadeMusicChange(volume, fadeSpeed, musicIndex));
    }

    private IEnumerator FadeMusicChange(float volume, float fadeSpeed, int musicIndex)
    {
        while(musicEmitter.volume != 0)
        {
            musicEmitter.volume -= Time.deltaTime * fadeSpeed;
            musicEmitter.volume = Mathf.Clamp(musicEmitter.volume, 0, 1);
            yield return null;
        }
        musicEmitter.Stop();
        musicEmitter.clip = musics[musicIndex];
        musicEmitter.Play();
        while(musicEmitter.volume != volume)
        {
            musicEmitter.volume += Time.deltaTime * fadeSpeed;
            musicEmitter.volume = Mathf.Clamp(musicEmitter.volume, 0, volume);
            yield return null;
        }
        yield return null;
    }

    public void StopAllSounds()
    {
        foreach (Sound sound in sounds)
        {
            sound.source.Stop();
        }
    }

    public void PauseAllSounds()
    {
        foreach (Sound sound in sounds)
        {
            if(sound.source.isPlaying)
            {
                soundsOnHold.Add(sound.source);
                sound.source.Pause();
            }
        }
    }

    public void ResumeAllSounds()
    {
        foreach (AudioSource source in soundsOnHold)
        {
            source.UnPause();
        }
        soundsOnHold.Clear();
    }

    public void PlaySound(string name)
    {
        Sound _sound = Array.Find(sounds, sound => sound.name == name);
        if(_sound == null)
        {
            //Debug.LogError("Sound " + name + " not found");
            return;
        }
        _sound.source.Play();
    }

    public void PauseSound(string name)
    {
        Sound _sound = Array.Find(sounds, sound => sound.name == name);
        if(_sound == null)
        {
            Debug.LogError("Sound " + name + " not found");
            return;
        }
        _sound.source.Pause();
    }

    public void ResumeSound(string name)
    {
        Sound _sound = Array.Find(sounds, sound => sound.name == name);
        if(_sound == null)
        {
            Debug.LogError("Sound " + name + " not found");
            return;
        }
        _sound.source.UnPause();
    }

    public void StopSound(string name)
    {
        Sound _sound = Array.Find(sounds, sound => sound.name == name);
        if(_sound == null)
        {
            Debug.LogError("Sound " + name + " not found");
            return;
        }
        _sound.source.Stop();
    }

    private void Awake()
    {
        if(Singleton == null)
            Singleton = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        musicEmitter = gameObject.GetComponent<AudioSource>();
        foreach (Sound sound in sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;
            sound.source.pitch = sound.pitch;
            sound.source.loop = sound.loop;
            sound.source.outputAudioMixerGroup = sfxGroup;
            soundEmitters.Add(sound.source);
        }
    }
}
