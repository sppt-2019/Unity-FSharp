using UnityEngine;

public class Shot : MonoBehaviour
{
    public float Speed;
    public bool HasExitedSpawnerCollider { get; private set; } = false;

    void Start()
    {
        Destroy(this.gameObject, 5f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = transform.position + (transform.forward * Speed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(HasExitedSpawnerCollider)
            Destroy(this.gameObject);
    }

    private void OnCollisionExit(Collision collision)
    {
        HasExitedSpawnerCollider = true;
    }
}
