using System.Collections.Generic;
using Cards;
using Events;
using Managers;
using UnityEngine;

namespace UI
{
    public class DeckUI : MonoBehaviour
    {
        [SerializeField] private GameObject cardTabPrefab;
        
        private readonly List<GameObject> _cardTabGameObjects = new List<GameObject>();
        private const float VerticalSpacing = 0.6f;

        private void Start()
        {
            if (DeckManager.Instance != null) BuildTheUI();
        }

        private void OnEnable()
        {
            DeckEvents.OnDeckProcessed += BuildTheUI;
        }

        private void OnDisable()
        {
            DeckEvents.OnDeckProcessed -= BuildTheUI;
        }

        private void BuildTheUI()
        {
            foreach (GameObject cardTab in _cardTabGameObjects) Destroy(cardTab);
            
            _cardTabGameObjects.Clear();
            List<CardData> deck = DeckManager.Instance.GetDeck();
            
            for (int i = 0; i < deck.Count; i++)
            {
                GameObject cardTab = Instantiate(cardTabPrefab, transform);
                cardTab.GetComponent<CardTab>().LoadCardTabData(deck[i]);

                // if (cardTab.GetComponent<CardTab>().GetCardData().type == CardType.Defend)
                // {
                //     cardTab.GetComponent<CardData>().illustration.
                //         GameObject().transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                // }
                    
                cardTab.transform.localPosition = new Vector3(0, -i * VerticalSpacing, 0);
                _cardTabGameObjects.Add(cardTab);
            }
        }
    }
}