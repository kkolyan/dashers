using System;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Kk.BusyEcs
{
    [EcsSystemClass]
    public class StartupSystem
    {
        [Inject] private IEnv _env = default;

        [EcsSystem]
        public void LoadLevel(EarlyUpdatePhase _, Entity entity, LevelLoadCommand command)
        {
            if (command.secondsRemaining > 0)
            {
                command.secondsRemaining -= Time.deltaTime;
            }

            if (!Camera.main.orthographic)
            {
                throw new Exception("spawn depends on orthographic camera props");
            }
            
            foreach (LevelDef.MobType mobType in command.level.mobs)
            {
                for (int i = 0; i < mobType.count; i++)
                {
                    SpawnDasher(mobType.def, player: false, position: Random.insideUnitCircle * Camera.main.orthographicSize * 2);
                }
            }

            SpawnDasher(command.level.player, player: true, position: Vector3.zero);
            
            entity.DelEntity();
        }

        private void SpawnDasher(Dasher dasher, bool player, Vector3 position)
        {
            GameObject spawn = Object.Instantiate(dasher.prefab);
            Entity mob = _env.NewEntity(
                new Go { value = spawn.gameObject },
                new Position { value = position },
                new Health { value = 100 },
                new DasherSo { value = dasher }
            );
            mob.Add(new ActionProgress());
            if (player)
            {
                mob.Add(new PlayerMind());
            }
            else
            {
                mob.Add(new AiMind());
            }

            spawn.gameObject.AddComponent<EcsEntityLink>().@ref = mob.AsRef();
        }
    }
}