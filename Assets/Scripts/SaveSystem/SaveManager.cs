using System.IO;
using UnityEngine;

namespace SaveSystem
{
    public static class SaveManager
    {
        private static readonly string SavePath = Path.Combine(Application.persistentDataPath, "playerSave.json");

        public static void Save(PlayerSaveData data)
        {
            string json = JsonUtility.ToJson(data, true);
            File.WriteAllText(SavePath, json);
            Debug.Log("Saved to: " + SavePath);
        }

        public static PlayerSaveData Load()
        {
            if (!File.Exists(SavePath))
            {
                Debug.Log("No save file found.");
                return null; // ← let PlayerDataManager handle default creation
            }

            string json = File.ReadAllText(SavePath);
            return JsonUtility.FromJson<PlayerSaveData>(json);
        }

        public static bool SaveFileExists() => File.Exists(SavePath);

        public static void DeleteSave()
        {
            if (!File.Exists(SavePath)) return;
            File.Delete(SavePath);
            Debug.Log("Save file deleted.");
        }
    }
}