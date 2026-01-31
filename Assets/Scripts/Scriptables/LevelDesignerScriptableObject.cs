using System.Collections.Generic;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "Level_", menuName = "Data/Level", order = 0)]

    public class LevelDesignerScriptableObject : ScriptableObject
    {
        [SerializeField] private string levelID;
        
        [SerializeField] private string levelName;

        [SerializeField] private int numberOfColumns;
        [SerializeField] private int numberOfRows;
        [SerializeField] private List<string> sprites;


        public string LevelID => levelID;
        public string LevelName => levelName;
        public int NumberOfColumns => numberOfColumns;
        public int NumberOfRows => numberOfRows;
        public List<string> Sprites => sprites;
    }
}