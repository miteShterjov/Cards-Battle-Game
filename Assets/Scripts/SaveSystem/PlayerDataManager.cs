using System.Collections.Generic;
using Cards;
using Managers;
using UnityEngine;

namespace SaveSystem
{
    public class PlayerDataManager : Singleton<PlayerDataManager>
    {
        [Header("Starter Setup")]
        [SerializeField] private StarterDeck starterDeck;
        [Header("Daily Login")]
        [SerializeField] private int dailyLoginGold = 2;

        public event System.Action<int> OnDailyLoginBonusGranted;

        public PlayerSaveData CurrentData { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(gameObject);
            LoadOrCreateSave();
            CheckDailyLogin();
        }
        
        private void Start()
        {
            Debug.Log(Application.persistentDataPath);
            Debug.Log(JsonUtility.ToJson(CurrentData, true)); // pretty-printed save data
        }

        public void SaveGame() => SaveManager.Save(CurrentData);
        
        public void AddGold(int amount)
        {
            CurrentData.gold += amount;
            SaveGame();
        }

        public bool SpendGold(int amount)
        {
            if (CurrentData.gold < amount) return false;
            CurrentData.gold -= amount;
            SaveGame();
            return true;
        }

        public bool OwnsCard(string cardId) => CurrentData.ownedCardIds.Contains(cardId);

        public void UnlockCard(string cardId)
        {
            if (OwnsCard(cardId)) return;
            CurrentData.ownedCardIds.Add(cardId);
            SaveGame();
        }

        private void CheckDailyLogin()
        {
            string today = System.DateTime.Now.ToString("yyyy-MM-dd");
            
            if (CurrentData.lastLoginDate == today) return;

            CurrentData.lastLoginDate = today;
            AddGold(dailyLoginGold);
            
            Debug.Log($"Daily login bonus granted: +{dailyLoginGold} gold");
            OnDailyLoginBonusGranted?.Invoke(dailyLoginGold);
        }
        
        private void LoadOrCreateSave()
        {
            if (SaveManager.SaveFileExists())
            {
                CurrentData = SaveManager.Load();
                if (CurrentData == null) // safety net if file exists but is corrupt
                {
                    CreateDefaultSave();
                }
            }
            else
            {
                CreateDefaultSave();
            }
        }

        private void CreateDefaultSave()
        {
            List<string> starterCardIds = new List<string>();
            foreach (CardData card in starterDeck.cards)
                starterCardIds.Add(card.cardId);

            CurrentData = PlayerSaveData.CreateDefault(starterCardIds);
            SaveManager.Save(CurrentData);
        }
    }
}