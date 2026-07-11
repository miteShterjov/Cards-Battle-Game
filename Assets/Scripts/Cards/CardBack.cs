using System;
using Cosmetics;
using UnityEngine;
using UnityEngine.Serialization;

namespace Cards
{
    public class CardBack : MonoBehaviour
    {
        private Sprite _cardBackSprite;
        private string _cardBackName;
        private int _goldCost;
        private CardBackData _cardBackData;
        private SpriteRenderer _spriteRenderer;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public CardBackData GetCardBackData() => _cardBackData;

        public void LoadCardBackDataInfo(CardBackData cardBackData)
        {
            _spriteRenderer.sprite = cardBackData.sprite;
            _cardBackName = cardBackData.cardBackName;
            _goldCost = cardBackData.goldCost;
            _cardBackData = cardBackData;
        }
    }
}
