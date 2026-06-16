using Cards;
using UnityEngine;

namespace Triggers
{
    public class PlayZoneTrigger : MonoBehaviour
    {
        [SerializeField] private PlayerHand playerHand;
    
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out Card card))
            {
                playerHand.PlayCard(card);
            }
        }
    }
}
