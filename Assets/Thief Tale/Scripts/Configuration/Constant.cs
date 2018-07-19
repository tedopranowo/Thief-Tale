//Constant.cs
//Created by: Tedo Pranowo (tedokdr@yahoo.com)
using UnityEngine;

namespace ThiefTale
{
    namespace Constant
    {
        public static class Button
        {
            public const string kHorizontalMovement = "Horizontal";
            public const string kVerticalMovement = "Vertical";
            public const string kJump = "Jump";
            public const string kRun = "Run";
            public const string kInteract = "Interact";
            public const string kFire = "Fire";
            public const string kMenu = "Menu";
        }

        public static class Inventory
        {
            public const int kMaxQuestItemCount = 8;
            public const int kMaxToolCount = 8;
        }

        namespace AnimationParameters
        {
            public static class Character
            {
                public const int kState = 0;
            }

        }
    }
}
