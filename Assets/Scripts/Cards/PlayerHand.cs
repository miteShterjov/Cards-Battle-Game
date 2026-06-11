using System;
using System.Collections.Generic;
using Cards;
using Events;
using Systems;
using UnityEngine;

public class PlayerHand : MonoBehaviour
{
    [Header("Player Hand Config")]
    [SerializeField] private Deck deck;
    [SerializeField] private Transform[] cardSlots;
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private int startingHandSize = 2;
    [SerializeField] private DiscardPile discardPile;

    private readonly List<Card> _cardsInHand = new List<Card>();
    
    private void Start()
    {
        for (int i = 0; i < startingHandSize; i++)
        {
            DrawNextCard();
        }
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

    public void DrawNextCard()
    {
        if (cardSlots == null || _cardsInHand.Count >= cardSlots.Length)
        {
            Debug.LogError("Either no card slots are assigned or the hand is full.");
            return;
        }
        
        CardData cardData = deck.DrawCard();
        int slotIndex = _cardsInHand.Count;
        
        if (cardData == null)
        {
            Debug.LogError("No cards left in deck.");
            return;
        }
        
        GameObject newCard = GetNewCard(slotIndex);
        Card cardComponent = newCard.GetComponent<Card>();
        
        cardComponent.LoadCardData(cardData);
        _cardsInHand.Add(cardComponent);
        _cardsInHand[slotIndex].transform.SetParent((cardSlots[slotIndex]));

        if (!TurnSystem.Instance.HasActionsRemaining())
        {
            cardComponent.SetInteractable(false);
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

    private void RepositionCards()
    {
        for (int i = 0; i < _cardsInHand.Count; i++)
        {
            _cardsInHand[i].transform.SetParent(null);
        }
        for (int i = 0; i < _cardsInHand.Count; i++)
        {
            _cardsInHand[i].transform.SetParent(cardSlots[i]);
            _cardsInHand[i].transform.position = cardSlots[i].position;
        }
    }
    
    private GameObject GetNewCard(int index) => 
        Instantiate(
            cardPrefab, 
            cardSlots[index].position, 
            cardSlots[index].rotation
            );
}
