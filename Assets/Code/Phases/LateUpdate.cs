using System;

namespace Kk.BusyEcs
{
    [EcsPhase]
    [AttributeUsage(AttributeTargets.Method)]
    public class LateUpdate : Attribute { }
}