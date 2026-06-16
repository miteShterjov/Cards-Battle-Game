using Events;
using TMPro;
using UnityEngine;

namespace Cards
{
    public class CardTab : MonoBehaviour
    {
        [Header("Card Tab Config")]
        [SerializeField] private SpriteRenderer illustration;
        [SerializeField] private SpriteRenderer cardBase;
        [SerializeField] private TextMeshProUGUI cardNameText;
        [SerializeField] private TextMeshProUGUI actionsPointText;
    
        private CardData _cardData;
        private Color _originalColor;

        private void Start()
        {
            _originalColor = cardBase.color;
        }

        public void LoadCardTabData(CardData cardData)
        {
            if (cardData == null) return;
        
            this._cardData = cardData;
            illustration.sprite = cardData.illustration;
            cardNameText.text = cardData.cardName;
            actionsPointText.text = cardData.actionCost.ToString();
        }
        
        public CardData GetCardData() => _cardData;

        private void OnMouseEnter()
        {
            cardBase.color = Color.yellowGreen;  
        }
    
        private void OnMouseExit()
        {
            cardBase.color = _originalColor;
        }

        private void OnMouseDown()
        {
            DeckEvents.RemoveCardFromDeck(_cardData);
        }
    }
}
