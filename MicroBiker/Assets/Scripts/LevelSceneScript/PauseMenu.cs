using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {

    float musicOriginalValue;

    private void Awake()
    {
        AudioManager.instance.mixer.GetFloat("MusicVolume", out musicOriginalValue);
        //AudioManager.instance.mixer.SetFloat("MusicVolume", 0);
    }

    // Use this for initialization
    void Start () {
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void PasueGame()
    {
        Time.timeScale = 0;
    }
    
    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void BackToLevelsScene()
    {
        Time.timeScale = 1;
        AudioManager.instance.mixer.SetFloat("MusicVolume", musicOriginalValue);
        SceneManager.LoadScene("LevelsScene");
    }

    public void BackToStartScene()
    {
        Time.timeScale = 1;
        AudioManager.instance.mixer.SetFloat("MusicVolume", musicOriginalValue);
        SceneManager.LoadScene("StartScene");
    }
}
