using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceController : MonoBehaviour
{
    public Text DildSild;
    public Text MildAbild;
    public Text Vildild;
    public Text DildSildResult;
    public Text MildAbildResult;
    public Text VildildResult;

    public void UpdateResources(int dildSild, int mildAbild, int vildild)
    {
        DildSild.text = dildSild.ToString();
        MildAbild.text = mildAbild.ToString();
        Vildild.text = vildild.ToString();
    }

    public void UpdateResults(int dildSild, int mildAbild, int vildild)
    {
        DildSildResult.text = dildSild.ToString();
        MildAbildResult.text = mildAbild.ToString();
        VildildResult.text = vildild.ToString();
    }
}
