using System;
using System.Diagnostics;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

namespace RTS.ECS
{
    [BurstCompile]
    struct MoveUnitJob : IJobForEach<Translation, Unit, ECSMovingUnit>
    {
        private readonly float _deltaTime;

        public MoveUnitJob(float deltaTime)
        {
            _deltaTime = deltaTime;
        }
            
        public void Execute(ref Translation trans, ref Unit baseUnit, [ReadOnly] ref ECSMovingUnit unit)
        {
            if (Vector3.Distance(trans.Value, unit.MoveTarget) < 0.1f)
            {
                baseUnit.DesiredNextState = State.Attacking;
                return;
            }

            var dir = unit.MoveTarget - trans.Value;
            var step = _deltaTime * unit.Speed * dir;
            trans.Value += step;
        }
    }

    [BurstCompile]
    struct FleeingUnitJob : IJobForEach<Translation, Rotation, Unit, ECSFleeingUnit>
    {
        private readonly float _deltaTime;
        [ReadOnly] private Unity.Mathematics.Random _random;

        public FleeingUnitJob(float deltaTime)
        {
            _deltaTime = deltaTime;
            _random = new Unity.Mathematics.Random((uint) DateTime.Now.Ticks);
        }
        
        public void Execute(ref Translation trans, ref Rotation rotation, ref Unit baseUnit, [ReadOnly] ref ECSFleeingUnit unit)
        {
            baseUnit.Cooldown -= _deltaTime;
            if (baseUnit.Cooldown <= 0f)
            {
                baseUnit.DesiredNextState = State.Attacking;
                return;
            }

            //Rotate the unit randomly
            var deltaRotation = _random.NextFloat(-unit.RotationSpeed, unit.RotationSpeed);
            rotation.Value = math.mul(math.normalize(rotation.Value), quaternion.AxisAngle(math.up(), deltaRotation));
            
            //And move forward
            var f = math.forward(rotation.Value);
            var stepSize = math.mul(unit.Speed, _deltaTime);
            trans.Value += f * stepSize;
        }
    }

    struct AttackingUnitJob : IJobForEachWithEntity<Translation, Unit, ECSAttackingUnit>
    {
        private readonly float _deltaTime;
        [ReadOnly] private EntityCommandBuffer _entityCommandBuffer;

        public AttackingUnitJob(float deltaTime, EntityCommandBuffer ecb)
        {
            _deltaTime = deltaTime;
            _entityCommandBuffer = ecb;
        }
        
        public void Execute(Entity entity, int index, ref Translation trans, ref Unit baseUnit, ref ECSAttackingUnit unit)
        {
            if (unit.ShotsBeforeStateChange == 0)
            {
                baseUnit.DesiredNextState = State.Moving;
                return;
            }
            
            var sharedData = World.Active.EntityManager.GetSharedComponentData<SharedUnitData>(entity);

            baseUnit.Cooldown -= _deltaTime;
            if (!(baseUnit.Cooldown <= 0f)) return;
            
            ShootAt(trans, unit, sharedData.ShotArchetype);
            unit.ShotsBeforeStateChange--;
            baseUnit.Cooldown = SharedUnitData.ShotCooldown;
        }

        private void ShootAt(Translation trans, ECSAttackingUnit unit, Entity shotArchetype)
        {
            var e = _entityCommandBuffer.Instantiate(shotArchetype);

            var shotTrans = trans.Value;
            _entityCommandBuffer.SetComponent(e, new Translation { Value =  shotTrans });

            shotTrans.y = 1;
            var shotForward = math.normalize(unit.AttackTarget - shotTrans);
            var rot = quaternion.LookRotation(shotForward, new float3(0, 1, 0));
            _entityCommandBuffer.SetComponent(e, new Rotation { Value = rot });
        }
    }
}