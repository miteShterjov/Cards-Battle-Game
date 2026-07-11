using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

namespace Cards
{
    public class Card : MonoBehaviour
    {
        [Header("Card Data")]
        [SerializeField] private SpriteRenderer illustrationRender;
        [SerializeField] private TextMeshPro cardNameText;
        [SerializeField] private TextMeshPro descriptionText;
        [SerializeField] private TextMeshPro actionText;

        [Header("Card Visuals")]
        [SerializeField] private float hoverMultiplier = 1.2f;
        [SerializeField] private int hoverSortingOrder = 10;
        [SerializeField] private Vector3 hoverPositionOffset = new Vector3(0, 2f, 0);
        [SerializeField] private GameObject cardBase;

        private Vector3 _originalScale;
        private Vector3 _originalLocalPosition;
        private Quaternion _originalRotation;
        private int _originalSortingOrder;
        private static bool _isBeingDragged;
        private bool _hoverEnabled = true;
        
        private Collider2D _cardCollider;
        private SortingGroup _sortingGroup;
        private CardData _cardData;

        private void Awake()
        {
            _sortingGroup = GetComponent<SortingGroup>();
            _cardCollider = GetComponent<Collider2D>();
        }

        private void Start()
        {
            _originalScale = transform.localScale;
            _originalLocalPosition = transform.localPosition;
            _originalRotation = transform.rotation;
            _originalSortingOrder = _sortingGroup.sortingOrder;
        }

        // called by PlayerHand after every reposition so cached values stay fresh
        public void UpdateOriginalTransform()
        {
            _originalLocalPosition = transform.localPosition;
            _originalRotation = transform.rotation;
            _originalSortingOrder = _sortingGroup.sortingOrder;
        }

        public CardData GetCardData() => _cardData;
        
        public void LoadCardData(CardData cardData)
        {
            _cardData = cardData;
            AdjustCardColor(cardData);
            illustrationRender.sprite = cardData.illustration;
            cardNameText.text = cardData.cardName;
            descriptionText.text = cardData.description;
            actionText.text = cardData.actionCost.ToString();
        }

        public void SetInteractable(bool isInteractable) => _cardCollider.enabled = isInteractable;
        public void SetHoverEnabled(bool value) => _hoverEnabled = value;

        private void AdjustCardColor(CardData cardData)
        {
            SpriteRenderer sp = cardBase.GetComponent<SpriteRenderer>();
            switch (cardData.type)
            {
                case CardType.Attack:
                    sp.color = new Color(1f, 0.1008771f, 0.004716992f);
                    break;
                case CardType.Heal:
                    sp.color = new Color(0.1285843f, 0.764151f, 0f);
                    break;
                case CardType.Defend:
                    sp.color = new Color(0.06094661f, 0.003921568f, 1f);
                    break;
                case CardType.Debuff:
                    sp.color = new Color(1f, 0f, 0.8986384f);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        private void OnDestroy() => _isBeingDragged = false;

        private void OnMouseEnter()
        {
            if (!_hoverEnabled) return;
            if (_isBeingDragged) return;
            transform.localScale = _originalScale * hoverMultiplier;
            transform.localPosition += hoverPositionOffset;
            transform.rotation = Quaternion.identity; // ← straighten
            _sortingGroup.sortingOrder = hoverSortingOrder;
        }

        private void OnMouseExit()
        {
            if (!_hoverEnabled) return;
            if (_isBeingDragged) return;
            transform.localScale = _originalScale;
            transform.localPosition = _originalLocalPosition;
            transform.rotation = _originalRotation; // ← restore tilt
            _sortingGroup.sortingOrder = _originalSortingOrder;
        }

        private void OnMouseDrag()
        {
            if (!_hoverEnabled) return;
            _isBeingDragged = true;
            transform.rotation = Quaternion.identity;
            transform.position = GetMousePosition();
        }

        private void OnMouseUp()
        {
            if (!_hoverEnabled) return;
            _isBeingDragged = false;
            transform.localScale = _originalScale;
            transform.localPosition = _originalLocalPosition;
            transform.rotation = _originalRotation;
            _sortingGroup.sortingOrder = _originalSortingOrder;
        }

        private Vector3 GetMousePosition()
        {
            if (!Camera.main)
            {
                Debug.LogError("Camera is missing.");
                return Vector3.zero;
            }
            Vector3 mousePosition = Mouse.current.position.ReadValue();
            mousePosition.z = transform.position.z - Camera.main.transform.position.z;
            return Camera.main.ScreenToWorldPoint(mousePosition);
        }
    }   
}