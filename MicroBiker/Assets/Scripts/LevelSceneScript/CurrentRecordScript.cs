using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CurrentRecordScript : MonoBehaviour {

    [Header("Record values")]
    public TextMeshProUGUI time;
    public TextMeshProUGUI bugs;
    public TextMeshProUGUI coins;
    public TextMeshProUGUI score;

    Level levelData;

    private void Awake()
    {
        levelData = GameManager.instance.levelData;
    }

    // Use this for initialization
    void Start () {
        time.text = levelData.GoalsAchieved.Time.ToString("##.##") + " s";
        bugs.text = levelData.GoalsAchieved.Bugs.ToString() + " / " + levelData.LevelGoals.Bugs;
        coins.text = levelData.GoalsAchieved.Coins.ToString() + " / " + levelData.LevelGoals.Coins;
        score.text = levelData.MaxScore.ToString();
    }
}
