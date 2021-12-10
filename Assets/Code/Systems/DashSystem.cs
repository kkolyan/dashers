using UnityEngine;

namespace Kk.BusyEcs
{
    [EcsSystem]
    public class DashSystem
    {
        [Inject] private IEnv _env;

        [Update]
        public void AimAi(Entity entity, AiMind __, ActionProgress progress)
        {
            if (progress.DoesNothing())
            {
                _env.Query((ref Position enemyPos, ref PlayerMind ___) =>
                {
                    _env.NewEntity(new DashCommand { actor = entity.AsRef(), target = enemyPos.value });
                });
            }
        }

        [Update]
        public void AimPlayer(Entity entity, PlayerMind __, ActionProgress progress)
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

        [Update]
        public void ExecuteDashCommand(Entity commandEntity, DashCommand command)
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

        [LateUpdate]
        public void Progress(Entity entity, ref ActionProgress progress)
        {
            progress.spent += Time.deltaTime;
        }

        [SuperLateUpdate]
        public void ProgressDash(Entity entity, ref ActionProgress progress, ref DashAction dash)
        {
            if (progress.spent > progress.mainDuration)
            {
                entity.Del<DashAction>();
            }
        }

        [Update]
        public void Move(ref Position position, DashAction dash, ActionProgress progress)
        {
            if (progress.spent <= progress.mainDuration)
            {
                position.value += Time.deltaTime * dash.speed * dash.direction;
            }
        }
    }
}