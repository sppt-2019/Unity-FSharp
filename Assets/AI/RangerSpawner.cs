using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public enum RangerType
{
    CSharp,
    CSharpInverse,
    FRP,
    FRPInverse
}

[Serializable]
public struct RangerTypePrefab
{
    public RangerType Type;
    public GameObject Prefab;
}

public class RangerSpawner : MonoBehaviour
{
    public int NumberOfRangers;
    public Vector2 XBoundary;
    public Vector2 ZBoundary;
    public RangerType RangerType;
    public RangerTypePrefab[] RangerPrefabs;

    // Start is called before the first frame update
    private void Start()
    {
        DestroyUnusedSetup();
        
        for (var i = 0; i < NumberOfRangers; i++)
        {
            var r = InstantiateRanger();

            var pos = new Vector3(
                Random.Range(XBoundary.x, XBoundary.y),
                r.transform.position.y,
                Random.Range(ZBoundary.x, ZBoundary.y));
            r.transform.position = pos;
        }
    }

    private GameObject InstantiateRanger()
    {
        var pf = RangerPrefabs.FirstOrDefault(r => r.Type == RangerType);
        return Instantiate<GameObject>(pf.Prefab);
    }

    private void DestroyUnusedSetup()
    {
        var sms = GameObject.FindGameObjectsWithTag("StateMachine");
        foreach (var sm in sms)
        {
            if(sm.name.Contains("FRP") && !IsFRP(RangerType))
                Destroy(sm.gameObject);
            else if(!sm.name.Contains("FRP") && IsFRP(RangerType))
                Destroy(sm.gameObject);
        }
    }
    
    public static bool IsFRP(RangerType type)
    {
        return type == RangerType.FRP || type == RangerType.FRPInverse;
    }
}
