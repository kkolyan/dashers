using UnityEngine;

namespace Kk.BusyEcs
{
    public struct DashAction
    {
        public float speed;
        public Vector3 direction;

        public DashAction(float speed, Vector3 direction)
        {
            this.speed = speed;
            this.direction = direction;
        }
    }
}