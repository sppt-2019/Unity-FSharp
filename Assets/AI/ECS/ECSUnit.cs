using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.Serialization;

namespace RTS.ECS
{
    [RequiresEntityConversion]
    public class ECSUnit : MonoBehaviour, IConvertGameObjectToEntity
    {
        [FormerlySerializedAs("EcsShotPrefab")] [SerializeField] private GameObject ecsShotPrefab;
        public float Speed;
        private Entity _shotPrefab;
        private SharedUnitData _sharedUnitData;

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            if (_shotPrefab == default(Entity))
            {
                _shotPrefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(ecsShotPrefab, World.Active);
                _sharedUnitData = new SharedUnitData
                {
                    AttackTarget = GameObject.FindGameObjectWithTag("Tower").transform.position,
                    ShotArchetype = _shotPrefab
                };
            }
            
            var moveUnit = new ECSMovingUnit
            {
                Speed = Speed,
                MoveTarget = new float3(
                    UnityEngine.Random.Range(-3.7f, 5.7f),
                    1,
                    UnityEngine.Random.Range(-6f, 3.4f))
            };
            var unit = new Unit(0);
            
            dstManager.AddComponentData(entity, moveUnit);
            dstManager.AddComponentData(entity, unit);
            dstManager.AddSharedComponentData(entity, _sharedUnitData);
        }
    }
}