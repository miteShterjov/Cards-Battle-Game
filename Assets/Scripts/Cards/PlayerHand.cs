using System.Collections.Generic;
using Events;
using Managers;
using UnityEngine;

namespace Cards
{
    public class PlayerHand : MonoBehaviour
    {
        [Header("Player Hand Config")]
        [SerializeField] private Deck deck;
        [SerializeField] private GameObject cardPrefab;
        [SerializeField] private int startingHandSize = 2;
        [SerializeField] private DiscardPile discardPile;

        [Header("Hand Layout Config")]
        [SerializeField] private float cardSpacing = 1.2f;
        [SerializeField] private float arcHeight = 0.8f;      // how much cards dip in the middle
        [SerializeField] private float maxRotation = 15f;     // tilt angle on outermost cards
        [SerializeField] private int maxHandSize = 5;

        private readonly List<Card> _cardsInHand = new List<Card>();

        private void Start()
        {
            for (int i = 0; i < startingHandSize; i++)
                DrawNextCard();
        }

        private void OnEnable()
        {
            TurnEvents.OnPlayerTurnEnds += DisableHand;
            TurnEvents.OnPlayerTurnStart += EnableHand;
            PlayerEvents.OnDrawCardRequested += DrawNextCard;
        }

        private void OnDisable()
        {
            TurnEvents.OnPlayerTurnEnds -= DisableHand;
            TurnEvents.OnPlayerTurnStart -= EnableHand;
            PlayerEvents.OnDrawCardRequested -= DrawNextCard;
        }

        public void PlayCard(Card card)
        {
            if (!TurnSystem.Instance.CanPlayCard(card.GetCardData())) return;
            _cardsInHand.Remove(card);
            discardPile.DiscardCard(card.GetCardData());
            Destroy(card.gameObject);
            RepositionCards();
            PlayerEvents.CardPlayed(card.GetCardData());
        }

        private void DrawNextCard()
        {
            if (_cardsInHand.Count >= maxHandSize)
            {
                Debug.Log("Hand is full, cannot draw.");
                return; // bail before touching AP at all
            }

            CardData cardData = deck.DrawCard();
            if (cardData == null) return; // deck empty

            GameObject newCard = Instantiate(cardPrefab, transform.position, Quaternion.identity, transform);
            Card cardComponent = newCard.GetComponent<Card>();
            cardComponent.LoadCardData(cardData);

            if (!TurnSystem.Instance.HasActionsRemaining())
                cardComponent.SetInteractable(false);

            _cardsInHand.Add(cardComponent);
            RepositionCards();

            PlayerEvents.DrawCardSucceeded(); // ← new event, only fires on actual success
        }

        private void RepositionCards()
        {
            int count = _cardsInHand.Count;
            for (int i = 0; i < count; i++)
            {
                float t = count == 1 ? 0f : (float)i / (count - 1) * 2f - 1f;

                float xPos = t * (cardSpacing * (count - 1) / 2f);
                float yPos = -Mathf.Abs(t) * arcHeight;
                float rotation = -t * maxRotation;

                _cardsInHand[i].transform.position = transform.position + new Vector3(xPos, yPos, i * -0.01f);
                _cardsInHand[i].transform.rotation = Quaternion.Euler(0, 0, rotation);
                _cardsInHand[i].UpdateOriginalTransform(); // ← keep cache in sync
            }
        }

        private void DisableHand()
        {
            foreach (Card card in _cardsInHand) card.SetInteractable(false);
        }

        private void EnableHand()
        {
            foreach (Card card in _cardsInHand) card.SetInteractable(true);
        }
    }
}