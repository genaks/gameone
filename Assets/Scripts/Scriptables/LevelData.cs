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
        public List<string> sprites;

        public LevelData()
        {
            
        }
        
        public LevelData(LevelDesignerScriptableObject levelObject)
        {
            numberOfRows = levelObject.NumberOfRows;
            numberOfColumns = levelObject.NumberOfColumns;
            sprites = levelObject.Sprites;
        }
    }
}
