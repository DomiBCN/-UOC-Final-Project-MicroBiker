using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    [Header("Panels")]
    public GameObject startPanel;
    public GameObject bestScorePanel;
    public GameObject tutorialPanel;
    public GameObject finalScore;

    [Header("HUD")]
    public TextMeshProUGUI timer;
    public TextMeshProUGUI bugCounter;
    public TextMeshProUGUI coinsCounter;
    public TextMeshProUGUI scoreCounter;

    [Header("Points PopUps")]
    public GameObject coinPopUp;
    public GameObject bugPopUp;

    [Header("Levels")]
    public Transform levelContainer;
    public List<GameObject> levels = new List<GameObject>();

    [HideInInspector] public static GameManager instance;
    [HideInInspector] public Level levelData;
    [HideInInspector] public double timerValue;
    [HideInInspector] public int bugsKilled = 0;
    [HideInInspector] public int coinsCollected = 0;
    [HideInInspector] public bool gameStarted;

    GameObject canvas;

    int currentLevelId;

    float startTime;

    int pointsPerSection = 10000;
    int pointsPerBug;
    int pointsPerCoin;

    int totalScoreCounter;

    private void Awake()
    {
        instance = this;
        InitializeLevel();
        Time.timeScale = 0;
        canvas = GameObject.Find("Canvas");
    }

    // Use this for initialization
    void Start()
    {

        if (levelData.MaxScore != 0)
        {
            bestScorePanel.SetActive(true);
        }
        else
        {
            startPanel.SetActive(true);
            tutorialPanel.SetActive(true);
        }
        UpdateCounter(bugCounter, bugsKilled, levelData.LevelGoals.Bugs);
        UpdateCounter(coinsCounter, coinsCollected, levelData.LevelGoals.Coins);
        UpdateScoreCounter(0);

        pointsPerBug = pointsPerSection / levelData.LevelGoals.Bugs;
        pointsPerCoin = pointsPerSection / levelData.LevelGoals.Coins;
        AudioManager.instance.DecreaseGroupAudio("MusicVolume");
    }

    // Update is called once per frame
    void Update()
    {
        if (gameStarted)
        {
            timerValue = Math.Round((Time.time - startTime), 2);
            timer.text = timerValue.ToString("##.##") + " s";
        }
    }

    void InitializeLevel()
    {
        currentLevelId = PlayerPrefs.GetInt("CurrentLevel", 1);
        //add level prefab
        GameObject levelPrefab = Instantiate(levels[currentLevelId - 1]);
        levelPrefab.transform.SetParent(levelContainer);

        levelData = GamePersister.GetLevelById(currentLevelId);
    }

    public void StartGame()
    {
        gameStarted = true;
        startTime = Time.time;
        AudioManager.instance.Play("MotorbikeRun");
        Time.timeScale = 1;
    }

    public void FinshLineReached()
    {
        Time.timeScale = 0;
        finalScore.SetActive(true);
    }

    public void NextLevel(int nextLevelId)
    {
        PlayerPrefs.SetInt("CurrentLevel", nextLevelId);
        RestartLevel();
    }

    public void RestartLevel()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    #region COUNTERS
    public void UpdateBugCounter(Vector3 bugPosition)
    {
        bugsKilled++;
        UpdateCounter(bugCounter, bugsKilled, levelData.LevelGoals.Bugs);
        UpdateScoreCounter(pointsPerBug);

        //PopUpText
        InstantiatePopUpText(bugPosition, bugPopUp, pointsPerBug);
    }

    public void UpdateCoinsCounter(Vector3 coinPosition)
    {
        coinsCollected++;
        AudioManager.instance.Play("Coin");
        UpdateCounter(coinsCounter, coinsCollected, levelData.LevelGoals.Coins);
        UpdateScoreCounter(pointsPerCoin);

        //PopUpText
        InstantiatePopUpText(coinPosition, coinPopUp, pointsPerCoin);
    }

    public void UpdateScoreCounter(int points)
    {
        totalScoreCounter += points;
        scoreCounter.text = totalScoreCounter.ToString();
    }

    void UpdateCounter(TextMeshProUGUI counterTxt, int currentCounter, int totalCounter)
    {
        counterTxt.text = currentCounter + " / " + totalCounter;
    }

    void InstantiatePopUpText(Vector3 position, GameObject popUpPrefab, int points)
    {
        Vector2 screenPosition = Camera.main.WorldToScreenPoint(new Vector2(position.x + UnityEngine.Random.Range(-0.5f, 0.5f), position.y + 2));
        GameObject popUpInstance = Instantiate(popUpPrefab, screenPosition, Quaternion.identity, canvas.transform);
        popUpInstance.GetComponent<TextMeshProUGUI>().text = "+" + points.ToString();
    }
    #endregion
}
