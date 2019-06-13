using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Jobs;

namespace AI.Job_Systems
{
    public class MovingJobSytem : MonoBehaviour
    {
        public float ShooterMoveSpeed;
        
        private List<Shooter> _shooters;
        private JobHandle _handle;
        private NativeArray<Vector3> targets;
        private TransformAccessArray transforms;

        public static MovingJobSytem Instance { get; private set; }

        private struct MoveShooterJob : IJobParallelForTransform
        {
            private float _deltaTime;
            private float _moveSpeed;
            [ReadOnly] private NativeArray<Vector3> _moveTargets;

            public MoveShooterJob(float deltaTime, float moveSpeed, NativeArray<Vector3> moveTargets)
            {
                _deltaTime = deltaTime;
                _moveSpeed = moveSpeed;
                _moveTargets = moveTargets;
            }
            
            public void Execute(int index, TransformAccess transform)
            {
                var target = _moveTargets[index];
                var dirVect = (target - transform.position).normalized;

                transform.position += _deltaTime * _moveSpeed * dirVect;
            }
        }
        
        private void Start()
        {
            if(Instance != null)
                throw new InvalidOperationException("There may only be one instance of the MovingHobSystem.");
            Instance = this;
            
            _shooters = new List<Shooter>();
        }

        public void StartMoving(Shooter shooter, Vector3 target)
        {
            _shooters.Add(shooter);
            shooter.MoveTarget = target;
        }

        public void StopMoving(Shooter shooter)
        {
            var index = _shooters.FindIndex(s => s == shooter);
            _shooters.RemoveAt(index);
            shooter.MoveTarget = Vector3.negativeInfinity;
        }

        private void OnDestroy()
        {
            _handle.Complete();
            targets.Dispose();
            transforms.Dispose();
        }

        private void Update()
        {
            _handle.Complete();
            if (targets.IsCreated)
            {
                targets.Dispose();
                transforms.Dispose();
            }

            targets = new NativeArray<Vector3>(_shooters.Select(s => s.MoveTarget).ToArray(), Allocator.TempJob);
            transforms = new TransformAccessArray(_shooters.Select(s => s.transform).ToArray());
            
            var job = new MoveShooterJob(Time.deltaTime, ShooterMoveSpeed, targets);
            _handle = job.Schedule(transforms);
        }
    }
}