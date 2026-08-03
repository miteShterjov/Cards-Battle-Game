using System.IO;
using UnityEngine;

namespace SaveSystem
{
    public static class SaveManager
    {
        private const string SaveKey = "DeckOfRunesSave";

        public static void Save(PlayerSaveData data)
        {
            string json = JsonUtility.ToJson(data, true);
            PlayerPrefs.SetString(SaveKey, json);
            PlayerPrefs.Save();
            Debug.Log("Game saved to PlayerPrefs");
        }

        public static PlayerSaveData Load()
        {
            if (!PlayerPrefs.HasKey(SaveKey))
            {
                Debug.Log("No save found in PlayerPrefs");
                return null;
            }

            string json = PlayerPrefs.GetString(SaveKey);
            return JsonUtility.FromJson<PlayerSaveData>(json);
        }

        public static bool SaveFileExists() => PlayerPrefs.HasKey(SaveKey);

        public static void DeleteSave()
        {
            PlayerPrefs.DeleteKey(SaveKey);
            PlayerPrefs.Save();
            Debug.Log("Save deleted from PlayerPrefs");
        }
    }
}