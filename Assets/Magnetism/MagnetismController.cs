using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetismController : MonoBehaviour
{
    [SerializeField]
    public float moveSpeed;
    private GameObject[] balls;

    // Start is called before the first frame update
    void Start()
    {
        balls = GameObject.FindGameObjectsWithTag("Magnetic");
        foreach (var ball in balls)
        {
            ball.transform.LookAt(CalcCenter(balls));
        }
    }

    private Vector3 CalcCenter(GameObject[] objects)
    {
        var vec = new Vector3();

        foreach (var obj in objects)
        {
            vec += obj.transform.position;
        }

        return new Vector3(vec.x / objects.Length,-1.51f,vec.z / objects.Length);
    }
    
    // Update is called once per frame
    void Update()
    {
        var center = CalcCenter(balls);
        foreach (var ball in balls)
        {
            ball.transform.LookAt(center);
            var rb = ball.GetComponent<Rigidbody>();
            rb.MovePosition(rb.transform.position + transform.forward * moveSpeed * Time.deltaTime);
            //ball.transform.Translate(ball.transform.forward * moveSpeed * Time.deltaTime, Space.Self);
        }
    }
}
