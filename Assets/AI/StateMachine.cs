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
    public Dictionary<Shooter, State> Shooters = new Dictionary<Shooter, State>();


    private void Update()
    {
        foreach (var shooter in Shooters)
        {
            switch (shooter.Value)
            {
                case State.Moving:
                    MoveTo(shooter.Key);
                    break;
                case State.Fleeing:
                    Flee(shooter.Key);
                    break;
                case State.Attacking:
                    ExecuteShoot(shooter.Key);
                    break;
            }
        }
    }

    public void JoinState(Shooter shooter, State state)
    {
        Shooters.Add(shooter, state);
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