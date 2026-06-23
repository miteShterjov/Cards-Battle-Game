using System.Collections.Generic;
using UnityEngine;

namespace Cards
{
    [CreateAssetMenu(fileName = "StarterDeck", menuName = "Scriptable Objects/StarterDeck")]
    public class StarterDeck : ScriptableObject
    {
        public List<CardData> cards = new List<CardData>();
    }
}
