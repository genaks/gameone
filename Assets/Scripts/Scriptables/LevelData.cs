using System;
using System.Collections.Generic;
using ScriptableObjects;

namespace Scriptables
{
    [Serializable]
    public class LevelData
    {
        public int NumberOfColumns { get; }
        public int NumberOfRows { get; }
        public Dictionary<string, bool> Cards { get; }
        public bool SavedGame { get; }
        public int Turns { get; }
        public int Score { get; }
        
        public LevelData(Dictionary<string, bool> cards, bool savedGame, int turns, int score)
        {
            Cards = cards;
            SavedGame = savedGame;
            Turns = turns;
            Score = score;
        }
        
        public LevelData(LevelDesignerScriptableObject levelObject, bool savedGame, int turns, int score)
        {
            SavedGame = savedGame;
            Turns = turns;
            Score = score;
            NumberOfRows = levelObject.NumberOfRows;
            NumberOfColumns = levelObject.NumberOfColumns;
        }
    }
}
