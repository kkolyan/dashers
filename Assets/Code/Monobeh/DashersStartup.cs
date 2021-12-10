using System.Reflection;
using Leopotam.EcsLite;
using UnityEngine;

namespace Kk.BusyEcs
{
    public class DashersStartup : MonoBehaviour
    {
        private IEcsContainer _ecs;
        public LevelDef level;
        private EcsSystems _ecsSystems;

        private void Start()
        {
            _ecsSystems = new EcsSystems(new EcsWorld());

            _ecs = new EcsContainerBuilder()
                .Integrate(_ecsSystems)
                .Scan(Assembly.GetAssembly(typeof(DashersStartup)))
                .End();

            SetupUnityLeoEcsLiteDebugger();
            _ecsSystems.Init();

            _ecs.Execute<Start>();

            _ecs.NewEntity(new LevelLoadCommand { level = level, secondsRemaining = level.countdownSeconds });
        }

        private void Update()
        {
            _ecsSystems.Run();
            _ecs.Execute<EarlyUpdate>();
            _ecs.Execute<Update>();
            _ecs.Execute<LateUpdate>();
            _ecs.Execute<SuperLateUpdate>();
        }

        private void OnDestroy()
        {
            _ecsSystems.Destroy();
        }

        private void SetupUnityLeoEcsLiteDebugger()
        {
            _ecsSystems.Add(new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem());
            foreach (string worldName in _ecsSystems.GetAllNamedWorlds().Keys)
            {
                _ecsSystems.Add(new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem(worldName));
            }
        }
    }
}