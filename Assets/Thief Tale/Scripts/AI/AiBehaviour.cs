using UnityEngine;

using System.IO;

namespace ThiefTale.AI
{
    public static class AiBehaviour
    {
        public enum Parameter : int
        {
            kIsPlayerInSight,
            kIsIdle,
            kSqrDistanceToPlayer,
            kHeardNoise,
            kCount
        }

        public static int[] s_hash = new int[(int)Parameter.kCount];

        public static int GetId(Parameter param)
        {
            return s_hash[(int)param];
        }

        [RuntimeInitializeOnLoadMethod]
        public static void OnGameStart()
        {
            s_hash[0] = Animator.StringToHash("IsPlayerInSight");
            s_hash[1] = Animator.StringToHash("IsIdle");
            s_hash[2] = Animator.StringToHash("SqrDistanceToPlayer");
            s_hash[3] = Animator.StringToHash("HeardNoise");

            Debug.Log("Test");
        }
    }
}
