using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.UI;

namespace RTS.ECS
{
    public struct SharedUnitData : ISharedComponentData
    {
        public float3 AttackTarget;
        public const float ShotCooldown = 2f;
        public Entity ShotArchetype;
    }

    public struct Unit : IComponentData
    {
        public float Cooldown;
        public State DesiredNextState;
        public State CurrentState;

        public Unit(float cooldown, State currentState = State.Moving, State desiredNextState = State.None)
        {
            Cooldown = cooldown;
            DesiredNextState = desiredNextState;
            CurrentState = currentState;
        }
    }
    
    public struct ECSMovingUnit : IComponentData
    {
        public float Speed;
        public float3 MoveTarget;
    }

    public struct ECSAttackingUnit : IComponentData
    {
        public int ShotsBeforeStateChange;
        public float3 AttackTarget;
    }

    public struct ECSFleeingUnit : IComponentData
    {
        public float RotationSpeed;
        public float Speed;
    }
}