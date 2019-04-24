using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

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
    public static StateMachine Instance { get; private set; }
    
    public StateMaterial[] StateMaterials;
    public GameObject ShotPrefab;
    private List<State> _states;
    private List<Shooter> _shooters;
    private Transform _towerTransform;

    private void Start()
    {
        Instance = this;
        _states = new List<State>();
        _shooters = new List<Shooter>();
        _towerTransform = GameObject.FindGameObjectWithTag("Tower").transform;
        Debug.Log(_towerTransform);
    }

    public void Update()
    {
        for (var i = 0; i < _shooters.Count; i++)
        {
            var entity = _shooters[i];
            var state = _states[i];
            State newState;
            
            switch (state)
            {
                case State.Fleeing:
                    newState = Flee(entity);
                    break;
                case State.Moving:
                    newState = Move(entity);
                    break;
                case State.Attacking:
                    newState = Attack(entity);
                    break;
                default:
                    newState = State.Moving;
                    break;
            }

            if (state != newState)
            {
                _initialiseState(newState, entity);
                _states[i] = newState;
            }
        }
    }

    private void _initialiseState(State state, Shooter shooter)
    {
        shooter.Renderer.material = StateMaterials.First(sm => sm.State == state).Material;

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
                shooter.AttackTarget = _towerTransform;
                break;
        }
    }

    public void JoinState(Shooter entity, State state)
    {
        _initialiseState(state, entity);
        _shooters.Add(entity);
        _states.Add(state);
    }

    private State Attack(Shooter s)
    {
        //Check if the shooter has any shots left, move a little if not
        if (s.ShotsBeforeStateChange == 0)
            return State.Moving;

        //Decrement the cooldown and shoot if ready
        s.Cooldowner -= Time.deltaTime;
        if (!(s.Cooldowner <= 0f)) return State.Attacking;
        
        ShootAt(s, s.AttackTarget);
        s.ShotsBeforeStateChange--;
        s.Cooldowner = s.ShotCooldown;
        return State.Attacking;
    }

    private void ShootAt(Component r, Transform target)
    {
        var targetPosition = target.position;
        var entityPosition = r.transform.position;
        var lookAtTransform = new Vector3(targetPosition.x, entityPosition.y, targetPosition.z);

        var shot = Instantiate(ShotPrefab);
        shot.transform.position = entityPosition;
        shot.transform.LookAt(lookAtTransform, Vector3.up);
    }

    private static State Flee(Shooter s)
    {
        s.Cooldowner -= Time.deltaTime;
        if(s.Cooldowner <= 0f)
            return State.Attacking;

        var stepSize = s.Speed * Time.deltaTime * 2;
        var entityTransform = s.transform;
        entityTransform.Rotate(0, UnityEngine.Random.Range(-s.RotationSpeed, s.RotationSpeed), 0);
        entityTransform.position = entityTransform.position + entityTransform.forward * stepSize;
        return State.Fleeing;
    }

    private static State Move(Shooter s)
    {
        if(Vector3.Distance(s.transform.position, s.MoveTarget) < 0.1f)
            return State.Attacking;

        var entityTransform = s.transform;
        entityTransform.LookAt(s.MoveTarget);
        entityTransform.position = entityTransform.position + (entityTransform.forward * s.Speed * Time.deltaTime);
        return State.Moving;
    }
}