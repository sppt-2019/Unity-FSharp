using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace RTS.ECS
{
    [RequiresEntityConversion]
    public class ShotECS : MonoBehaviour, IConvertGameObjectToEntity
    {
        public float Speed = 5;
        
        // The MonoBehaviour data is converted to ComponentData on the entity.
        // We are specifically transforming from a good editor representation of the data (Represented in degrees)
        // To a good runtime representation (Represented in radians)
        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            var data = new MoveForward(Speed);
            dstManager.AddComponentData(entity, data);
        }
    }
}