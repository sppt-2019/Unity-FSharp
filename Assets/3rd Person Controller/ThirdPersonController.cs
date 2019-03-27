using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonController : MonoBehaviour
{
    public float speed;
    public float jumpower = 500;
    public Camera cam;
    
    
    void Start()
    {

    }

    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        
        Vector3 dir = new Vector3(x,0,y);
        
        transform.Translate(dir);


        float mx = Input.GetAxis("Mouse X");
        float my = Input.GetAxis("Mouse Y");
        
        Vector3 mdir = new Vector3(my, mx, 0f);
        
        cam.transform.RotateAround(transform.position, Vector3.up, mx);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            GetComponent<Rigidbody>().AddForce(Vector3.up * jumpower);
        }

    }
}
