using System;
using System.Collections.Generic;
using Cards;
using Events;
using UnityEngine;
using UnityEngine.Serialization;

namespace Systems
{
    public class DeckManager : Singleton<DeckManager>
    {
        [SerializeField] private List<CardData> currentDeck;
        [SerializeField] private int maxDeckSize = 9;
        [SerializeField] private DefaultDeck defaultDeck;

        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(gameObject);
            currentDeck = new List<CardData>(defaultDeck.cards);
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

        private void AddCard(CardData cardData)
        {
            if (currentDeck.Count >= maxDeckSize) return;
            currentDeck.Add(cardData);
            DeckEvents.DeckProcessed();
        }
        
        private void RemoveCard(CardData cardData)
        {
            if (!currentDeck.Contains(cardData)) return;
            currentDeck.Remove(cardData);
            DeckEvents.DeckProcessed();
        }
    }
}
