﻿using System;
using System.Linq;
using UnityEngine;

enum State
{
    Fleeing, Moving, Attacking
}

[Serializable]
struct StateMaterial
{
    public State State;
    public Material Material;
}

class StateMachine : MonoBehaviour
{
    public StateMaterial[] StateMaterials;
    public GameObject ShotPrefab;

    /*JoinState is called by the shooters to indicate that they want to be part of the state machine*/
    public void JoinState(Shooter shooter, State state)
    {
        shooter.GetComponent<Renderer>().material = StateMaterials.First(m => m.State == state).Material;
    }

    /*TransferState is called by the shooters to indicate that they want to move from one state to another*/
    public void TransferState(Shooter shooter, State state)
    {
        shooter.GetComponent<Renderer>().material = StateMaterials.First(m => m.State == state).Material;
    }

    #region State Logic
    public void Attack(Shooter s)
    {
        void ShootAt(Transform target)
        {
            var lookAtTransform = new Vector3(target.position.x, s.transform.position.y, target.position.z);

            var shot = Instantiate(ShotPrefab);
            shot.transform.position = s.transform.position;
            shot.transform.LookAt(lookAtTransform, Vector3.up);
        }
        
        //Check if the shooter has any shots left, move a little if not
        if (s.ShotsBeforeStateChange == 0)
        {
            TransferState(s, State.Moving);
            return;
        }

        //Decrement the cooldown and shoot if ready
        s.Cooldowner -= Time.deltaTime;
        if (s.Cooldowner <= 0f)
        {
            ShootAt(s.AttackTarget);
            s.ShotsBeforeStateChange--;
            s.Cooldowner = s.ShotCooldown;
        }
    }

    public void Flee(Shooter s)
    {
        s.Cooldowner -= Time.deltaTime;
        if(s.Cooldowner <= 0f)
        {
            TransferState(s, State.Attacking);
            return;
        }

        var stepSize = s.Speed * Time.deltaTime * 2;
        s.transform.Rotate(0, UnityEngine.Random.Range(-s.RotationSpeed, s.RotationSpeed), 0);
        s.transform.position = s.transform.position + s.transform.forward * stepSize;
    }

    public void Move(Shooter s)
    {
        if(Vector3.Distance(s.transform.position, s.MoveTarget) < 0.1f)
        {
            Debug.Log(s.name + " has arrived, attacking");
            TransferState(s, State.Attacking);
            return;
        }

        s.transform.LookAt(s.MoveTarget);
        s.transform.position = s.transform.position + (s.transform.forward * s.Speed * Time.deltaTime);
    }
    #endregion
}