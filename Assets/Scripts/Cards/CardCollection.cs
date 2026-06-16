using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Cards
{
    public class CardCollection : MonoBehaviour
    {
        [Header("Card Collection Config")]
        [SerializeField] private List<CardData> availableCards;
        [SerializeField] private Transform[] cardSlots;
        [SerializeField] private GameObject cardPrefab;

        [Header("Pagination")]
        [SerializeField] private Button nextPageButton;
        [SerializeField] private Button prevPageButton;

        private int _currentPage;
        private int CardsPerPage => cardSlots.Length;
        private int TotalPages => Mathf.CeilToInt((float)availableCards.Count / CardsPerPage);

        private readonly List<GameObject> _spawnedCards = new List<GameObject>();

        private void Start() => LoadPage(0);

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

        private void NextPage()
        {
            if (_currentPage < TotalPages - 1)
            {
                _currentPage++;
                LoadPage(_currentPage);
            }
        }

        private void PrevPage()
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

            int startIndex = page * CardsPerPage;

            for (int i = 0; i < CardsPerPage; i++)
            {
                int cardIndex = startIndex + i;
                if (cardIndex >= availableCards.Count) break; // last page might not be full
                SpawnCard(cardIndex, i);
            }

            UpdateButtonStates();
        }

        private void SpawnCard(int cardIndex, int slotIndex)
        {
            GameObject card = Instantiate(cardPrefab, cardSlots[slotIndex].position, Quaternion.identity, cardSlots[slotIndex]);
            card.GetComponent<Card>().LoadCardData(availableCards[cardIndex]);
            _spawnedCards.Add(card);
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



















