using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    private List<Tuple<State, Shooter>> _stateList = new List<Tuple<State, Shooter>>();

    private void Update()
    {
        Execute(State.Attacking, ExecuteShoot);
        Execute(State.Moving, MoveTo);
        Execute(State.Fleeing, Flee);
    }

    private void Execute(State state, Action<Shooter> StateLogic)
    {
        var gosInState = _stateList.Where(t => t.Item1 == state);
        foreach (var go in gosInState)
        {
            //Todo: StateLogic may alter collection, make a work around
            StateLogic(go.Item2);
        }
    }

    public void JoinState(Shooter ranger, State state)
    {
        ranger.GetComponent<Renderer>().material = StateMaterials.First(sm => sm.State == state).Material;

        if(state == State.Moving)
        {
            var newTarget = new Vector3(
                UnityEngine.Random.Range(-3.7f, 5.7f),
                ranger.transform.position.y,
                UnityEngine.Random.Range(-6f, 3.4f));
            ranger.MoveTarget = newTarget;
        }
        else if(state == State.Fleeing)
        {
            ranger.Cooldowner = 4f;
        }
        else if(state == State.Attacking)
        {
            ranger.ShotsBeforeStateChange = 5;
            ranger.Cooldowner = 0f;
            ranger.AttackTarget = GameObject.FindGameObjectWithTag("Tower").transform;
        }

        var f = _stateList.Find(t => t.Item2 == ranger);
        if (f != null)
            _stateList.Remove(f);
        _stateList.Add(new Tuple<State, Shooter>(state, ranger));
    }

    public void ExecuteShoot(Shooter s)
    {
        //Check if the shooter has any shots left, move a little if not
        if (s.ShotsBeforeStateChange == 0)
        {
            JoinState(s, State.Moving);
            return;
        }

        //Decrement the cooldown and shoot if ready
        s.Cooldowner -= Time.deltaTime;
        if (s.Cooldowner <= 0f)
        {
            ShootAt(s, s.AttackTarget);
            s.ShotsBeforeStateChange--;
            s.Cooldowner = s.ShotCooldown;
        }
    }

    public void ShootAt(Shooter r, Transform target)
    {
        var lookAtTransform = new Vector3(target.position.x, r.transform.position.y, target.position.z);

        var shot = Instantiate(ShotPrefab);
        shot.transform.position = r.transform.position;
        shot.transform.LookAt(lookAtTransform, Vector3.up);
    }

    public void Flee(Shooter s)
    {
        s.Cooldowner -= Time.deltaTime;
        if(s.Cooldowner <= 0f)
        {
            JoinState(s, State.Attacking);
            return;
        }

        var stepSize = s.Speed * Time.deltaTime * 2;
        s.transform.Rotate(0, UnityEngine.Random.Range(-s.RotationSpeed, s.RotationSpeed), 0);
        s.transform.position = s.transform.position + s.transform.forward * stepSize;
    }

    public void MoveTo(Shooter s)
    {
        if(Vector3.Distance(s.transform.position, s.MoveTarget) < 0.1f)
        {
            Debug.Log(s.name + " has arrived, attacking");
            JoinState(s, State.Attacking);
            return;
        }

        s.transform.LookAt(s.MoveTarget);
        s.transform.position = s.transform.position + (s.transform.forward * s.Speed * Time.deltaTime);
    }
}