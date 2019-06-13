using System;
using System.Collections.Generic;
using Unity.Jobs;
using UnityEngine;

namespace AI.Job_Systems
{
    public class UpdateStateJobSystem : MonoBehaviour
    {
        private List<Shooter> _shooters;
        public JobHandle FleeJobHandle;
        public JobHandle MoveJobHandle;
        public JobHandle AttackJobHandle;

        public StateMachine StateMachine;

        public static UpdateStateJobSystem Instance { get; private set; }
        
        private void Start()
        {
            if(Instance != null)
                throw new InvalidOperationException("There may only be one instance of the UpdateStateJobSystem.");
            Instance = this;
            
            _shooters = new List<Shooter>();
        }

        public void AddShooter(Shooter shooter)
        {
            _shooters.Add(shooter);
        }

        private void Update()
        {
            /*FleeJobHandle.Complete();
            MoveJobHandle.Complete();
            AttackJobHandle.Complete();*/

            for (var i = 0; i < _shooters.Count; i++)
            {
                var shooter = _shooters[i];
                switch (_shooters[i].State)
                {
                    case State.Fleeing:
                        if(shooter.Cooldowner <= 0)
                            StateMachine.TransferState(shooter, State.Moving);
                        break;
                    case State.Moving:
                        var dist = Vector3.Distance(shooter.transform.position, shooter.MoveTarget);
                        if(dist <= 0.1f)
                            StateMachine.TransferState(shooter, State.Attacking);
                        break;
                    case State.Attacking:
                        if(shooter.ShotsBeforeStateChange == 0)
                            StateMachine.TransferState(shooter, State.Moving);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }
}