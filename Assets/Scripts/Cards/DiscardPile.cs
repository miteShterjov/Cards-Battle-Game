using System;
using System.Collections.Generic;
using Cards;
using Systems;
using UnityEngine;
using UnityEngine.Rendering;

public class DiscardPile : MonoBehaviour
{
    [Header("Discard Pile Config")]
    [SerializeField] private List<CardData> discardPile = new List<CardData>();
    [SerializeField] private Deck deck;
    [SerializeField] private GameObject cardPrefab;
    
    [Header("Deck Visuals Config")]
    [SerializeField] private float drawDeckOffset = 0.25f;

    public void DiscardCard(CardData cardData)
    {
        discardPile.Add(cardData);
        
        GameObject discardedCard = Instantiate(cardPrefab, transform);
        
        discardedCard.GetComponent<Card>().LoadCardData(cardData);
        discardedCard.GetComponent<Card>().SetInteractable(false);
        
        SortingGroup sortingGroup = discardedCard.GetComponent<SortingGroup>();
        sortingGroup.sortingOrder = discardPile.Count - 1;
        
        discardedCard.transform.SetParent(transform);
        discardedCard.transform.localPosition = new Vector3(0, (discardPile.Count - 1) * -drawDeckOffset, 0);
    }

    public void MoveCardsToDeck(List<CardData> drawPile)
    {
        if (!TurnSystem.Instance.CanReshuffleDiscardPile()) return;
        if (drawPile == null) return;
        if (discardPile.Count == 0) return;
        drawPile.AddRange(discardPile);
        ClearDiscardPile();
    }

    private void ClearDiscardPile()
    {
        if (!TurnSystem.Instance.CanReshuffleDiscardPile()) return;
        discardPile.Clear();
        foreach (Transform discardedCard in transform)
        {
            Destroy(discardedCard.gameObject);
        }
    }

    private void OnMouseDown()
    {
        if (!TurnSystem.Instance.CanReshuffleDiscardPile()) return;
        if (!TurnSystem.Instance.HasActionsRemaining()) return;
        if (discardPile.Count <= 0) return;
        deck.ReshuffleFromDiscardPile(); // ← move cards first
        PlayerEvents.ReshuffleRequested(); // ← consume actions after
    }
}
