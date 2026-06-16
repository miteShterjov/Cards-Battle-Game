using System.Collections.Generic;
using UnityEngine;

namespace Cards
{
    [CreateAssetMenu(fileName = "DefaultDeck", menuName = "Scriptable Objects/DefaultDeck")]
    public class DefaultDeck : ScriptableObject
    {
        public List<CardData> cards = new List<CardData>();
    }
}
