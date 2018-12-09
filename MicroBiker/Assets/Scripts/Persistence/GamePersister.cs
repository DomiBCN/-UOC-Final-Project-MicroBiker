using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class GamePersister
{

    const string gameDataPath = "/GameData";

    public static void UpdateAudioSettings(List<Level> levelsData)
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
            UpdateAudioSettings(levelsData);
            return levelsData;
        }
    }

    public static Level GetLevelById(int levelId)
    {
        return LoadLevelsData().Where(l => l.LevelId == levelId).FirstOrDefault();
    }
}
