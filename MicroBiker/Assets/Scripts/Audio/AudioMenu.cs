using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioMenu : MonoBehaviour
{
    [Header("On/Off")]
    public GameObject musicOnBtn;
    public GameObject musicOffBtn;
    public GameObject soundOnBtn;
    public GameObject soundOffBtn;

    public static MenuSettingsData menuSettings;

    private void Start()
    {
        menuSettings = AudioPersister.LoadAudioSettings();

        UpdateMusicVolume(menuSettings.MusicStatus);
        UpdateSoundVolume(menuSettings.SoundStatus);
        InitAudioMenuButtons();
    }

    public void UpdateMusicVolume(bool on)
    {
        if (GameManager.instance == null)
        {
            AudioManager.instance.mixer.SetFloat("MusicVolume", on && (GameManager.instance == null || !GameManager.instance.gameStarted) ? 0 : -80);//if instance == null means we are not playing(music in level is not allowed)
            if (on != menuSettings.MusicStatus)
            {
                menuSettings.MusicStatus = on;
                UpdateAudioSettings();
            }
        }
    }

    public void UpdateSoundVolume(bool on)
    {
        AudioManager.instance.mixer.SetFloat("SoundVolume", on && (GameManager.instance == null || !GameManager.instance.gameStarted) ? 0 : -80);
        if (on != menuSettings.SoundStatus)
        { 
            menuSettings.SoundStatus = on;
            UpdateAudioSettings();
        }
    }

    void UpdateAudioSettings()
    {
        AudioPersister.UpdateAudioSettings(menuSettings);
    }

    void InitAudioMenuButtons()
    {
        musicOnBtn.SetActive(menuSettings.MusicStatus);
        musicOffBtn.SetActive(!menuSettings.MusicStatus);
        soundOnBtn.SetActive(menuSettings.SoundStatus);
        soundOffBtn.SetActive(!menuSettings.SoundStatus);
    }
}
