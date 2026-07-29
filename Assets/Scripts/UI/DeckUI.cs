using System.Collections.Generic;
using Cards;
using Events;
using Managers;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class DeckUI : MonoBehaviour
    {
        [SerializeField] private GameObject cardTabPrefab;
        [SerializeField] private Button nextPageButton;
        [SerializeField] private Button prevPageButton;
        [SerializeField] private int cardsPerPage = 8;

        private readonly List<GameObject> _cardTabGameObjects = new List<GameObject>();
        private List<CardData> _deck = new List<CardData>();
        private const float VerticalSpacing = 0.6f;
        private int _currentPage = 0;

        private int TotalPages => Mathf.CeilToInt((float)_deck.Count / cardsPerPage);

        private void OnEnable()
        {
            DeckEvents.OnDeckProcessed += OnDeckProcessed;
            nextPageButton.onClick.RemoveAllListeners();
            prevPageButton.onClick.RemoveAllListeners();
            nextPageButton.onClick.AddListener(NextPage);
            prevPageButton.onClick.AddListener(PrevPage);
            if (DeckManager.Instance != null) OnDeckProcessed();
        }

        private void OnDisable()
        {
            DeckEvents.OnDeckProcessed -= OnDeckProcessed;
        }

        public void OnGoBackButtonClicked() => SceneFader.Instance.FadeToScene("MainMenu");

        private void OnDeckProcessed()
        {
            _currentPage = 0;
            _deck = DeckManager.Instance.GetDeck();
            BuildTheUI();
        }

        private void NextPage()
        {
            if (_currentPage < TotalPages - 1)
            {
                _currentPage++;
                BuildTheUI();
            }
        }

        private void PrevPage()
        {
            if (_currentPage > 0)
            {
                _currentPage--;
                BuildTheUI();
            }
        }

        private void BuildTheUI()
        {
            foreach (GameObject cardTab in _cardTabGameObjects) Destroy(cardTab);
            _cardTabGameObjects.Clear();

            int startIndex = _currentPage * cardsPerPage;

            for (int i = 0; i < cardsPerPage; i++)
            {
                int cardIndex = startIndex + i;
                if (cardIndex >= _deck.Count) break;

                GameObject cardTab = Instantiate(cardTabPrefab, transform);
                cardTab.GetComponent<CardTab>().LoadCardTabData(_deck[cardIndex]);
                cardTab.transform.localPosition = new Vector3(0, -i * VerticalSpacing, 0);
                _cardTabGameObjects.Add(cardTab);
            }

            UpdateButtonStates();
        }

        private void UpdateButtonStates()
        {
            prevPageButton.interactable = _currentPage > 0;
            nextPageButton.interactable = _currentPage < TotalPages - 1;
        }
    }
}