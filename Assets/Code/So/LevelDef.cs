using System;
using UnityEngine;

namespace Kk.BusyEcs
{
    [CreateAssetMenu]
    public class LevelDef : ScriptableObject
    {
        public Dasher player;
        public MobType[] mobs;
        public float countdownSeconds;

        [Serializable]
        public struct MobType
        {
            public int count;
            public Dasher def;
        }
    }
}