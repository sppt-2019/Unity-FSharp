using System.Collections;
using System.Collections.Generic;
using RTS.ECS;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace RTS.ECS
{
    public class MoveForwardSystem : JobComponentSystem
    {
        [BurstCompile]
        struct MoveForwardJob : IJobForEach<Translation, Rotation, MoveForward>
        {
            private readonly float _deltaTime;

            public MoveForwardJob(float deltaTime)
            {
                _deltaTime = deltaTime;
            }

            public void Execute(ref Translation trans, [ReadOnly] ref Rotation rotation, [ReadOnly] ref MoveForward mf)
            {
                var forward = math.forward(rotation.Value);
                var step = math.mul(mf.speed, _deltaTime);
                var moveVec = math.mul(forward, new float3(step));
                trans.Value += moveVec;
            }
        }

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            var job = new MoveForwardJob(Time.deltaTime);
            return job.Schedule(this, inputDeps);
        }
    }
}