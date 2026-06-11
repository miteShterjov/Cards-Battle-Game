using System;
using System.Collections.Generic;
using Systems;
using UnityEngine;
using Random = UnityEngine.Random;

public class Deck : MonoBehaviour
{
    [Header("Deck Config")]
    [SerializeField] private GameObject cardBackPrefab;
    [SerializeField] private List<CardData> drawPile = new List<CardData>();
    [SerializeField] private DiscardPile discardPile;
    [Header("Deck Visuals Config")]
    [SerializeField] private float drawDeckOffset = 0.25f;

    private void Start()
    {
        drawPile = DeckManager.Instance.GetDeck();
        DeckDrawVisuals();
    }

    public CardData DrawCard()
    {
        if (drawPile.Count == 0) return null;
        
        int topIndex = drawPile.Count - 1;
        CardData card = drawPile[topIndex];
        drawPile.RemoveAt(topIndex);
        DeckDrawVisuals();
        
        return card;
    }
    
    public void ShuffleCards()
    {
        for (int i = 0; i < drawPile.Count; i++)
        {
            CardData card = drawPile[i];
            int randomIndex = Random.Range(i, drawPile.Count);
            drawPile[i] = drawPile[randomIndex];
            drawPile[randomIndex] = card;
        }
    }

    public void ReshuffleFromDiscardPile()
    {
        discardPile.MoveCardsToDeck(drawPile);
        ShuffleCards();
        DeckDrawVisuals();
    }

    private void OnMouseDown()
    {
        if (drawPile.Count == 0) return;
        if (TurnSystem.Instance.HasActionsRemaining()) PlayerEvents.DrawCardRequested();
        DeckDrawVisuals();
    }

    private void DeckDrawVisuals()
    {
        foreach (Transform discardedCard in transform)
        {
            Destroy(discardedCard.gameObject);
        }
        
        for (int i = 0; i < drawPile.Count; i++)
        {
            GameObject newCardBack = Instantiate(cardBackPrefab, transform);
            newCardBack.transform.localPosition = new Vector3(0, i * -drawDeckOffset, 0);
        }
    }
}
