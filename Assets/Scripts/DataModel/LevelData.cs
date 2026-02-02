using System;
using System.Collections.Generic;
using ScriptableObjects;

namespace DataModel
{
    [Serializable]
    public class LevelData
    {
        public int NumberOfColumns { get; set; }
        public int NumberOfRows { get; set; }
        public CardModel[] Cards { get; set; }
        public bool SavedGame { get; set; }
        public int Turns { get; set; }
        public int Score { get; set; }
        
        public LevelData()
        {
            
        }
        
        public LevelData(LevelDesignerScriptableObject levelObject, bool savedGame, int turns, int score)
        {
            SavedGame = savedGame;
            Turns = turns;
            Score = score;
            NumberOfRows = levelObject.NumberOfRows;
            NumberOfColumns = levelObject.NumberOfColumns;
            List<CardModel> cardsList = new List<CardModel>();
            foreach (var sprite in levelObject.Sprites)
            {
                CardModel cardModel = new CardModel(sprite, false);
                cardsList.Add(cardModel);
            }

            Cards = cardsList.ToArray();
        }
        
        public LevelData(LevelData levelData, Dictionary<string, bool> cards, bool savedGame, int turns, int score)
        {
            SavedGame = savedGame;
            Turns = turns;
            Score = score;
            NumberOfRows = levelData.NumberOfRows;
            NumberOfColumns = levelData.NumberOfColumns;
            List<CardModel> cardsList = new List<CardModel>();
            foreach (var cardID in cards.Keys)
            {
                CardModel cardModel = new CardModel(cardID, cards[cardID]);
                cardsList.Add(cardModel);
            }
            
            Cards = cardsList.ToArray();
        }
    }
}
