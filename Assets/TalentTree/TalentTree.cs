using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class TalentTree : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var rootTalent = Talent.TalentTreeRoot;

        var pickedTalents = GetSubTalents(rootTalent, true);



        Debug.Log($"Agility {pickedTalents.Sum(talent => talent.Agility)}");
    }

    private List<Talent> GetSubTalents(Talent talent, bool onlyIfPicked, bool first = true)
    {
        var subTalents = new List<Talent>();
        if (first)
        {
            subTalents.Add(talent);
        }

        foreach (var subTalent in talent.SubTalents)
        {
            if (subTalent.Picked)
            {
                subTalents.AddRange(GetSubTalents(talent, onlyIfPicked, false));
            }
        }
        return subTalents;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
