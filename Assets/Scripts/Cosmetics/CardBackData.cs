using UnityEngine;

namespace Cosmetics
{
    [CreateAssetMenu(fileName = "CardBackData", menuName = "Scriptable Objects/CardBackData")]
    public class CardBackData : ScriptableObject
    {
        public string cardBackId;
        public string cardBackName;
        public Sprite sprite;
        public int goldCost;

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (string.IsNullOrEmpty(cardBackId) && !string.IsNullOrEmpty(cardBackName))
                cardBackId = cardBackName.ToLower().Replace(" ", "_");
        }
#endif
    }
}
