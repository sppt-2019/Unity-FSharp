using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

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

    
    public static List<Item> Exercise1()
    {
        List<Item> armour = new List<Item>();
        
        armour.Add(new Item("Helm", 4, 1.8f, 80, 1.1f, 13, 1.8f));
        armour.Add(new Item("Amulet", 86, 1.7f, 72, 1.9f, 32, 2.0f));
        armour.Add(new Item("Chest", 53, 1.6f, 55, 0.6f, 42, 1.0f));
        armour.Add(new Item("Pauldrons", 16, 1.8f, 63, 0.9f, 95, 1.0f));
        armour.Add(new Item("Belt", 80, 1.5f, 50, 1.5f, 5, 1.5f));
        armour.Add(new Item("Greaves", 77, 1.8f, 94, 1.1f, 92, 1.1f));
        armour.Add(new Item("Boots", 4, 0.9f, 61, 1.4f, 2, 1.8f));
        armour.Add(new Item("Gloves", 25, 1.9f, 20, 1.0f, 40, 0.4f));
        armour.Add(new Item("Rings", 81, 0.9f, 48, 1.8f, 18, 0.3f));
        armour.Add(new Item("Weapons", 75, 1.3f, 79, 0.3f, 77, 0.4f));
            
        return armour;
    }
    
    public static List<Group> Exercise2()
    {
        List<Group> GroupedArmour = new List<Group>();
        
        Group head = new Group("Head");
        List<Item> headItems = new List<Item>();
        headItems.Add(new Item("Helm", 4, 1.8f, 80, 1.1f, 13, 1.8f));
        headItems.Add(new Item("Amulet",86, 1.7f, 72, 1.9f, 32, 2.0f));
        head.Items = headItems;
        GroupedArmour.Add(head);
        
        Group torso = new Group("Torso");
        List<Item> torsoItems = new List<Item>();
        torsoItems.Add(new Item("Chest", 53, 1.6f, 55, 0.6f, 42, 1.0f));
        torsoItems.Add(new Item("Pauldrons", 16, 1.8f, 63, 0.9f, 95, 1.0f));
        torsoItems.Add(new Item("Belt", 80, 1.5f, 50, 1.5f, 5, 1.5f));
        torso.Items = torsoItems;
        GroupedArmour.Add(torso);
        
        Group legs = new Group("Legs");
        List<Item> legsItems = new List<Item>();
        legsItems.Add(new Item("Greaves", 77, 1.8f, 94, 1.1f, 92, 1.1f));
        legsItems.Add(new Item("Boots",4, 0.9f, 61, 1.4f, 2, 1.8f));
        legs.Items = legsItems;
        GroupedArmour.Add(legs);
        
        Group hands = new Group("Hands");
        List<Item> handsItems = new List<Item>();
        handsItems.Add(new Item("Gloves", 25, 1.9f, 20, 1.0f, 40, 0.4f));
        handsItems.Add(new Item("Rings",81, 0.9f, 48, 1.8f, 18, 0.3f));
        handsItems.Add(new Item("Weapons",75, 1.3f, 79, 0.3f, 77, 0.4f));
        hands.Items = handsItems;
        GroupedArmour.Add(hands);
            
        return GroupedArmour;
    }

    public string Name { get; set; }
    public int Intellect { get; set; } = 0;
    public int Strength { get; set; } = 0;
    public int Agility { get; set; } = 0;
    
    public float IntellectMod { get; set; }
    public float StrengthMod { get; set; }
    public float AgilityMod { get; set; }

    public override string ToString()
    {
        return $"{Name}: [{Intellect}, {Strength}, {Agility}]";
    }
}
