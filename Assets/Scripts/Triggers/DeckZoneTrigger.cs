using Cards;
using Events;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Triggers
{
    public class DeckZoneTrigger : MonoBehaviour
    {
        private Card _pendingCard;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.TryGetComponent(out Card card)) return;
            _pendingCard = card;
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.TryGetComponent(out Card card)) return;
            if (card != _pendingCard) return;

            if (!Mouse.current.leftButton.isPressed)
                DeckEvents.AddCardToDeck(_pendingCard.GetCardData()); // released inside zone
            
            _pendingCard = null; // always clear
        }
    }
}