using UnityEngine;

namespace Kk.BusyEcs
{
    [EcsSystemClass]
    public class DashSystem
    {
        [Inject] private IEnv _env;

        [EcsSystem]
        public void AimAi(UpdatePhase _, Entity entity, AiMind __, ActionProgress progress)
        {
            if (progress.DoesNothing()) 
            {
                _env.Query((ref Position enemyPos, ref PlayerMind ___) =>
                {
                    _env.NewEntity(new DashCommand { actor = entity.AsRef(), target = enemyPos.value });
                });
            }
        }

        [EcsSystem]
        public void AimPlayer(UpdatePhase _, Entity entity, PlayerMind __, ActionProgress progress)
        {
            if (progress.DoesNothing())
            {
                if (Input.GetMouseButtonDown(0))
                {
                    Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                    Plane gamePlane = new Plane(Vector3.forward, Vector3.zero);
                    if (gamePlane.Raycast(mouseRay, out float ray))
                    {
                        Vector3 targetPoint = mouseRay.GetPoint(ray);
                        _env.NewEntity(new DashCommand { actor = entity.AsRef(), target = targetPoint });
                    }
                }
            }
        }

        [EcsSystem]
        public void ExecuteDashCommand(UpdatePhase _, Entity commandEntity, DashCommand command)
        {
            command.actor.Match((Entity entity, ref DasherSo def, ref Position position, ref ActionProgress progress) =>
            {
                if (progress.DoesNothing())
                {
                    Dasher.Dash dashDef = def.value.dash;
                    
                    entity.Add(new DashAction(
                        speed: dashDef.distance / dashDef.duration,
                        direction: (command.target - position.value).normalized
                    ));
                    progress = new ActionProgress(
                        mainDuration: dashDef.duration, 
                        cooldownDuration: dashDef.cooldown
                    );
                }
            });
            commandEntity.DelEntity();
        }

        [EcsSystem]
        public void Progress(LateUpdatePhase _, Entity entity, ref ActionProgress progress)
        {
            progress.spent += Time.deltaTime;
        }

        [EcsSystem]
        public void ProgressDash(SuperLateUpdatePhase _, Entity entity, ref ActionProgress progress, ref DashAction dash)
        {
            if (progress.spent > progress.mainDuration)
            {
                entity.Del<DashAction>();
            }
        } 

        [EcsSystem]
        public void Move(UpdatePhase _, ref Position position, DashAction dash, ActionProgress progress)
        {
            if (progress.spent <= progress.mainDuration)
            {
                position.value += Time.deltaTime * dash.speed * dash.direction;
            }
        }
    }
}