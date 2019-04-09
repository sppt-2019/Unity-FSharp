using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MbRanger : MonoBehaviour
{
    private State _state;
    
    public float speed;
    public float rotationSpeed = 25f;
    public int shotsBeforeStateChange = 5;
    public Transform attackTarget;
    public Vector3 moveTarget;
    private const float ShotCooldown = 2f;
    public float cooldowner;

    public GameObject shotPrefab;

    public StateMaterialsComponent StateMaterials;

    private void Start()
    {
        StateMaterials = GameObject.FindGameObjectWithTag("Plastic").GetComponent<StateMaterialsComponent>();
    }

    private void InitNewState(State state)
    {
        GetComponent<Renderer>().material = StateMaterials.StateMaterials.First(sm => sm.State == state).Material;

        switch (state)
        {
            case State.Moving:
                var newTarget = new Vector3(
                    UnityEngine.Random.Range(-3.7f, 5.7f),
                    transform.position.y,
                    UnityEngine.Random.Range(-6f, 3.4f));
                moveTarget = newTarget;
                break;
            case State.Fleeing:
                cooldowner = 4f;
                break;
            case State.Attacking:
                shotsBeforeStateChange = 5;
                cooldowner = 0f;
                attackTarget = GameObject.FindGameObjectWithTag("Tower").transform;
                break;
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        var state = State.Moving;
        switch (_state)
        {
            case State.Fleeing:
                state = Flee();
                break;
            case State.Moving:
                state = Move();
                break;
            case State.Attacking:
                state = Attack();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        if (state != _state)
        {
            InitNewState(state);
            _state = state;
        }
    }
    
    public State Attack()
        {
            //Check if the shooter has any shots left, move a little if not
            if (shotsBeforeStateChange == 0)
                return State.Moving;
    
            //Decrement the cooldown and shoot if ready
            cooldowner -= Time.deltaTime;
            if (cooldowner <= 0f)
            {
                ShootAt(attackTarget);
                shotsBeforeStateChange--;
                cooldowner = ShotCooldown;
            }
            return State.Attacking;
        }
    
        public void ShootAt(Transform target)
        {
            var pos = transform.position;
            var tarPos = target.position;
            
            var lookAtTransform = new Vector3(tarPos.x, pos.y, tarPos.z);
    
            var shot = Instantiate(shotPrefab);
            shot.transform.position = pos;
            shot.transform.LookAt(lookAtTransform, Vector3.up);
        }
    
        public State Flee()
        {
            cooldowner -= Time.deltaTime;
            if(cooldowner <= 0f)
                return State.Attacking;
    
            var stepSize = speed * Time.deltaTime * 2;

            var trans = transform;
            trans.Rotate(0, UnityEngine.Random.Range(-rotationSpeed, rotationSpeed), 0);
            trans.position = trans.position + trans.forward * stepSize;
            return State.Fleeing;
        }
    
        public State Move()
        {
            if(Vector3.Distance(transform.position, moveTarget) < 0.1f)
                return State.Attacking;

            var trans = transform;
            trans.LookAt(moveTarget);
            trans.position = trans.position + (trans.forward * speed * Time.deltaTime);
            return State.Moving;
        }
        
        private void OnCollisionEnter(Collision collision)
        {
            var shot = collision.collider.GetComponent<Shot>();
            if(shot != null && shot.HasExitedSpawnerCollider)
            {
                _state = State.Fleeing;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.name != "Boundary") return;
            transform.Rotate(0, 180, 0);
        }
}
