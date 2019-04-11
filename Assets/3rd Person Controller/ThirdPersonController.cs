using UnityEngine;

public class ThirdPersonController : MonoBehaviour
{

    [SerializeField]
    private float Speed;

    [SerializeField]
    private float JumpPower;

    [SerializeField]
    private Transform _camera;

    private Vector3 _movement;
    private Vector3 _mouseRot;
    private Rigidbody _rb;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

        _movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        _movement *= Speed;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _movement.y = JumpPower;
        }

      

        _rb.AddForce((transform.forward*_movement.z)+(transform.right*_movement.x));

        transform.Rotate(Vector3.up, Input.GetAxis("Mouse X"));
        _camera.transform.RotateAround(transform.position,_camera.right, Input.GetAxis("Mouse Y"));






    }
}
