using System.Collections;
using System.Collections.Generic;
using ScriptableObjects;
using UnityEngine;

namespace MemoryMatch.DataModel
{
    [CreateAssetMenu(fileName = "Levels_", menuName = "Data/LevelsContainer", order = 1)]

    public class LevelsScriptableObject : ScriptableObject
    {
        [SerializeField] private List<LevelDesignerScriptableObject> levels;

        public List<LevelDesignerScriptableObject> Levels => levels;
    }
}