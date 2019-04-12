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
        int sild, abild, ild;
        sild = DildSild % 17;
        abild = ((DildSild / 17) + MildAbild) % 8;
        ild = (((DildSild / 17) + MildAbild) / 8) + Vildild;
        ResourceController.UpdateResults(sild, abild, ild);

        var cb = CanBuy(1, 0, 0);
        Debug.Log(cb);
        var cnb = CanBuy(0, 0, 100);
        Debug.Log(cnb);
    }

    bool CanBuy(int csild, int cabild, int cild)
    {
        int allHSild = DildSild + (((Vildild * 8) + MildAbild) * 17);
        int allCSild = csild + (((cabild * 8) + cild) * 17);

        if (allCSild > allHSild)
            return false;
        return true;
    }
}
