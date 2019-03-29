using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using UnityEditor;
using UnityEngine;

public class TalentSummer : MonoBehaviour
{
    public Talent BaseTalent;

    private T WalkTree<T>(Talent talent, Func<Talent, T> action, Func<T, T, T> combine)
    {
        var tSelf = action(talent);
        if (talent.SubTalents == null || talent.SubTalents.Length == 0)
            return tSelf;

        return talent.SubTalents.Select(st => WalkTree(st, action, combine)).Aggregate(tSelf, combine);
    }

    private (int, int, int) Sum((int a1, int b1, int c1) a, (int a2, int b2, int c2) b) =>
        (a.a1 + b.a2, a.b1 + b.b2, a.c1 + b.c2);
    
    // Start is called before the first frame update
    void Start()
    {
        var (agility, intellect, strength) = WalkTree(BaseTalent, t => (t.Agility, t.Intellect, t.Strength), Sum);
        Debug.Log($"Agility: {agility}\nIntellect: {intellect}\nStrength: {strength}");
        
        var (tAgility, tIntellect, tStrength) = WalkTree(BaseTalent, 
            t => t.Picked ? (t.Agility, t.Intellect, t.Strength) : (0,0,0), Sum);
        Debug.Log($"Agility: {tAgility}\nIntellect: {tIntellect}\nStrength: {tStrength}");
    }
}
