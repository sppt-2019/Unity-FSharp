using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MagnetismController : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed;
    private GameObject[] _myBalls;

    // Start is called before the first frame update
    void Start()
    {
        _myBalls = GameObject.FindGameObjectsWithTag("Magnetic");
    }

    private Vector3 CalcCenter(GameObject[] objects)
    {
        var center = new Vector3();
        center = objects.Aggregate(center, (current, obj) => current + obj.transform.position);

        return new Vector3(center.x / objects.Length,center.y / objects.Length,center.z / objects.Length);
    }
    
    // Update is called once per frame
    void Update()
    {
        var center = CalcCenter(_myBalls);

        foreach (var ball in _myBalls)
        {
            ball.transform.LookAt(center);
            ball.transform.GetComponent<Rigidbody>().MovePosition(ball.transform.position + (ball.transform.forward * moveSpeed * Time.deltaTime));
        }
    }
}
