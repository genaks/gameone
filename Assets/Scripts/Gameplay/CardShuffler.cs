using UnityEngine;
using Random = System.Random;

namespace Gameplay
{
    public class CardShuffler
    {
        public string[] ShuffleCards(string[] cards)
        {
            Random rng = new Random();

            for (int i = cards.Length - 1; i > 0; i--)
            {
                int j = rng.Next(i + 1);
                (cards[i], cards[j]) = (cards[j], cards[i]);
            }
            
            return cards;
        }
    }
}