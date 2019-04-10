using System;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;
using System.Threading.Tasks;

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

    private List<Shooter> fleeingShooters = new List<Shooter>();
    private List<Shooter> movingShooters = new List<Shooter>();
    private List<Shooter> attackingShooters = new List<Shooter>();

    /*JoinState is called by the shooters to indicate that they want to be part of the state machine*/
    public void JoinState(Shooter shooter, State state)
    {
        shooter.GetComponent<Renderer>().material = StateMaterials.First(m => m.State == state).Material;

        switch(state)
        {
            case State.Fleeing:
                fleeingShooters.Add(shooter);
                break;

            case State.Moving:
                movingShooters.Add(shooter);
                break;

            case State.Attacking:
                attackingShooters.Add(shooter);
                break;

            default:
                break;
        }
    }

    private void Update()
    {
        List<Task> fleeTask = new List<Task>();
        foreach(var fleeingShooter in fleeingShooters)
        {
            fleeTask.Add(Task.Run(() => Flee(fleeingShooter)));
        }

        List<Task> moveTask = new List<Task>();
        foreach (var movingShooter in movingShooters)
        {
            moveTask.Add(Task.Run(() => Move(movingShooter)));
        }

        List<Task> attackTask = new List<Task>();
        foreach (var attackingShooter in attackingShooters)
        {
            attackTask.Add(Task.Run(() => Attack(attackingShooter)));
        }

        Task.WhenAll(fleeTask);
        Task.WhenAll(moveTask);
        Task.WhenAll(attackTask);
    }

    /*TransferState is called by the shooters to indicate that they want to move from one state to another*/
    public void TransferState(Shooter shooter, State state)
    {
        shooter.GetComponent<Renderer>().material = StateMaterials.First(m => m.State == state).Material;

        switch (state)
        {
            case State.Fleeing:
                fleeingShooters.Add(shooter);
                RemoveFromList(shooter, ref movingShooters, ref attackingShooters);
                break;

            case State.Moving:
                movingShooters.Add(shooter);
                RemoveFromList(shooter, ref fleeingShooters, ref attackingShooters);
                break;

            case State.Attacking:
                attackingShooters.Add(shooter);
                RemoveFromList(shooter, ref movingShooters, ref fleeingShooters);
                break;

            default:
                break;
        }
    }

    private void RemoveFromList(Shooter shooter, ref List<Shooter> list1, ref List<Shooter> list2)
    {
        if (list1.IndexOf(shooter) != -1)
        {
            list1.Remove(shooter);
            return;
        }

        if(list2.IndexOf(shooter) != -1)
        {
            list2.Remove(shooter);
            return;
        }
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