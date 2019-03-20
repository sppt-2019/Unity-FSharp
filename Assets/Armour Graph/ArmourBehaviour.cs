using System;
using System.Collections.Generic;
using UnityEngine;

public class ArmourBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var Armour = Item.Exercise1();
        var GroupedArmour = Item.Exercise2();

        Solution1(Armour);
        Solution2(GroupedArmour);
    }

    public void Solution1(IEnumerable<Item> Armour)
    {
        int totalAgi = 0;
        int totalStr = 0;
        int totalInt = 0;
        
        foreach (var item in Armour)
        {
            totalAgi += item.Agility;
            totalStr += item.Strength;
            totalInt += item.Intellect;
        }
        Debug.Log($"Exercise 1\n\t" +
                  $"Agility: {totalAgi}\n\t" +
                  $"Strength: {totalStr}\n\t" +
                  $"Intellect: {totalInt}");
    }

    public void Solution2(List<Group> GroupedArmour)
    {
        GroupedArmour = Item.Exercise2();
        
        var groupTotals = new List<Tuple<ItemGroup, int, int, int>>();
        
        foreach (var group in GroupedArmour)
        {
            int totalAgi = 0;
            int totalStr = 0;
            int totalInt = 0;

            float agiMod = 0.0f;
            float strMod = 0.0f;
            float intMod = 0.0f;
            
            foreach (var item in group.Items)
            {
                totalAgi += item.Agility;
                totalStr += item.Strength;
                totalInt += item.Intellect;
                
                agiMod += item.AgilityMod;
                strMod += item.StrengthMod;
                intMod += item.IntellectMod;
            }

            totalAgi = (int) (totalAgi * agiMod);
            totalStr = (int) (totalStr * strMod);
            totalInt = (int) (totalInt * intMod);

            groupTotals.Add(new Tuple<ItemGroup, int, int, int>(group.GroupName, totalAgi, totalStr, totalInt));
        }
        
        string res = "Exercise 2\n";
        foreach (var tuple in groupTotals)
        {
            res += $"{tuple.Item1}\n\t" +
                      $"Agi: {tuple.Item2}\n\t" +
                      $"Str: {tuple.Item3}\n\t" +
                      $"Int: {tuple.Item4}\n";
        }
        Debug.Log(res);
    }

    // Update is called once per frame
    void Update()
    {
       
    }
}
