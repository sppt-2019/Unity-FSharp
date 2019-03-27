using UnityEngine;

public class TreePopulater : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var children = this.GetComponentsInChildren<TalentUI>();
        var bt = Talent.TalentTreeRoot;
        Debug.Log(bt);
        
        int noTalents = 0;
        FillTalents(bt, children, ref noTalents);
        Debug.Log("There were " + noTalents + " talents");
    }

    private void FillTalents(Talent talent, TalentUI[] uiComponents, ref int index)
    {
        uiComponents[index].SetTalent(talent);
        index++;
        
        foreach (var t in talent.SubTalents)
        {
            FillTalents(t, uiComponents, ref index);
        }
    }
}
