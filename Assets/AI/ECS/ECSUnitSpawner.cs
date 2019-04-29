using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace RTS.ECS
{
    public class ECSUnitSpawner : MonoBehaviour
    {
        public GameObject UnitPrefab;

        private void Start()
        {
            var entityPrefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(UnitPrefab, World.Active);

            var eMan = World.Active.EntityManager;
            var unit = eMan.Instantiate(entityPrefab);
            var t = new float3(1, 1, -2);
            eMan.SetComponentData(unit, new Translation {Value = t});
        }
    }
}