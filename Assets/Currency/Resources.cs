using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using UnityEditor;
using UnityEngine;

public class Resources : MonoBehaviour
{
    private const int MILDABILD_TO_DILDSILD = 17;
    private const int VILDILD_TO_DILDSILD = 8 * 17;
    
    public int DildSild;
    public int MildAbild;
    public int Vildild;
    public ResourceController ResourceController;

    private int Sum(int dildSild, int mildAbild, int vildIld) => dildSild + (mildAbild * MILDABILD_TO_DILDSILD) + (vildIld * VILDILD_TO_DILDSILD);

    private (int dildSild, int mildAbild, int vildIld) Distribute(int totalDildSild)
    {

        var rs = new[] {VILDILD_TO_DILDSILD, MILDABILD_TO_DILDSILD, 1}.Select(cr =>
        {
            var resources = totalDildSild / cr;
            totalDildSild -= (resources * cr);
            return resources;
        }).ToArray();

        return (rs[2], rs[1], rs[0]);
    }

    private bool CanBuy(int dildSild, int mildAbild, int vildIld) =>
        Sum(DildSild, MildAbild, Vildild) >= Sum(dildSild, mildAbild, vildIld);

    public void Buy(int dildSild, int mildAbild, int vildIld)
    {
        if (!CanBuy(dildSild, mildAbild, vildIld)) return;
        var price = Sum(dildSild, mildAbild, vildIld);
        var wallet = Sum(DildSild, MildAbild, Vildild);

        var rem = wallet - price;
        var (ds, ma, vi) = Distribute(rem);

        DildSild = ds;
        MildAbild = ma;
        Vildild = vi;
            
        ResourceController.UpdateResults(DildSild, MildAbild, Vildild);
    }
    
    // Start is called before the first frame update
    void Start()
    {
        ResourceController.UpdateResources(DildSild, MildAbild, Vildild);

        var tds = Sum(DildSild, MildAbild, Vildild);
        Debug.Log("Total DildSild: " + tds);
        var (ds, ma, vi) = Distribute(tds);
        
        ResourceController.UpdateResults(ds, ma, vi);

        Buy(10, 10, 10);
        Buy(1, 2, 3);
        Buy(15, 0, 0);
    }
}
