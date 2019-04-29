using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.UI;

namespace RTS.ECS
{
    //[UpdateInGroup(typeof(SimulationSystemGroup))]
    public class ECSUnitSystem : JobComponentSystem
    {
        private BeginInitializationEntityCommandBufferSystem _entityCommandBufferSystem;
        private Unity.Mathematics.Random _random;
        
        struct ChangeStateJob : IJobForEachWithEntity<Unit>
        {
            [ReadOnly] private EntityCommandBuffer _entityCommandBuffer;
            [ReadOnly] private Unity.Mathematics.Random _random;

            public ChangeStateJob([ReadOnly] EntityCommandBuffer entityCommandBuffer)
            {
                _entityCommandBuffer = entityCommandBuffer;
                _random = new Unity.Mathematics.Random((uint) DateTime.Now.Ticks);
            }

            public void Execute(Entity entity, int index, ref Unit unit)
            {
                if (unit.DesiredNextState == State.None) return;
                
                _entityCommandBuffer.RemoveComponent(entity, GetStateComponentType(unit.CurrentState));
                
                switch (unit.DesiredNextState)
                {
                    case State.None:
                        throw new ArgumentOutOfRangeException(nameof(unit.DesiredNextState), unit.DesiredNextState, 
                            "state was State.None, which is not associated with a state");
                    case State.Fleeing:
                        var f = new ECSFleeingUnit
                        {
                            RotationSpeed = 25,
                            Speed = 2
                        };
                        _entityCommandBuffer.AddComponent(entity, f);
                        break;
                    case State.Moving:
                        var m = new ECSMovingUnit
                        {
                            Speed = 1,
                            MoveTarget = new float3(
                                _random.NextFloat(-3.7f, 5.7f),
                                1,
                                _random.NextFloat(-6f, 3.4f))
                        };
                        _entityCommandBuffer.AddComponent(entity, m);
                        break;
                    case State.Attacking:
                        var shared =
                            World.Active.EntityManager.GetSharedComponentData<SharedUnitData>(entity);
                        
                        var a = new ECSAttackingUnit
                        {
                            AttackTarget = shared.AttackTarget,
                            ShotsBeforeStateChange = 5
                        };
                        _entityCommandBuffer.AddComponent(entity, a);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(unit.DesiredNextState), unit.DesiredNextState, 
                            "state was State.None, which is not associated with a state");
                }

                unit.CurrentState = unit.DesiredNextState;
                unit.DesiredNextState = State.None;
            }

            private ComponentType GetStateComponentType(State state)
            {
                switch (state)
                {
                    case State.None:
                        throw new ArgumentOutOfRangeException(nameof(state), state, 
                            "state was State.None, which is not associated with a state");
                    case State.Fleeing:
                        return typeof(ECSFleeingUnit);
                    case State.Moving:
                        return typeof(ECSMovingUnit);
                    case State.Attacking:
                        return typeof(ECSAttackingUnit);
                    default:
                        throw new ArgumentOutOfRangeException(nameof(state), state, 
                            "state was State.None, which is not associated with a state");
                }
            }
        }

        protected override void OnCreate()
        {
            base.OnCreate();
            _entityCommandBufferSystem = World.GetOrCreateSystem<BeginInitializationEntityCommandBufferSystem>();
            World.CreateSystem<MoveForwardSystem>();
        }

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            var ecb = _entityCommandBufferSystem.CreateCommandBuffer();
            
            var move = new MoveUnitJob(Time.deltaTime);
            var flee = new FleeingUnitJob(Time.deltaTime);
            var attack = new AttackingUnitJob(Time.deltaTime, ecb);
            
            var mh = move.Schedule(this, inputDeps);
            var fh = flee.Schedule(this, mh);
            var ah = attack.Schedule(this, fh);

            var updateUnitStateJob = new ChangeStateJob(ecb);
            var dep = updateUnitStateJob.Schedule(this, ah);
            
            _entityCommandBufferSystem.AddJobHandleForProducer(dep);

            return dep;
        }
    }
}