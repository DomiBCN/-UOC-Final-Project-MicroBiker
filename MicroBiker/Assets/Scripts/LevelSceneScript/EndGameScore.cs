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
        SetCountersValues();
        StartCoroutine(SumScores());
    }

    private void Update()
    {
        CheckForStars();
    }

    void SetCountersValues()
    {
        timerTxt.text = timer + " / " + levelData.LevelGoals.Time;
        bugsTxt.text = bugs + " / " + levelData.LevelGoals.Bugs;
        coinsTxt.text = coins + " / " + levelData.LevelGoals.Coins;
    }

    //Scores counter effect
    IEnumerator SumScores()
    {
        if (timer <= levelData.LevelGoals.Time)
        {
            timerContainer.SetActive(true);
            timerPointsContainer.SetActive(true);

            //Bonus points per extra second 
            int bonusPoints = pointsPerSection + Convert.ToInt32((pointsPerSection / levelData.LevelGoals.Time) * timer);
            totalScoreValue += bonusPoints;
            while (scoreCounter < bonusPoints)
            {
                scoreCounter += scoreCounterIncrement;
                if (scoreCounter > bonusPoints)
                {
                    scoreCounter = bonusPoints;
                }
                timerPointsTxt.text = scoreCounter.ToString();
                yield return null;
            }
            scoreCounter = 0;
        }

        if (bugs > 0)
        {
            bugsContainer.SetActive(true);
            bugsPointsContainer.SetActive(true);

            //Bonus points per extra second 
            int bonusPoints = Convert.ToInt32((pointsPerSection / levelData.LevelGoals.Bugs) * bugs);
            totalScoreValue += bonusPoints;
            while (scoreCounter < bonusPoints)
            {
                scoreCounter += scoreCounterIncrement;
                if (scoreCounter > bonusPoints)
                {
                    scoreCounter = bonusPoints;
                }
                bugsPointsTxt.text = scoreCounter.ToString();
                yield return null;
            }
            scoreCounter = 0;
        }

        if (coins > 0)
        {
            coinsContainer.SetActive(true);
            coinsPointsContainer.SetActive(true);

            //Bonus points per extra second 
            int bonusPoints = Convert.ToInt32((pointsPerSection / levelData.LevelGoals.Coins) * coins);
            totalScoreValue += bonusPoints;
            while (scoreCounter < bonusPoints)
            {
                scoreCounter += scoreCounterIncrement;
                if (scoreCounter > bonusPoints)
                {
                    scoreCounter = bonusPoints;
                }
                coinsPointsTxt.text = scoreCounter.ToString();
                yield return null;
            }
            scoreCounter = 0;
        }


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
