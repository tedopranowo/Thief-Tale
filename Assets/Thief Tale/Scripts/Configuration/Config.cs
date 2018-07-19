//Config.cs
//Created by: Tedo Pranowo (tedokdr@yahoo.com)
namespace ThiefTale
{
    namespace AI
    {
        public static class Config
        {
            public const float kUpdatePathDistanceLimit = 0.1f;
            public const float kSplineTimeIncreaseInterval = 0.1f;
            public const float kPathUpdateInterval = 0.1f;

            public const string kAiBehaviourAssetPath = "Assets/Thief Tale/ScriptableObjects/AI/Behaviours/NewAiBehaviour.asset";
            public const string kAiBehaviourIDManagerAssetPath = "Assets/Thief Tale/ScriptableObjects/AI/BehaviourIDManager.asset";
        }
    }

    namespace TTEditor
    {
        public static class Config
        {
            public const float kButtonPickSize = 0.06f;
        }
    }

}

