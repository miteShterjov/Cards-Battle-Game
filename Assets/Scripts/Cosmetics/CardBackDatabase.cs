using System.Collections.Generic;
using UnityEngine;

namespace Cosmetics
{
    [CreateAssetMenu(fileName = "CardBackDatabase", menuName = "Scriptable Objects/CardBackDatabase")]
    public class CardBackDatabase : ScriptableObject
    {
        public List<CardBackData> allCardBacks;
        public CardBackData defaultCardBack;
    }
}
