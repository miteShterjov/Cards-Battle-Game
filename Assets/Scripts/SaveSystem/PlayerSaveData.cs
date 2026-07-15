using System;
using System.Collections.Generic;

namespace SaveSystem
{
    [Serializable]
    public class PlayerSaveData
    {
        public int gold;
        public List<string> ownedCardIds = new List<string>();
        public List<string> ownedSkinIds = new List<string>();
        public List<string> currentDeckCardIds = new List<string>();
        public int wins;
        public int losses;
        public string lastLoginDate;
        public string selectedCardBackId;

        // Run progression
        public bool runActive;
        public int currentNodeIndex;

        public static PlayerSaveData CreateDefault(List<string> starterCardIds)
        {
            return new PlayerSaveData
            {
                gold = 1000,
                ownedCardIds = new List<string>(starterCardIds),
                ownedSkinIds = new List<string>(),
                currentDeckCardIds = new List<string>(starterCardIds),
                wins = 21,
                losses = 4,
                lastLoginDate = "",
                selectedCardBackId = "",
                runActive = false,
                currentNodeIndex = 0
            };
        }
    }
}