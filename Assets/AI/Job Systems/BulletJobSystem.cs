using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Jobs;
using UnityEngine.UIElements;

public class BulletJobSystem : MonoBehaviour {
    //This array holds all bullets that need to be moved
    private TransformAccessArray _transforms;
    //This job handle may be used to synchronise with previously scheduled move jobs
    private JobHandle _bulletMoveHandle;
    public GameObject BulletPrefab;

    public static BulletJobSystem Instance { get; private set; }
    
    //This struct implements the actual job.
    public struct MoveBulletsJob : IJobParallelForTransform {
        public float _moveSpeed;
        public float _deltaTime;

        public MoveBulletsJob(float moveSpeed, float deltaTime)
        {
            _moveSpeed = moveSpeed;
            _deltaTime = deltaTime;
        }
    
        //In the Execute-method we define how we want to apply an update to a single bullet.
        public void Execute(int index, TransformAccess transform) {
            transform.position += _moveSpeed * _deltaTime * (transform.rotation * new Vector3(0,0,1));
        }
    }
    
    void Start() {
        //Create an array with 0 elements and unlimited capacity
        _transforms = new TransformAccessArray(0, -1);

        if (Instance != null)
            throw new InvalidOperationException("There may only be one instance of the bullet job system.");
        Instance = this;
    }
    
    public void SpawnBullet(Vector3 spawnPosition, Vector3 target) {
        //Instantiate the bullet as usual in Unity
        var bullet = Instantiate(BulletPrefab);
        bullet.transform.position = spawnPosition;
        bullet.transform.LookAt(target, Vector3.up);

        //Synchronise with the job and add the new bullet to the _transforms array.
        _bulletMoveHandle.Complete();
        _transforms.capacity++;
        _transforms.Add(bullet.transform);
    }

    private void OnDestroy()
    {
        _transforms.Dispose();
    }

    void Update() {
        //Complete the job, i.e. only apply one update at a time
        _bulletMoveHandle.Complete();

        //And schedule a new bullet update
        var bulletMoveJob = new MoveBulletsJob (5f, Time.deltaTime);
        _bulletMoveHandle = bulletMoveJob.Schedule(_transforms);
        JobHandle.ScheduleBatchedJobs();
    }
}
