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
        [SerializeField] private int hoverSortingOrder = 1;
        [SerializeField] private Vector3 newPosition = new Vector3(0, 2f, 0);

        private Vector3 _originalScale;
        private int _originalSortingOrder;
        private Vector3 _originalPosition;
        private static bool _isBeingDragged;
    
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
            _originalPosition = transform.localPosition;
            _originalSortingOrder = _sortingGroup.sortingOrder;
        }
    
        public CardData GetCardData() => _cardData;

        public void LoadCardData(CardData cardData)
        {
            this._cardData = cardData;
            illustrationRender.sprite = cardData.illustration;
            cardNameText.text = cardData.cardName;
            descriptionText.text = cardData.description;
            actionText.text = cardData.actionCost.ToString();
        }

        public void SetInteractable(bool isInteractable) => _cardCollider.enabled = isInteractable;
    
        private void OnDestroy()
        {
            _isBeingDragged = false;
        }

        private void OnMouseEnter()
        {
            transform.localScale = _originalScale * hoverMultiplier;
            transform.localPosition += newPosition;
            _sortingGroup.sortingOrder = hoverSortingOrder;
        }

        private void OnMouseExit()
        {
            if (_isBeingDragged) return;
            transform.localScale = _originalScale;
            transform.localPosition = _originalPosition;
            _sortingGroup.sortingOrder = _originalSortingOrder;
        }

        private void OnMouseDrag()
        {
            _isBeingDragged = true;
            gameObject.transform.position = GetMousePosition();
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
            return Camera.main!.ScreenToWorldPoint(mousePosition);
        }

        private void OnMouseUp()
        {
            _isBeingDragged = false;
            transform.localScale = _originalScale;
            transform.localPosition = _originalPosition;
            _sortingGroup.sortingOrder = _originalSortingOrder;
        }
    }
}
