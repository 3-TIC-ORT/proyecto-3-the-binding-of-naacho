using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor.Build.Content;

public static class SaveManager
{
    public static void SaveItemsUnlocked(string[] normalItemsNamesP, string[] specialItemsNamesP)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/itemsUnlocked.putoelquelee";
        FileStream stream = new FileStream(path, FileMode.Create);
        ItemsUnlocked data = new ItemsUnlocked(normalItemsNamesP, specialItemsNamesP);
        formatter.Serialize(stream, data);
        stream.Close();
    }
    public static ItemsUnlocked LoadItemsUnlocked()
    {
        string path = Application.persistentDataPath + "/itemsUnlocked.putoelquelee";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            ItemsUnlocked data = formatter.Deserialize(stream) as ItemsUnlocked;
            stream.Close();

            return data;
        }
        else
        {
            Debug.LogWarning("Save file not found in: " + path);
            SaveItemsUnlocked(RoomTemplates.normalDefaultItemsNames, RoomTemplates.specialDefaultItemsNames);
            return new ItemsUnlocked(RoomTemplates.normalDefaultItemsNames, RoomTemplates.specialDefaultItemsNames);
        }

    }

    public static void ResetItemsUnlocked()
    {
        SaveItemsUnlocked(RoomTemplates.normalDefaultItemsNames, RoomTemplates.specialDefaultItemsNames);
        Debug.Log("Se saldrá del juego");
        Application.Quit();
    }
}
