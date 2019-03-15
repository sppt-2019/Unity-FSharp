using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TreePopulater : MonoBehaviour
{
    public Talent BaseTalent;

    // Start is called before the first frame update
    void Start()
    {
        var children = this.GetComponentsInChildren<TalentUI>();

        int noTalents = 0;
        FillTalents(BaseTalent, children, ref noTalents);
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
