using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class AudioPersister
{

    const string menuSettingsPath = "/MenuSettings";

    public static void UpdateAudioSettings(MenuSettingsData settings)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(Application.persistentDataPath + menuSettingsPath, FileMode.Create);

        formatter.Serialize(stream, settings);
        stream.Close();
    }

    public static MenuSettingsData LoadAudioSettings()
    {
        if (File.Exists(Application.persistentDataPath + menuSettingsPath))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(Application.persistentDataPath + menuSettingsPath, FileMode.Open);

            MenuSettingsData settings = formatter.Deserialize(stream) as MenuSettingsData;
            stream.Close();

            return settings;
        }
        else
        {
            return new MenuSettingsData() { MusicStatus = true, SoundStatus = true };
        }
    }
}
