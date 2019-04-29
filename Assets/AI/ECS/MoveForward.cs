using System;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace RTS.ECS
{
    [Serializable]
    public struct MoveForward : IComponentData
    {
        public float speed;

        public MoveForward(float speed)
        {
            this.speed = speed;
        }
    }

}
