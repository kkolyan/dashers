using UnityEngine;

namespace Kk.BusyEcs
{
    [EcsSystem]
    public class MobVisualizeSystem
    {
        [LateUpdate]
        public void Visualize(Entity entity, Go go, Position position, DasherSo def, ActionProgress progress)
        {
            go.value.transform.localPosition = position.value;

            bool dashing = entity.Match((ref DashAction dash) =>
            {
                go.value.GetComponent<Renderer>().material.color = def.value.dash.color;
            });
            if (!dashing)
            {
                if (progress.DoesNothing())
                {
                    go.value.GetComponent<Renderer>().material.color = def.value.standby;
                }
                else
                {
                    go.value.GetComponent<Renderer>().material.color = def.value.recovering;
                }
            }

        }
    }
}