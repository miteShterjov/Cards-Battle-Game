using System;
using Cards;
using Cosmetics;
using Managers;
using SaveSystem;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Store
{
    public class CardBackStoreUI : MonoBehaviour
    {
        [Header("Card Back Config")]
        [SerializeField] private SpriteRenderer cardBackPreview; // shows the card back sprite
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI priceText;
        [SerializeField] private GameObject ownedBadge;
        [SerializeField] private GameObject equippedBadge; // shows "EQUIPPED" on the active one
        [SerializeField] private Button buyButton;
        [SerializeField] private Button equipButton;
        [Header("Card Back nonStore Config")]
        [SerializeField] private GameObject nameLabel;
        [SerializeField] private GameObject buyUI;
        

        private CardBackData _cardBackData;
        private CardBackStore _store;

        public void Setup(CardBackData cardBackData, CardBackStore store)
        {
            _cardBackData = cardBackData;
            _store = store;

            cardBackPreview.sprite = cardBackData.sprite;
            nameText.text = cardBackData.cardBackName;

            bool owned = CosmeticsManager.Instance.OwnsCardBack(cardBackData.cardBackId);
            bool equipped = PlayerDataManager.Instance.CurrentData.selectedCardBackId == cardBackData.cardBackId;

            
            bool isInStore = SceneManager.GetActiveScene().name == "GameStore";
    
            nameText.gameObject.SetActive(isInStore);
            buyButton.transform.parent.gameObject.SetActive(isInStore);
            
            
            // buy state
            buyButton.gameObject.SetActive(!owned);
            priceText.gameObject.SetActive(!owned);

            if (!owned)
            {
                priceText.text = cardBackData.goldCost + " Gold";
                buyButton.onClick.RemoveAllListeners();
                buyButton.onClick.AddListener(() => _store.TryPurchase(_cardBackData));
            }

            // owned/equipped state
            equipButton.transform.parent.gameObject.SetActive(owned);
            ownedBadge.SetActive(owned && !equipped);
            equippedBadge.SetActive(equipped);
            equipButton.gameObject.SetActive(owned && !equipped);

            if (owned && !equipped)
            {
                equipButton.onClick.RemoveAllListeners();
                equipButton.onClick.AddListener(() => _store.TryEquip(_cardBackData));
            }
        }
    }
}