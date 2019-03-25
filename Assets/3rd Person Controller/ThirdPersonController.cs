using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ThirdPersonController : MonoBehaviour
{
    [Header("Movement")]
    public float jumpSpeed = 8.0f;
    public float moveSpeed = 8.0f;
    public float rotationSpeed = 180;

    [Header("Camera")]
    public float cameraRotationSpeed = 50;
    public Transform cameraTransform;

    private Rigidbody _rigidbody;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Jump()
    {
        _rigidbody.velocity = _rigidbody.velocity + (Vector3.up * jumpSpeed);
    }

    private void Move()
    {
        var deltaX = Input.GetAxis("Horizontal");
        var deltaY = Input.GetAxis("Vertical");

        _rigidbody.MovePosition(_rigidbody.transform.position +
                                _rigidbody.transform.forward * (moveSpeed * Time.deltaTime * deltaY));
        _rigidbody.transform.Rotate(0.0f, rotationSpeed * Time.deltaTime * deltaX, 0.0f);
    }

    private void MoveCamera()
    {
        var deltaX = Input.GetAxis("Mouse X");
        var deltaY = Input.GetAxis("Mouse Y");

        var position = transform.position;
        cameraTransform.RotateAround(position, Vector3.up, cameraRotationSpeed * Time.deltaTime * deltaX);
        cameraTransform.RotateAround(position, cameraTransform.right,
            cameraRotationSpeed * Time.deltaTime * deltaY);
    }
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            Jump();

        Move();
        MoveCamera();
    }
}
