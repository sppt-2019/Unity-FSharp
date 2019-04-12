using UnityEngine;

public class Resources : MonoBehaviour
{
    const int DILDSILD_TO_MILDABILD = 17;
    const int MILDABILD_TO_VILDILD = 8;

    public int DildSild;
    public int MildAbild;
    public int Vildild;
    public ResourceController ResourceController;

    // Start is called before the first frame update
    void Start()
    {
        ResourceController.UpdateResources(DildSild, MildAbild, Vildild);

        MildAbild += Mathf.FloorToInt(DildSild / DILDSILD_TO_MILDABILD);
        DildSild = DildSild % DILDSILD_TO_MILDABILD;

        Vildild += Mathf.FloorToInt(MildAbild / MILDABILD_TO_VILDILD);
        MildAbild = MildAbild % MILDABILD_TO_VILDILD;

        ResourceController.UpdateResults(DildSild, MildAbild, Vildild);
    }
}
