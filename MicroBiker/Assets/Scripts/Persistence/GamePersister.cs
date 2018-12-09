using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class GamePersister
{

    const string gameDataPath = "/GameData";

    public static void UpdateLevels(List<Level> levelsData)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(Application.persistentDataPath + gameDataPath, FileMode.Create);

        formatter.Serialize(stream, levelsData);
        stream.Close();
    }

    public static List<Level> LoadLevelsData()
    {
        if (File.Exists(Application.persistentDataPath + gameDataPath))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(Application.persistentDataPath + gameDataPath, FileMode.Open);

            List<Level> levelsData = formatter.Deserialize(stream) as List<Level>;
            stream.Close();

            return levelsData.OrderBy(l => l.LevelId).ToList();
        }
        else
        {
            List<Level> levelsData = new List<Level>();
            levelsData = LevelsFiller.FillLevels();

            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(Application.persistentDataPath + gameDataPath, FileMode.Create);

            formatter.Serialize(stream, levelsData);
            stream.Close();
            UpdateLevels(levelsData);
            return levelsData;
        }
    }

    public static Level GetLevelById(int levelId)
    {
        return LoadLevelsData().Where(l => l.LevelId == levelId).FirstOrDefault();
    }

    public static void UpdateLevelScore(int levelId, LevelGoals levelGoalsAchieved, int newScoreRecord)
    {
        List<Level> levelsData = LoadLevelsData();

        var indexToUpdate = levelsData.IndexOf(levelsData.First(l => l.LevelId == levelId));
        levelsData[indexToUpdate].GoalsAchieved = levelGoalsAchieved;
        levelsData[indexToUpdate].MaxScore = newScoreRecord;
        
        UpdateLevels(levelsData);
    }

    public static bool UnblockNextLevel(int levelIdToUnblock)
    {
        List<Level> levelsData = LoadLevelsData();
        
        //check if we have to unblock the next level
        var nextLevel = levelsData.Find(l => l.LevelId == levelIdToUnblock);
        if (nextLevel != null)
        {
            levelsData[levelsData.IndexOf(nextLevel)].Unblocked = true;
            UpdateLevels(levelsData);
            return true;
        }
        else
        {
            return false;
        }
        
    }
}
