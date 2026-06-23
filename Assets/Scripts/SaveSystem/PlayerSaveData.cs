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
        
        // sensible defaults for a brand new save
        public static PlayerSaveData CreateDefault()
        {
            return new PlayerSaveData
            {
                gold = 100, // starting gold, tweak as you like
                ownedCardIds = new List<string>(),
                ownedSkinIds = new List<string>(),
                currentDeckCardIds = new List<string>(),
                wins = 0,
                losses = 0
            };
        }
        
        public static PlayerSaveData CreateDefault(List<string> starterCardIds)
        {
            return new PlayerSaveData
            {
                gold = 100,
                ownedCardIds = new List<string>(starterCardIds),
                ownedSkinIds = new List<string>(),
                currentDeckCardIds = new List<string>(starterCardIds),
                wins = 0,
                losses = 0,
                lastLoginDate = ""
            };
        }
    }
}
