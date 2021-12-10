using System.Reflection;
using UnityEngine;

namespace Kk.BusyEcs
{
    public class DashersStartup : MonoBehaviour
    {
        private IEcsContainer _ecs;
        public LevelDef level;

        private void Start()
        {
            _ecs = new EsContainerBuilder()
                .Scan(Assembly.GetAssembly(typeof(DashersStartup)))
                .End();

            _ecs.GetWorlds().Add(new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem());
            _ecs.GetWorlds().Init();

            _ecs.NewEntity(new LevelLoadCommand { level = level, secondsRemaining = level.countdownSeconds});
            
            _ecs.Execute(new StartPhase());
        }

        private void Update()
        {
            _ecs.Execute(new EarlyUpdatePhase());
            _ecs.Execute(new UpdatePhase());
            _ecs.Execute(new LateUpdatePhase());
            _ecs.Execute(new SuperLateUpdatePhase());
            _ecs.GetWorlds().Run();
        }
    }
}