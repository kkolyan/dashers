using UnityEngine;

namespace Kk.BusyEcs
{
    [EcsWorld("events")]
    public struct DashCommand
    {
        public EntityRef actor;
        public Vector3 target;
    }
}