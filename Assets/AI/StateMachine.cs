using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public enum State
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
    private List<Shooter> _shooters;
    public Transform AttackTarget;

    private void Start()
    {
        _shooters = new List<Shooter>();
    }

    public void Update()
    {
        for (var i = 0; i < _shooters.Count; i++)
        {
            State newState;
            var shooter = _shooters[i];
            switch (shooter.state)
            {
                case State.Fleeing:
                    newState = Flee(shooter);
                    break;
                case State.Moving:
                    newState = Move(shooter);
                    break;
                case State.Attacking:
                    newState = Attack(shooter);
                    break;
                default: 
                    newState = shooter.state;
                    break;
            }
            
            if(newState != shooter.state)
                TransferState(shooter, newState);
        }
    }

    private void _initialiseState(State state, Shooter shooter)
    {
        shooter.GetComponent<Renderer>().material = StateMaterials.First(sm => sm.State == state).Material;

        switch (state)
        {
            case State.Moving:
                var newTarget = new Vector3(
                    UnityEngine.Random.Range(-3.7f, 5.7f),
                    shooter.transform.position.y,
                    UnityEngine.Random.Range(-6f, 3.4f));
                shooter.MoveTarget = newTarget;
                break;
            case State.Fleeing:
                shooter.Cooldowner = 4f;
                break;
            case State.Attacking:
                shooter.ShotsBeforeStateChange = 5;
                shooter.Cooldowner = 0f;
                shooter.AttackTarget = AttackTarget;
                break;
        }
    }

    public void TransferState(Shooter shooter, State state)
    {
        _initialiseState(state, shooter);
        shooter.state = state;
    }

    public void JoinState(Shooter entity, State state)
    {
        _initialiseState(state, entity);
        entity.state = state;
        _shooters.Add(entity);
    }

    public State Attack(Shooter s)
    {
        //Check if the shooter has any shots left, move a little if not
        if (s.ShotsBeforeStateChange == 0)
            return State.Moving;

        //Decrement the cooldown and shoot if ready
        s.Cooldowner -= Time.deltaTime;
        if (s.Cooldowner <= 0f)
        {
            ShootAt(s, s.AttackTarget);
            s.ShotsBeforeStateChange--;
            s.Cooldowner = s.ShotCooldown;
        }
        return State.Attacking;
    }

    public void ShootAt(Shooter r, Transform target)
    {
        var shooterPosition = r.transform.position;
        var targetPosition = target.position;
        var lookAtTransform = new Vector3(targetPosition.x, shooterPosition.y, targetPosition.z);

        var shot = Instantiate(ShotPrefab);
        shot.transform.position = shooterPosition;
        shot.transform.LookAt(lookAtTransform, Vector3.up);
    }

    public State Flee(Shooter s)
    {
        s.Cooldowner -= Time.deltaTime;
        if(s.Cooldowner <= 0f)
            return State.Attacking;

        var stepSize = s.Speed * Time.deltaTime * 2;
        var shooterTrans = s.transform;
        shooterTrans.Rotate(0, UnityEngine.Random.Range(-s.RotationSpeed, s.RotationSpeed), 0);
        shooterTrans.position = shooterTrans.position + shooterTrans.forward * stepSize;
        return State.Fleeing;
    }

    public State Move(Shooter s)
    {
        if(Vector3.Distance(s.transform.position, s.MoveTarget) < 0.1f)
            return State.Attacking;

        var shooterTrans = s.transform;
        shooterTrans.LookAt(s.MoveTarget);
        shooterTrans.position = shooterTrans.position + (s.Speed * Time.deltaTime * shooterTrans.forward);
        return State.Moving;
    }
}