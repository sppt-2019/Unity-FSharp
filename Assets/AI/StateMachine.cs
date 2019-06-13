using System;
using System.Linq;
using AI.Job_Systems;
using UnityEngine;
using Random = System.Random;

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

public class StateMachine : MonoBehaviour
{
    public StateMaterial[] StateMaterials;
    public GameObject ShotPrefab;
    public float ShooterYHeight = 1.1f;
    public Vector2 XBoundary;
    public Vector2 ZBoundary;

    /*JoinState is called by the shooters to indicate that they want to be part of the state machine*/
    public void JoinState(Shooter shooter, State state)
    {
        shooter.GetComponent<Renderer>().material = StateMaterials.First(m => m.State == state).Material;
        shooter.State = state;
        UpdateStateJobSystem.Instance.AddShooter(shooter);

        InitializeShooterState(shooter, state);
    }

    private void InitializeShooterState(Shooter shooter, State state)
    {
        switch (state)
        {
            case State.Fleeing:
                FleeJobSystem.Instance.StartFleeing(shooter);
                break;
            case State.Moving:
                var randX = UnityEngine.Random.Range(XBoundary.x, XBoundary.y);
                var randZ = UnityEngine.Random.Range(ZBoundary.x, ZBoundary.y);
                var target = new Vector3(randX, ShooterYHeight, randZ);
                MovingJobSytem.Instance.StartMoving(shooter, target);
                break;
            case State.Attacking:
                AttackingJobSystem.Instance.StartAttacking(shooter);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(state), state, null);
        }
    }

    /*TransferState is called by the shooters to indicate that they want to move from one state to another*/
    public void TransferState(Shooter shooter, State state)
    {
        shooter.GetComponent<Renderer>().material = StateMaterials.First(m => m.State == state).Material;

        switch (shooter.State)
        {
            case State.Fleeing:
                FleeJobSystem.Instance.StopFleeing(shooter);
                break;
            case State.Moving:
                MovingJobSytem.Instance.StopMoving(shooter);
                break;
            case State.Attacking:
                AttackingJobSystem.Instance.StopAttacking(shooter);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(state), state, null);
        }

        InitializeShooterState(shooter, state);
        
        shooter.State = state;
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
    #endregion
}