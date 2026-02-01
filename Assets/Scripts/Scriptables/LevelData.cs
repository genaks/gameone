using System;
using System.Collections.Generic;
using ScriptableObjects;

namespace Scriptables
{
    [Serializable]
    public class LevelData
    {
        public int numberOfColumns;
        public int numberOfRows;
        public Dictionary<string, bool> cards = new ();

        public LevelData()
        {
            
        }
        
        public LevelData(LevelDesignerScriptableObject levelObject)
        {
            numberOfRows = levelObject.NumberOfRows;
            numberOfColumns = levelObject.NumberOfColumns;
            foreach (var sprite in levelObject.Sprites)
            {
                cards[sprite] = false;
            }
        }
    }
}
