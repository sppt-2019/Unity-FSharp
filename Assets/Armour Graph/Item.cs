using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public enum ItemGroup
{
    Head, Torso, Legs, Hands
}

[CreateAssetMenu(fileName = "Item", menuName = "SPPT/Item", order = 1)]
public class Item : ScriptableObject
{   
    public static IEnumerable<Item> AllItems()
    {
        return UnityEngine.Resources.LoadAll<Item>("Items");
    }
    
    public static List<Group> GroupedItems()
    {
        var items = UnityEngine.Resources.LoadAll<Item>("Items");
        return new List<Group>{
            new Group(ItemGroup.Head, items.Where(i => i.Group == ItemGroup.Head)),
            new Group(ItemGroup.Torso, items.Where(i => i.Group == ItemGroup.Torso)),
            new Group(ItemGroup.Legs, items.Where(i => i.Group == ItemGroup.Legs)),
            new Group(ItemGroup.Hands, items.Where(i => i.Group == ItemGroup.Hands))
};
    }

    public string Name;
    public int Intellect;
    public int Strength;
    public int Agility;
    public ItemGroup Group;

    public float IntellectMod;
    public float StrengthMod;
    public float AgilityMod;

    public override string ToString()
    {
        return $"{Name}: [int: {Intellect}, str: {Strength}, agi: {Agility}]";
    }
}
