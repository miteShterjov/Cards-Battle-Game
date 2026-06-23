using System.Collections;
using Cards;
using Events;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Managers
{
    public class TurnSystem : Singleton<TurnSystem>
    {
        private enum TurnState
        {
            PlayerTurn,
            EnemyTurn
        }

        [Header("Turn System Config")] 
        [SerializeField] private float turnTime = 3f;

        [Header("Turn Action System")] 
        [SerializeField] private int startingActionPoints = 2;
        [SerializeField] private int actionPointCap = 7;
        [SerializeField] private TextMeshProUGUI actionPointsText;
        [SerializeField] private int drawCardCost = 1;
        [SerializeField] private int reshuffleCost = 3;
        [SerializeField] private Transform actionPointsUI;
        [SerializeField] private Image actionPointsImagePrefab;
        [SerializeField] private TextMeshProUGUI turnText;
        [SerializeField] private TextMeshProUGUI timeText;

        private TurnState _currentTurnState = TurnState.PlayerTurn;
        private int _currentMaxActions;
        private int _actionsRemaining;
        private GameObject[] _actionPoints;
        private bool _isTransitioning;

        private void Start()
        {
            CreateActionPointUI();
            StartPlayerTurn();
        }

        private void OnEnable()
        {
            PlayerEvents.OnCardPlayed += CardPlayed;
            PlayerEvents.OnDrawCardSucceeded += DrawSucceeded;
            PlayerEvents.OnReshuffleRequested += ReshuffleRequested;
        }

        private void OnDisable()
        {
            PlayerEvents.OnCardPlayed -= CardPlayed;
            PlayerEvents.OnDrawCardSucceeded -= DrawSucceeded;
            PlayerEvents.OnReshuffleRequested -= ReshuffleRequested;
        }

        public bool HasActionsRemaining() => _actionsRemaining > 0;
        public bool CanReshuffleDiscardPile() => _actionsRemaining >= reshuffleCost;
        public bool CanPlayCard(CardData cardData) => cardData.actionCost <= _actionsRemaining;

        public void RequestEndPlayerTurn()
        {
            if (_isTransitioning) return;
            if (_currentTurnState != TurnState.PlayerTurn) return;
            if (!GameManager.Instance.IsGameActive) return;
            EndPlayerTurn();
        }

        private void StartPlayerTurn()
        {
            _currentTurnState = TurnState.PlayerTurn;

            _currentMaxActions = _currentMaxActions == 0 ? startingActionPoints : // first turn only
                Mathf.Min(_currentMaxActions + 1, actionPointCap);

            _actionsRemaining = _currentMaxActions;
            _isTransitioning = false;
            UpdateActionPointsUI();
            UpdateTurnTextIsPlayerTurn(true);
            TurnEvents.PlayerTurnStart();
        }

        private void EndPlayerTurn()
        {
            _isTransitioning = true;
            TurnEvents.PlayerTurnEnds();
            StartCoroutine(WaitBetweenTurns());
        }

        private void StartEnemyTurn()
        {
            _currentTurnState = TurnState.EnemyTurn;
            EnemyTurnStarts();
        }

        private void EndEnemyTurn()
        {
            TurnEvents.EnemyTurnEnds();
            StartCoroutine(WaitBetweenTurns());
        }

        private IEnumerator WaitBetweenTurns()
        {
            for (int i = (int)turnTime; i > 0; i--)
            {
                timeText.text = "turn ends in: " + i;
                yield return new WaitForSeconds(1f);
            }
            if (!GameManager.Instance.IsGameActive) yield break;
            if (_currentTurnState == TurnState.EnemyTurn)
            {
                StartPlayerTurn();
            }
            else
            {
                StartEnemyTurn();
            }
        }

        private void CardPlayed(CardData cardData)
        {
            ConsumeAction(cardData.actionCost);
        }

        private void DrawSucceeded() => ConsumeAction(drawCardCost);

        private void ConsumeAction(int amount)
        {
            _actionsRemaining -= amount;
            UpdateActionPointsUI();
            if (_actionsRemaining <= 0)
            {
                EndPlayerTurn();
            }
        }

        private void EnemyTurnStarts()
        {
            TurnEvents.EnemyTurnStart();
            UpdateTurnTextIsPlayerTurn(false);
            EndEnemyTurn();
        }

        private void ReshuffleRequested()
        {
            ConsumeAction(reshuffleCost);
        }

        private void CreateActionPointUI()
        {
            _actionPoints = new GameObject[actionPointCap];
            for (int i = 0; i < actionPointCap; i++)
            {
                Image actionPointImage = Instantiate(actionPointsImagePrefab, actionPointsUI);
                _actionPoints[i] = actionPointImage.gameObject;
                _actionPoints[i].SetActive(false);
            }
        }

        private void UpdateActionPointsUI()
        {
            for (int i = 0; i < actionPointCap; i++)
            {
                bool isUnlocked = i < _currentMaxActions;
                bool isFilled = i < _actionsRemaining;
                _actionPoints[i].SetActive(isUnlocked && isFilled);
            }
        }

        private void UpdateTurnTextIsPlayerTurn(bool isPlayerTurn) =>
            turnText.text = isPlayerTurn ? "Player Turn" : "Enemy Turn";
    }
}