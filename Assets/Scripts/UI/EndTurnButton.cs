using Managers;
using UnityEngine;

namespace UI
{
    public class EndTurnButton : MonoBehaviour
    {
        public void OnEndTurnClicked()
        {
            if (TurnSystem.Instance == null) return;
            TurnSystem.Instance.RequestEndPlayerTurn();
        }
    }
}
