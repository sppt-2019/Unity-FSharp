using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSController : MonoBehaviour
{
    public float MoveSpeed = 5.0f;
    public float RotationSpeed = 180.0f;
    public float JumpSpeed = 8.0f;

    public Camera Camera;

    private Rigidbody _rigidbody;
    
    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            Jump();

        var moveVec = transform.forward * GetStep("Vertical") + transform.right * GetStep("Horizontal");
        _rigidbody.MovePosition(_rigidbody.transform.position + moveVec);

        Camera.transform.Rotate(-GetRotationStep("Mouse Y"), 0.0f, 0.0f);
        transform.Rotate(0.0f, GetRotationStep("Mouse X"), 0.0f);
    }

    private float GetStep(string axis) => Input.GetAxis(axis) * Time.deltaTime * MoveSpeed;
    private float GetRotationStep(string axis) => Input.GetAxis(axis) * Time.deltaTime * RotationSpeed;

    private void Jump()
    {
        _rigidbody.velocity = _rigidbody.velocity + (Vector3.up * JumpSpeed);
    }
}
