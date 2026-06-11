using System;
using UnityEngine;

namespace Events
{
    public class DeckEvents : MonoBehaviour
    {
        public static event Action<CardData> OnRemoveCardFromDeck;
        public static event Action<CardData> OnAddCardToDeck;
        public static event Action OnDeckProcessed;
        
        public static void RemoveCardFromDeck(CardData cardData) => OnRemoveCardFromDeck?.Invoke(cardData);
        public static void AddCardToDeck(CardData cardData) => OnAddCardToDeck?.Invoke(cardData);
        public static void DeckProcessed() => OnDeckProcessed?.Invoke();
    }
}