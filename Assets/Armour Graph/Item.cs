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
    private static readonly int MAX = 100;
    private static readonly int MIN = 0;

    public Item(string name)
    {
        Name = name;
        Intellect = Random.Range(MIN, MAX);
        IntellectMod = Random.Range(0.5f, 2.0f);
        Strength = Random.Range(MIN, MAX);
        StrengthMod = Random.Range(0.5f, 2.0f);
        Agility = Random.Range(MIN, MAX);
        AgilityMod = Random.Range(0.5f, 2.0f);
        
    }
    
    public Item(string name, int intellect, float intMod, int strength, float strMod, int agility, float agiMod)
    {
        Name = name;
        Intellect = intellect;
        IntellectMod = intMod;
        Strength = strength;
        StrengthMod = strMod;
        Agility = agility;
        AgilityMod = agiMod;

    }

    
    public static IEnumerable<Item> Exercise1()
    {
        return UnityEngine.Resources.LoadAll<Item>("Items");
    }
    
    public static List<Group> Exercise2()
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
        return $"{Name}: [{Intellect}, {Strength}, {Agility}]";
    }
}
