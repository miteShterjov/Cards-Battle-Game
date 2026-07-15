using System.Collections.Generic;
using System.Linq;
using Cards;
using Events;
using SaveSystem;
using UnityEngine;

namespace Managers
{
    public class DeckManager : Singleton<DeckManager>
    {
        [SerializeField] private List<CardData> currentDeck = new List<CardData>();
        [SerializeField] private int maxDeckSize = 20;
        [SerializeField] private StarterDeck starterDeck;
        [SerializeField] private CardDatabase cardDatabase; // needed to resolve IDs back into CardData

        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            LoadDeckFromSave();
            DeckEvents.DeckProcessed();
        }

        private void OnEnable()
        {
            DeckEvents.OnAddCardToDeck += AddCard;
            DeckEvents.OnRemoveCardFromDeck += RemoveCard;
        }

        private void OnDisable()
        {
            DeckEvents.OnAddCardToDeck -= AddCard;
            DeckEvents.OnRemoveCardFromDeck -= RemoveCard;
        }

        public List<CardData> GetDeck() => new List<CardData>(currentDeck);

        private void LoadDeckFromSave()
        {
            List<string> savedIds = PlayerDataManager.Instance.CurrentData.currentDeckCardIds;
            Debug.Log($"Saved IDs count: {savedIds?.Count ?? -1}");

            if (savedIds == null || savedIds.Count == 0)
            {
                currentDeck = new List<CardData>(starterDeck.cards);
                SyncDeckToSave();
                return;
            }

            currentDeck = savedIds
                .Select(id => cardDatabase.allCards.FirstOrDefault(card => card.cardId == id))
                .Where(card => card != null)
                .ToList();

            Debug.Log($"Resolved currentDeck count: {currentDeck.Count}");
        }

        private void SyncDeckToSave()
        {
            PlayerDataManager.Instance.CurrentData.currentDeckCardIds =
                currentDeck.Select(card => card.cardId).ToList();
            PlayerDataManager.Instance.SaveGame();
        }

        private void AddCard(CardData cardData)
        {
            Debug.Log($"AddCard called — card: {cardData?.cardName}, deckCount: {currentDeck.Count}, maxSize: {maxDeckSize}, ownsCard: {PlayerDataManager.Instance.OwnsCard(cardData.cardId)}");
    
            if (currentDeck.Count >= maxDeckSize) return;
            if (!PlayerDataManager.Instance.OwnsCard(cardData.cardId)) return;

            currentDeck.Add(cardData);
            SyncDeckToSave();
            DeckEvents.DeckProcessed();
        }

        private void RemoveCard(CardData cardData)
        {
            if (!currentDeck.Contains(cardData)) return;

            currentDeck.Remove(cardData);
            SyncDeckToSave();
            DeckEvents.DeckProcessed();
        }
    }
}