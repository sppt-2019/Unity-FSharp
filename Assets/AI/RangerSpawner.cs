using UnityEngine;

public class RangerSpawner : MonoBehaviour
{
    public GameObject RangerPrefab;
    public GameObject FRP_RangerPrefab;
    public int NumberOfRangers;
    public Vector2 XBoundary;
    public Vector2 ZBoundary;
    public bool UseFRP = false;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < NumberOfRangers; i++)
        {
            var r = UseFRP ? Instantiate(FRP_RangerPrefab) : Instantiate(RangerPrefab);

            var pos = new Vector3(
                Random.Range(XBoundary.x, XBoundary.y),
                r.transform.position.y,
                Random.Range(ZBoundary.x, ZBoundary.y));
            r.transform.position = pos;
        }
    }
}
