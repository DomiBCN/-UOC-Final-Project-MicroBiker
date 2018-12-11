using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{

    public static AudioManager instance;

    public AudioMixer mixer;

    public Sound[] sounds;

    float musicVolume;
    float musicVolumeCounter;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.loop = s.loop;

            s.source.outputAudioMixerGroup = s.mixerGroup;
        }
    }

    public void Start()
    {
        Play("Music");
    }

    public void Play(string sound)
    {
        Sound s = Array.Find(sounds, item => item.name == sound);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }

        s.source.volume = s.volume;
        s.source.pitch = s.pitch;

        s.source.Play();
    }

    public void Stop(string sound)
    {
        Sound s = Array.Find(sounds, item => item.name == sound);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source.Stop();
    }

    public void UpdatePitch(string sound, float pitch)
    {
        Sound s = Array.Find(sounds, item => item.name == sound);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }

        s.source.pitch = pitch;
    }

    public void AudioGroupMuteControl(string group, bool mute)
    {
        mixer.SetFloat(group, mute ? -80 : 0);
    }

    public void DecreaseGroupAudio(string group)
    {
        StartCoroutine(DecreaseAudio(group));
    }

    IEnumerator DecreaseAudio(string group)
    {
        mixer.GetFloat(group, out musicVolume);
        while (musicVolume != -80)
        {
            mixer.GetFloat(group, out musicVolume);

            musicVolumeCounter = musicVolume - 0.5f;
            musicVolumeCounter = musicVolumeCounter < -80 ? -80 : musicVolumeCounter;

            mixer.SetFloat(group, musicVolumeCounter);
            yield return null;
        }
    }
}
