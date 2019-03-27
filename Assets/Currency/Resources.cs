using UnityEngine;

public class Resources : MonoBehaviour
{
    public int DildSild;
    public int MildAbild;
    public int Vildild;
    public ResourceController ResourceController;

    // Start is called before the first frame update
    void Start()
    {
        ResourceController.UpdateResources(DildSild, MildAbild, Vildild);
        ResourceController.UpdateResults(0, 0, 0);
    }
}
