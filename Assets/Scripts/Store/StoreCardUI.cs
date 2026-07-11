using Cards;
using SaveSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Store
{
    public class StoreCardUI : MonoBehaviour
    {
        [SerializeField] private Card cardDisplay; // your existing Card component for illustration/name/etc
        [SerializeField] private TextMeshProUGUI priceText;
        [SerializeField] private GameObject ownedBadge;
        [SerializeField] private Button buyButton;

        private CardData _cardData;
        private CardStore _store;

        public void Setup(CardData cardData, CardStore store)
        {
            _cardData = cardData;
            _store = store;

            cardDisplay.LoadCardData(cardData);
            cardDisplay.SetHoverEnabled(false);
            cardDisplay.SetInteractable(false);

            bool owned = PlayerDataManager.Instance.OwnsCard(cardData.cardId);

            ownedBadge.SetActive(owned);
            priceText.gameObject.SetActive(!owned);
            priceText.transform.parent.gameObject.SetActive(!owned);
            buyButton.gameObject.SetActive(!owned);

            if (!owned)
            {
                priceText.text = cardData.goldCost + " Gold";
                buyButton.onClick.RemoveAllListeners();
                buyButton.onClick.AddListener(() => _store.TryPurchase(_cardData));
            }
        }
    }
}