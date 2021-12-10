using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveManager
{
    private static string filePath = "/save.fish";

    public static void SaveGame(SaveData save) {
        string path = Application.persistentDataPath + filePath;

        File.WriteAllText(path, JsonUtility.ToJson(save));
    }

    public static SaveData LoadGame() {
        string path = Application.persistentDataPath + filePath;
        if(File.Exists(path)) {
            return JsonUtility.FromJson<SaveData>(File.ReadAllText(path));
        } else {
            return null;
        }
    }

    public static void ClearData() {
        string path = Application.persistentDataPath + filePath;
        if(File.Exists(path)) {
            File.Delete(path);
        }
    }
}
