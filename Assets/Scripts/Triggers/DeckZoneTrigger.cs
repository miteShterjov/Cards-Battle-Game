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
            Debug.Log($"TriggerEnter2D hit: {other.gameObject.name}");
            if (!other.TryGetComponent(out Card card)) return;
            Debug.Log($"Card detected: {card.GetCardData()?.cardName}");
            _pendingCard = card;
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            Debug.Log($"TriggerExit2D hit: {other.gameObject.name}, pendingCard null: {_pendingCard == null}");
            if (!other.TryGetComponent(out Card card)) return;
            if (card != _pendingCard) return;

            bool mouseReleased = !Mouse.current.leftButton.isPressed;
            Debug.Log($"Mouse released on exit: {mouseReleased}");
    
            if (mouseReleased)
                DeckEvents.AddCardToDeck(_pendingCard.GetCardData());
    
            _pendingCard = null;
        }
    }
}