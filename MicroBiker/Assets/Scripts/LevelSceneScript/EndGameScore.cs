using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndGameScore : MonoBehaviour
{

    [Header("Stars")]
    public Image star1;
    public Image star2;
    public Image star3;

    [Header("Counters")]
    public GameObject timerContainer;
    public GameObject bugsContainer;
    public GameObject coinsContainer;


    [Header("Points texts")]
    public GameObject timerPointsContainer;
    public GameObject bugsPointsContainer;
    public GameObject coinsPointsContainer;

    [Header("Total score text")]
    public TextMeshProUGUI totalScore;

    [Header("Next level button")]
    public Button nextLevelBtn;

    //Counter texts
    TextMeshProUGUI timerTxt;
    TextMeshProUGUI bugsTxt;
    TextMeshProUGUI coinsTxt;

    //Points texts
    TextMeshProUGUI timerPointsTxt;
    TextMeshProUGUI bugsPointsTxt;
    TextMeshProUGUI coinsPointsTxt;

    Level levelData;
    int pointsPerSection = 10000;

    float timer;
    int bugs;
    int coins;

    int scoreCounter = 0;
    int totalScoreValue;
    int scoreCounterIncrement = 265;

    int timerTotalScore;
    int bugsTotalScore;
    int coinsTotalScore;
    int globalScore;

    LevelGoals goalsAchieved;

    private void Awake()
    {
        levelData = GameManager.instance.levelData;
        timer = Convert.ToSingle(GameManager.instance.timerValue);
        bugs = GameManager.instance.bugsKilled;
        coins = GameManager.instance.coinsCollected;

        timerTxt = timerContainer.GetComponentInChildren<TextMeshProUGUI>();
        bugsTxt = bugsContainer.GetComponentInChildren<TextMeshProUGUI>();
        coinsTxt = coinsContainer.GetComponentInChildren<TextMeshProUGUI>();

        timerPointsTxt = timerPointsContainer.GetComponentInChildren<TextMeshProUGUI>();
        bugsPointsTxt = bugsPointsContainer.GetComponentInChildren<TextMeshProUGUI>();
        coinsPointsTxt = coinsPointsContainer.GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Start()
    {
        AudioManager.instance.Stop("MotorbikeRun");
        AudioManager.instance.Play("Win");
        CalculeTotalScores();
        if (globalScore > levelData.MaxScore)
        {
            //Update max score and unblock next level
            PersistLevelScore();
        }
        //Unblock next level
        UnblockNextLevel();

        SetCountersValues();
        StartCoroutine(SumScores());
    }

    void PersistLevelScore()
    {
        goalsAchieved = new LevelGoals()
        {
            Time = timer,
            Bugs = bugs,
            Coins = coins
        };
        GamePersister.UpdateLevelScore(levelData.LevelId, goalsAchieved, globalScore);
    }

    void UnblockNextLevel()
    {
        bool unblocked = GamePersister.UnblockNextLevel(levelData.LevelId + 1);
        //if true, means that there's a next level available
        if(unblocked)
        {
            nextLevelBtn.interactable = true;
            nextLevelBtn.onClick.AddListener(() =>
            {
                GameManager.instance.NextLevel(levelData.LevelId+1);
            });
        }
    }

    //sets the menu counters
    void SetCountersValues()
    {
        timerTxt.text = timer + " / " + levelData.LevelGoals.Time;
        bugsTxt.text = bugs + " / " + levelData.LevelGoals.Bugs;
        coinsTxt.text = coins + " / " + levelData.LevelGoals.Coins;
    }
    //calcule the scores for each field
    void CalculeTotalScores()
    {
        if (timer <= levelData.LevelGoals.Time)
        {
            //bonus points for each second spared
            float secondsSpared = levelData.LevelGoals.Time - timer;//bonus points for each second spared
            float valuePerSecondSpared = pointsPerSection / levelData.LevelGoals.Time;
            timerTotalScore = Convert.ToInt16(pointsPerSection + (secondsSpared * valuePerSecondSpared));//add bonus points, to the timer score base value, for each second spared
        }
        else
        {
            timerTotalScore = 0;
        }
        bugsTotalScore = Convert.ToInt32((pointsPerSection / levelData.LevelGoals.Bugs) * bugs); ;
        coinsTotalScore = Convert.ToInt32((pointsPerSection / levelData.LevelGoals.Coins) * coins);
        globalScore = timerTotalScore + bugsTotalScore + coinsTotalScore;
    }

    //Scores sum effect
    IEnumerator SumScores()
    {
        //timer points
        if (timer <= levelData.LevelGoals.Time)
        {
            timerContainer.SetActive(true);
            timerPointsContainer.SetActive(true);

            totalScoreValue += timerTotalScore;
            while (scoreCounter < timerTotalScore)
            {
                CheckForStars();
                scoreCounter += scoreCounterIncrement;
                if (scoreCounter > timerTotalScore)
                {
                    scoreCounter = timerTotalScore;
                }
                timerPointsTxt.text = scoreCounter.ToString();
                yield return null;
            }
            scoreCounter = 0;
        }
        //bugs points
        if (bugs > 0)
        {
            bugsContainer.SetActive(true);
            bugsPointsContainer.SetActive(true);

            totalScoreValue += bugsTotalScore;
            while (scoreCounter < bugsTotalScore)
            {
                CheckForStars();
                scoreCounter += scoreCounterIncrement;
                if (scoreCounter > bugsTotalScore)
                {
                    scoreCounter = bugsTotalScore;
                }
                bugsPointsTxt.text = scoreCounter.ToString();
                yield return null;
            }
            scoreCounter = 0;
        }
        //coins points
        if (coins > 0)
        {
            coinsContainer.SetActive(true);
            coinsPointsContainer.SetActive(true);

            totalScoreValue += coinsTotalScore;
            while (scoreCounter < coinsTotalScore)
            {
                CheckForStars();
                scoreCounter += scoreCounterIncrement;
                if (scoreCounter > coinsTotalScore)
                {
                    scoreCounter = coinsTotalScore;
                }
                coinsPointsTxt.text = scoreCounter.ToString();
                yield return null;
            }
            scoreCounter = 0;
        }

        //total points
        while (scoreCounter < totalScoreValue)
        {
            scoreCounter += scoreCounterIncrement;
            if (scoreCounter > totalScoreValue)
            {
                scoreCounter = totalScoreValue;
            }
            totalScore.text = scoreCounter.ToString();
            yield return null;
        }
        scoreCounter = 0;

        yield return null;
    }

    //used to add stars while summing points
    void CheckForStars()
    {
        if (totalScoreValue >= (pointsPerSection * 3))
        {
            star3.enabled = true;
        }
        else if (totalScoreValue >= (pointsPerSection * 2))
        {
            star2.enabled = true;
        }
        else if (totalScoreValue >= (pointsPerSection))
        {
            star1.enabled = true;
        }
    }
}
