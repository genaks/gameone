using ScriptableObjects;

namespace Core.MessageBroker
{
    public class StartGameEvent : IGameEvent
    {
        public LevelDesignerScriptableObject Level { get; }

        public StartGameEvent(LevelDesignerScriptableObject level)
        {
            Level = level;
        }
    }
}