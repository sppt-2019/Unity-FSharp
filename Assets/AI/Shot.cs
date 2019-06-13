using UnityEngine;

public class Shot : MonoBehaviour
{
    public bool HasExitedSpawnerCollider { get; private set; } = false;

    void Start()
    {
        Destroy(this.gameObject, 5f);
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
