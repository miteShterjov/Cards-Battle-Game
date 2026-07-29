using System.Collections.Generic;
using Events;
using Managers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Cards
{
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
            ShuffleCards();
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
        
        public void ReshuffleFromDiscardPile()
        {
            discardPile.MoveCardsToDeck(drawPile);
            ShuffleCards();
            DeckDrawVisuals();
        }

        private void ShuffleCards()
        {
            for (int i = 0; i < drawPile.Count; i++)
            {
                CardData card = drawPile[i];
                int randomIndex = Random.Range(i, drawPile.Count);
                drawPile[i] = drawPile[randomIndex];
                drawPile[randomIndex] = card;
            }
        }

        private void OnMouseDown()
        {
            if (drawPile.Count == 0) return;
            if (TurnSystem.Instance.HasActionsRemaining()) PlayerEvents.DrawCardRequested();
            DeckDrawVisuals();
        }

        private void DeckDrawVisuals()
        {
            foreach (Transform child in transform) Destroy(child.gameObject);

            Sprite cardBackSprite = CosmeticsManager.Instance.GetSelectedCardBack().sprite;

            for (int i = 0; i < drawPile.Count; i++)
            {
                GameObject newCardBack = Instantiate(cardBackPrefab, transform);
                newCardBack.GetComponent<SpriteRenderer>().sprite = cardBackSprite;
                newCardBack.transform.localPosition = new Vector3(0, i * -drawDeckOffset, 0);
            }
        }
    }
}
