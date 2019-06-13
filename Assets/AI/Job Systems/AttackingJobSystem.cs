using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Jobs;

namespace AI.Job_Systems
{
    public class AttackingJobSystem : MonoBehaviour
    {
        private List<Shooter> _shooters;
        public float TimeBetweenshots;
        public int NumberOfShotsBeforeStateChange;
        public Transform AttackTarget;

        private JobHandle _handle;
        private TransformAccessArray shooters;
        private NativeArray<float> cooldowns;
        private NativeArray<bool> shouldShoot;

        public static AttackingJobSystem Instance { get; private set; }

        private struct AttackJob : IJobParallelForTransform
        {
            private NativeArray<float> _cooldowns;
            private NativeArray<bool> _shouldShoot;
            
            [ReadOnly] private float _deltaTime;
            [ReadOnly] private float _shotCooldown;

            public AttackJob(NativeArray<float> cooldowns, NativeArray<bool> shouldShoot, float deltaTime, float shotCooldown)
            {
                _cooldowns = cooldowns;
                _shouldShoot = shouldShoot;
                _deltaTime = deltaTime;
                _shotCooldown = shotCooldown;
            }
            
            public void Execute(int index, TransformAccess transform)
            {
                var cd = _cooldowns[index] - _deltaTime;
                _cooldowns[index] = cd;
                if(_cooldowns[index] > 0)
                    return;

                _cooldowns[index] = _shotCooldown;
                _shouldShoot[index] = true;
            }
        }

        private void Start()
        {
            if(Instance != null)
                throw new InvalidOperationException("There may only be one instance of the AttackingJobSystem.");
            Instance = this;

            _shooters = new List<Shooter>();
        }

        private void Update()
        {
            shooters = new TransformAccessArray(_shooters.Select(s => s.transform).ToArray());
            cooldowns = new NativeArray<float>(_shooters.Select(s => s.Cooldowner).ToArray(), Allocator.TempJob);
            shouldShoot = new NativeArray<bool>(_shooters.Select(s => false).ToArray(), Allocator.TempJob);
            
            var job = new AttackJob(cooldowns, shouldShoot, Time.deltaTime, TimeBetweenshots);
            _handle = job.Schedule(shooters);
            
            _handle.Complete();
            for (var i = 0; i < cooldowns.Length; i++)
            {
                _shooters[i].Cooldowner = cooldowns[i];
                if (shouldShoot[i])
                {
                    BulletJobSystem.Instance.SpawnBullet(_shooters[i].transform.position, AttackTarget.position);
                    _shooters[i].ShotsBeforeStateChange--;
                }
            }
            
            cooldowns.Dispose();
            shouldShoot.Dispose();
            shooters.Dispose();
        }

        private void OnDestroy()
        {
            _handle.Complete();
            if(shooters.isCreated)
                shooters.Dispose();
            if(cooldowns.IsCreated)
                cooldowns.Dispose();
            if(shouldShoot.IsCreated)
                shouldShoot.Dispose();
        }

        public void StartAttacking(Shooter shooter)
        {
            shooter.Cooldowner = 0;
            shooter.ShotsBeforeStateChange = NumberOfShotsBeforeStateChange;
            _shooters.Add(shooter);
        }

        public void StopAttacking(Shooter shooter)
        {
            _shooters.Remove(shooter);
        }
    }
}