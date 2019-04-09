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
public struct StateMaterial
{
    public State State;
    public Material Material;
}

class StateMachine : MonoBehaviour
{
    public StateMaterial[] StateMaterials;
    public GameObject ShotPrefab;
    private List<(State state, Shooter entity)> _stateList;

    private void Start()
    {
        _stateList = new List<(State state, Shooter entity)>();
    }

    public void Update()
    {
        var newStates = _stateList.Select(s =>
        {
            switch (s.state)
            {
                case State.Fleeing:
                    return Flee(s.entity);
                case State.Moving:
                    return Move(s.entity);
                case State.Attacking:
                    return Attack(s.entity);
                default: return (State.Moving, s.entity);
            }
        }).ToList();

        foreach (var statePair in newStates.Zip(_stateList, (sNew, sOld) => (sNew,sOld)))
        {
            if (statePair.sNew.Item1 != statePair.sOld.state)
                TransferState(statePair.sNew.entity, statePair.sNew.Item1);
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
                shooter.AttackTarget = GameObject.FindGameObjectWithTag("Tower").transform;
                break;
        }
    }

    public void TransferState(Shooter shooter, State state)
    {
        _initialiseState(state, shooter);
        _stateList = _stateList.Select(s => s.entity == shooter ? (state, shooter) : s).ToList();
    }

    public void JoinState(Shooter entity, State state)
    {
        _initialiseState(state, entity);
        _stateList.Add((state, entity));
    }

    public (State state,Shooter entity) Attack(Shooter s)
    {
        //Check if the shooter has any shots left, move a little if not
        if (s.ShotsBeforeStateChange == 0)
            return (State.Moving, s);

        //Decrement the cooldown and shoot if ready
        s.Cooldowner -= Time.deltaTime;
        if (s.Cooldowner <= 0f)
        {
            ShootAt(s, s.AttackTarget);
            s.ShotsBeforeStateChange--;
            s.Cooldowner = s.ShotCooldown;
        }
        return (State.Attacking, s);
    }

    public void ShootAt(Shooter r, Transform target)
    {
        var lookAtTransform = new Vector3(target.position.x, r.transform.position.y, target.position.z);

        var shot = Instantiate(ShotPrefab);
        shot.transform.position = r.transform.position;
        shot.transform.LookAt(lookAtTransform, Vector3.up);
    }

    public (State state, Shooter entity) Flee(Shooter s)
    {
        s.Cooldowner -= Time.deltaTime;
        if(s.Cooldowner <= 0f)
            return (State.Attacking, s);

        var stepSize = s.Speed * Time.deltaTime * 2;
        s.transform.Rotate(0, UnityEngine.Random.Range(-s.RotationSpeed, s.RotationSpeed), 0);
        s.transform.position = s.transform.position + s.transform.forward * stepSize;
        return (State.Fleeing, s);
    }

    public (State state, Shooter entity) Move(Shooter s)
    {
        if(Vector3.Distance(s.transform.position, s.MoveTarget) < 0.1f)
            return (State.Attacking, s);

        s.transform.LookAt(s.MoveTarget);
        s.transform.position = s.transform.position + (s.transform.forward * s.Speed * Time.deltaTime);
        return (State.Moving, s);
    }
}