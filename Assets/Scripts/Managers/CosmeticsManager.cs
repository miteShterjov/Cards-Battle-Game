using System.Collections.Generic;
using Cosmetics;
using SaveSystem;
using UnityEngine;

namespace Managers
{
    public class CosmeticsManager : Singleton<CosmeticsManager>
    {
        [SerializeField] private CardBackDatabase cardBackDatabase;

        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(gameObject);
        }

        // Card Backs
        public CardBackData GetSelectedCardBack()
        {
            string selectedId = PlayerDataManager.Instance.CurrentData.selectedCardBackId;

            if (string.IsNullOrEmpty(selectedId))
                return cardBackDatabase.defaultCardBack;

            CardBackData found = cardBackDatabase.allCardBacks
                .Find(cb => cb.cardBackId == selectedId);

            return found ?? cardBackDatabase.defaultCardBack;
        }

        public void SelectCardBack(string cardBackId)
        {
            PlayerDataManager.Instance.CurrentData.selectedCardBackId = cardBackId;
            PlayerDataManager.Instance.SaveGame();
        }

        public bool OwnsCardBack(string cardBackId)
        {
            return cardBackDatabase.defaultCardBack.cardBackId == cardBackId 
                   || PlayerDataManager.Instance.CurrentData.ownedSkinIds.Contains(cardBackId);
        }

        public void UnlockCardBack(string cardBackId)
        {
            if (OwnsCardBack(cardBackId)) return;
            PlayerDataManager.Instance.CurrentData.ownedSkinIds.Add(cardBackId);
            PlayerDataManager.Instance.SaveGame();
        }

        public List<CardBackData> GetAllCardBacks() => cardBackDatabase.allCardBacks;
    }
}