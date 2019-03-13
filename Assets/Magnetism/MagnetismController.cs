using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetismController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var balls = GameObject.FindGameObjectsWithTag("Magnetic");
        Debug.Log("There are " + balls.Length + " magnetic balls");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
