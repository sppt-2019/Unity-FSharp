using UnityEngine;

public class ThirdPersonController : MonoBehaviour
{

    [SerializeField] private float _movementSpeed = 5f;
    [SerializeField] private float _jumpForce = 100f;

    void Awake()
    {
        _myBody = GetComponent<Rigidbody>();
        _camera = transform.GetChild(0);
    }

    private void Start()
    {
        _mouse = Input.mousePosition.normalized;
        _origPos = _camera.position;
        _origPos.z = 0;
        _origPos = _origPos.normalized;
    }

    private Rigidbody _myBody;
    private Transform _camera;
    Vector3 _origPos;
    private Vector2 _mouse;
    private void Update()
    {

        //Vector3 movementForce = new Vector3(;
        if (Input.GetKey(KeyCode.W))
        {
            _myBody.AddForce(Vector3.forward * _movementSpeed);
        }
        if (Input.GetKey(KeyCode.S))
        {
            _myBody.AddForce(Vector3.forward * -_movementSpeed);
        }
        if (Input.GetKey(KeyCode.A))
        {
            _myBody.AddForce(Vector3.right * -_movementSpeed);
        }
        if (Input.GetKey(KeyCode.D))
        {
            _myBody.AddForce(Vector3.right * _movementSpeed);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _myBody.AddForce(Vector3.up * _jumpForce);
        }

        Vector2 currentMouse = Input.mousePosition.normalized;
        Vector2 mouseDelta = _mouse - currentMouse;
        //_mouse = currentMouse;


        _camera.LookAt(transform);
        //_camera.RotateAround(transform.position,Vector3.up, mouseDelta.x*10);
        //_camera.RotateAround(transform.position, Vector3.right, mouseDelta.y*10);
        Vector3 target = new Vector3(Mathf.Cos(mouseDelta.x) * Mathf.PI, Mathf.Sin(mouseDelta.y) * Mathf.PI, 0);
        target.z = 3;
        _camera.localPosition = target;



    }
}
