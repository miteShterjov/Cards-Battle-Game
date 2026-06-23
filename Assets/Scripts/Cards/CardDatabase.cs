using System.Collections.Generic;
using UnityEngine;

namespace Cards
{
    [CreateAssetMenu(fileName = "CardDatabase", menuName = "Scriptable Objects/CardDatabase")]
    public class CardDatabase : ScriptableObject
    {
        public List<CardData> allCards;
    }
}