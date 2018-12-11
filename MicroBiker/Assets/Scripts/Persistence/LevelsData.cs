using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Level
{
    public int LevelId;
    public LevelGoals LevelGoals;
    public LevelGoals GoalsAchieved;
    public double MaxScore;
    public bool Unblocked;
}

[Serializable]
public class LevelGoals
{
    public float Time;
    public int Bugs;
    public int Coins;
}

public class LevelsFiller
{
    public static List<Level> FillLevels()
    {
        List<Level> levels = new List<Level>();

        levels.Add(new Level()
        {
            LevelId = 1,
            LevelGoals = new LevelGoals()
            {
                Time = 21,
                Bugs = 2,
                Coins = 7
            },
            GoalsAchieved = new LevelGoals()
            {
                Time = 0,
                Bugs = 0,
                Coins = 0
            },
            MaxScore = 0,
            Unblocked = true
        });

        levels.Add(new Level()
        {
            LevelId = 2,
            LevelGoals = new LevelGoals()
            {
                Time = 34,
                Bugs = 3,
                Coins = 15
            },
            GoalsAchieved = new LevelGoals()
            {
                Time = 0,
                Bugs = 0,
                Coins = 0
            },
            MaxScore = 0,
            Unblocked = false
        });

        return levels;
    }
}
