using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    public float Speed;
    public float RotationSpeed = 25f;
    public int ShotsBeforeStateChange = 5;
    public Transform AttackTarget;
    public Vector3 MoveTarget;
    public readonly float ShotCooldown = 2f;
    public float Cooldowner;

    private void Start()
    {
        GameObject.FindGameObjectWithTag("StateMachine").GetComponent<StateMachine>().JoinState(this, State.Moving);
    }

    private void OnCollisionEnter(Collision collision)
    {
        var shot = collision.collider.GetComponent<Shot>();
        if(shot != null && shot.HasExitedSpawnerCollider)
        {
            GameObject.FindGameObjectWithTag("StateMachine").GetComponent<StateMachine>().TransferState(this, State.Fleeing);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name == "Boundary")
        {
            transform.Rotate(0, 180, 0);
            return;
        }
    }
}
