using System.Collections.Generic;
using Cosmetics;
using Managers;
using SaveSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Store
{
    public class CardBackStore : MonoBehaviour
    {
        [Header("Store Config")]
        [SerializeField] private Transform[] cardBackSlots;
        [SerializeField] private GameObject cardBackItemPrefab;

        [Header("Pagination")]
        [SerializeField] private Button nextPageButton;
        [SerializeField] private Button prevPageButton;

        [Header("Currency Display")]
        [SerializeField] private TextMeshProUGUI goldText;

        private List<CardBackData> _allCardBacks;
        private int _currentPage;
        private int ItemsPerPage => cardBackSlots.Length;
        private int TotalPages => Mathf.CeilToInt((float)_allCardBacks.Count / ItemsPerPage);

        private readonly List<GameObject> _spawnedItems = new List<GameObject>();

        private void Start()
        {
            _allCardBacks = CosmeticsManager.Instance.GetAllCardBacks();
            Debug.Log($"CardBackStore — loaded {_allCardBacks?.Count ?? -1} card backs");
            UpdateGoldDisplay();
            LoadPage(0);
        }

        private void OnEnable()
        {
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
            if (_currentPage < TotalPages - 1)
            {
                _currentPage++;
                LoadPage(_currentPage);
            }
        }

        public void PrevPage()
        {
            if (_currentPage > 0)
            {
                _currentPage--;
                LoadPage(_currentPage);
            }
        }

        private void LoadPage(int page)
        {
            ClearSlots();

            int startIndex = page * ItemsPerPage;

            for (int i = 0; i < ItemsPerPage; i++)
            {
                int itemIndex = startIndex + i;
                if (itemIndex >= _allCardBacks.Count) break;
                SpawnCardBackItem(_allCardBacks[itemIndex], i);
            }

            UpdateButtonStates();
        }

        private void SpawnCardBackItem(CardBackData cardBackData, int slotIndex)
        {
            GameObject item = Instantiate(cardBackItemPrefab, cardBackSlots[slotIndex].position, Quaternion.identity, cardBackSlots[slotIndex]);
            CardBackStoreUI ui = item.GetComponent<CardBackStoreUI>();
            ui.Setup(cardBackData, this);
            _spawnedItems.Add(item);
        }

        public void TryPurchase(CardBackData cardBackData)
        {
            if (CosmeticsManager.Instance.OwnsCardBack(cardBackData.cardBackId)) return;

            bool success = PlayerDataManager.Instance.SpendGold(cardBackData.goldCost);
            if (!success)
            {
                Debug.Log("Not enough gold to purchase " + cardBackData.cardBackName);
                return;
            }

            CosmeticsManager.Instance.UnlockCardBack(cardBackData.cardBackId);
            UpdateGoldDisplay();
            LoadPage(_currentPage);
        }

        public void TryEquip(CardBackData cardBackData)
        {
            if (!CosmeticsManager.Instance.OwnsCardBack(cardBackData.cardBackId)) return;
            CosmeticsManager.Instance.SelectCardBack(cardBackData.cardBackId);
            LoadPage(_currentPage); // refresh so equip/equipped state updates
        }

        public void UpdateGoldDisplay()
        {
            goldText.text = PlayerDataManager.Instance.CurrentData.gold.ToString();
        }

        private void ClearSlots()
        {
            foreach (GameObject item in _spawnedItems) Destroy(item);
            _spawnedItems.Clear();
        }

        private void UpdateButtonStates()
        {
            prevPageButton.interactable = _currentPage > 0;
            nextPageButton.interactable = _currentPage < TotalPages - 1;
        }
    }
}