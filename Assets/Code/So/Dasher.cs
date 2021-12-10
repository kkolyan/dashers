using System;
using UnityEngine;

namespace Kk.BusyEcs
{
    [CreateAssetMenu]
    public class Dasher: ScriptableObject
    {
        public GameObject prefab;
        public Dash dash;
        public Color standby;
        public Color recovering;

        [Serializable]
        public struct Dash
        {
            public Color color;
            public float damage;
            public float cooldown;
            public float duration;
            public float distance;
        }
    }
}