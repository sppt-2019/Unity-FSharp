using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Jobs;

namespace AI.Job_Systems
{
    public class FleeJobSystem : MonoBehaviour
    {
        private List<Shooter> _shooters;
        public float ShooterMovingSpeed;
        public float RotatingSpeed;
        public float FleeTime = 5f;
        
        public static FleeJobSystem Instance { get; private set; }

        private JobHandle _handle;
        private TransformAccessArray shooters;
        private NativeArray<float> timers;
        private NativeArray<float> randoms;

        private struct ShooterFleeJob : IJobParallelForTransform
        {
            private float _deltaTime;
            private float _speed;
            private NativeArray<float> _fleeingTimers;
            [ReadOnly] private NativeArray<float> _randoms;

            public ShooterFleeJob(float deltaTime, float speed, NativeArray<float> fleeingTimers, NativeArray<float> randoms)
            {
                _deltaTime = deltaTime;
                _speed = speed;
                _fleeingTimers = fleeingTimers;
                _randoms = randoms;
            }
            
            public void Execute(int index, TransformAccess transform)
            {
                var rotation = _randoms[index];
                transform.rotation *= Quaternion.Euler(Vector3.up * rotation);
                
                var forward = transform.rotation * new Vector3(0, 0, 1);
                transform.position += _deltaTime * _speed * forward;

                var ft = _fleeingTimers[index] - _deltaTime;
                _fleeingTimers[index] = ft;
            }
        }

        private void Start()
        {
            if(Instance != null)
                throw new InvalidOperationException("There may only be one instance of the FleeJobSystem.");
            Instance = this;
            
            _shooters = new List<Shooter>();
        }

        public void StartFleeing(Shooter shooter)
        {
            _shooters.Add(shooter);
            shooter.Cooldowner = FleeTime;
        }

        public void StopFleeing(Shooter shooter)
        {
            _shooters.Remove(shooter);
        }

        private void Update()
        {   
            shooters = new TransformAccessArray(_shooters.Select(s => s.transform).ToArray());
            timers = new NativeArray<float>(_shooters.Select(s => s.Cooldowner).ToArray(), Allocator.TempJob);
            randoms = new NativeArray<float>(_shooters.Select(s => UnityEngine.Random.Range(-RotatingSpeed, RotatingSpeed)).ToArray(), Allocator.TempJob);
            var job = new ShooterFleeJob(Time.deltaTime, ShooterMovingSpeed, timers, randoms);
            
            _handle = job.Schedule(shooters);
            _handle.Complete();

            for (var i = 0; i < timers.Length; i++)
            {
                _shooters[i].Cooldowner = timers[i];
            }
            shooters.Dispose();
            timers.Dispose();
            randoms.Dispose();
        }

        private void OnDestroy()
        {
            _handle.Complete();
            if(shooters.isCreated)
                shooters.Dispose();
            if(timers.IsCreated)
                timers.Dispose();
            if(randoms.IsCreated)
                randoms.Dispose();
        }
    }
}