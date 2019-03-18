using System;
using System.Collections.Generic;
using UnityEngine;

public class ArmourBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        List<Item> Armour = Item.Exercise1();
        List<Group> GroupedArmour = Item.Exercise2();

        Solution1(Armour);
        Solution2(GroupedArmour);
    }

    public void Solution1(List<Item> Armour)
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
        
        var groupTotals = new List<Tuple<string, int, int, int>>();
        
        foreach (var group in GroupedArmour)
        {
            int totalAgi = 0;
            int totalStr = 0;
            int totalInt = 0;
            foreach (var item in group.Items)
            {
                totalAgi += item.Agility;
                totalStr += item.Strength;
                totalInt += item.Intellect;
            }
            groupTotals.Add(new Tuple<string, int, int, int>(group.Name, totalAgi, totalStr, totalInt));
        }
        
        Debug.Log("Exercise 2");
        foreach (var tuple in groupTotals)
        {
            Debug.Log($"{tuple.Item1}\n\t" +
                      $"Agi: {tuple.Item2}\n\t" +
                      $"Str: {tuple.Item3}\n\t" +
                      $"Int: {tuple.Item4}");
        }
    }

    // Update is called once per frame
    void Update()
    {
       
    }
}
