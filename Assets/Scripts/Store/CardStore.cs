using System.Collections.Generic;
using Cards;
using SaveSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Store
{
    public class CardStore : MonoBehaviour
    {
        [Header("Store Config")]
        [SerializeField] private CardDatabase cardDatabase;
        [SerializeField] private Transform[] cardSlots;
        [SerializeField] private GameObject storeCardPrefab; // a Card prefab variant with a buy button + price text

        [Header("Pagination")]
        [SerializeField] private Button nextPageButton;
        [SerializeField] private Button prevPageButton;

        [Header("Currency Display")]
        [SerializeField] private TextMeshProUGUI goldText;

        private int _currentPage;
        private int CardsPerPage => cardSlots.Length;
        private int TotalPages => Mathf.CeilToInt((float)cardDatabase.allCards.Count / CardsPerPage);

        private readonly List<GameObject> _spawnedCards = new List<GameObject>();

        private void Start()
        {
            UpdateGoldDisplay();
            LoadPage(0);
        }

        private void OnEnable()
        {
            UpdateGoldDisplay();
            LoadPage(0);
            
            nextPageButton.onClick.AddListener(NextPage);
            prevPageButton.onClick.AddListener(PrevPage);
        }

        private void OnDisable()
        {
            nextPageButton.onClick.RemoveListener(NextPage);
            prevPageButton.onClick.RemoveListener(PrevPage);
        }

        public void NextPage()
        {
            Debug.Log($"NextPage called, currentPage: {_currentPage}, TotalPages: {TotalPages}, CardsPerPage: {CardsPerPage}, TotalCards: {cardDatabase.allCards.Count}");
            if (_currentPage < TotalPages - 1)
            {
                _currentPage++;
                LoadPage(_currentPage);
            }
        }

        public void PrevPage()
        {
            Debug.Log($"PrevPage called, currentPage: {_currentPage}, TotalPages: {TotalPages}, CardsPerPage: {CardsPerPage}, TotalCards: {cardDatabase.allCards.Count}");
            if (_currentPage > 0)
            {
                _currentPage--;
                LoadPage(_currentPage);
            }
        }

        private void LoadPage(int page)
        {
            print("Load page method called");
            ClearSlots();

            int startIndex = page * CardsPerPage;

            for (int i = 0; i < CardsPerPage; i++)
            {
                int cardIndex = startIndex + i;
                if (cardIndex >= cardDatabase.allCards.Count) break;
                SpawnStoreCard(cardDatabase.allCards[cardIndex], i);
            }

            UpdateButtonStates();
        }

        private void SpawnStoreCard(CardData cardData, int slotIndex)
        {
            GameObject cardObj = Instantiate(storeCardPrefab, cardSlots[slotIndex].position, Quaternion.identity, cardSlots[slotIndex]);
            StoreCardUI storeCardUI = cardObj.GetComponent<StoreCardUI>();
            storeCardUI.Setup(cardData, this);
            _spawnedCards.Add(cardObj);
        }

        public void TryPurchase(CardData cardData)
        {
            if (PlayerDataManager.Instance.OwnsCard(cardData.cardId)) return; // already owned, shouldn't happen if UI is correct

            bool success = PlayerDataManager.Instance.SpendGold(cardData.goldCost);
            if (!success)
            {
                Debug.Log("Not enough gold to purchase " + cardData.cardName);
                return;
            }

            PlayerDataManager.Instance.UnlockCard(cardData.cardId);
            UpdateGoldDisplay();
            LoadPage(_currentPage); // refresh current page so the bought card now shows "Owned"
        }

        private void UpdateGoldDisplay()
        {
            goldText.text = PlayerDataManager.Instance.CurrentData.gold.ToString();
        }

        private void ClearSlots()
        {
            foreach (GameObject card in _spawnedCards) Destroy(card);
            _spawnedCards.Clear();
        }

        private void UpdateButtonStates()
        {
            prevPageButton.interactable = _currentPage > 0;
            nextPageButton.interactable = _currentPage < TotalPages - 1;
        }
    }
}