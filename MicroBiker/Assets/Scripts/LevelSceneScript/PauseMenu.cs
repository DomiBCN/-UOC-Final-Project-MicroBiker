using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    float musicOriginalValue;

    private void Awake()
    {
        AudioManager.instance.mixer.GetFloat("MusicVolume", out musicOriginalValue);
    }

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PasueGame()
    {
        Time.timeScale = 0;
        AudioManager.instance.AudioGroupMuteControl("SoundVolume", true);
    }

    public void Resume()
    {
        if (GameManager.instance.gameStarted)
        {
            Time.timeScale = 1;
            AudioManager.instance.AudioGroupMuteControl("SoundVolume", !AudioMenu.menuSettings.SoundStatus);
        }
        else
        {
            AudioManager.instance.AudioGroupMuteControl("MusicVolume", true);
            GameManager.instance.StartGame();
        }
    }

    public void Restart()
    {
        AudioManager.instance.Stop("MotorbikeRun");
        GameManager.instance.RestartLevel();
    }

    public void BackToLevelsScene()
    {
        Time.timeScale = 1;
        GameManager.instance = null;
        AudioManager.instance.Stop("MotorbikeRun");
        AudioManager.instance.mixer.SetFloat("MusicVolume", musicOriginalValue);
        SceneManager.LoadScene("LevelsScene");
    }

    public void BackToStartScene()
    {
        Time.timeScale = 1;
        AudioManager.instance.Stop("MotorbikeRun");
        AudioManager.instance.mixer.SetFloat("MusicVolume", musicOriginalValue);
        SceneManager.LoadScene("StartScene");
    }

    public void DecreaseMusicVolume()
    {
        AudioManager.instance.DecreaseGroupAudio("MusicVolume");
    }
}
