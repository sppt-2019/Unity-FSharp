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
        Strength = Random.Range(MIN, MAX);
        Agility = Random.Range(MIN, MAX);
        
    }

    
    public static List<Item> Exercise1()
    {
        List<Item> armour = new List<Item>();
        
        armour.Add(new Item("Helm"));
        armour.Add(new Item("Amulet"));
        armour.Add(new Item("Chest"));
        armour.Add(new Item("Pauldrons"));
        armour.Add(new Item("Belt"));
        armour.Add(new Item("Greaves"));
        armour.Add(new Item("Boots"));
        armour.Add(new Item("Gloves"));
        armour.Add(new Item("Rings"));
        armour.Add(new Item("Weapons"));
            
        return armour;
    }
    
    public static List<Group> Exercise2()
    {
        List<Group> GroupedArmour = new List<Group>();
        
        Group head = new Group("Head");
        List<Item> headItems = new List<Item>();
        headItems.Add(new Item("Helm"));
        headItems.Add(new Item("Amulet"));
        head.Items = headItems;
        GroupedArmour.Add(head);
        
        Group torso = new Group("Torso");
        List<Item> torsoItems = new List<Item>();
        torsoItems.Add(new Item("Chest"));
        torsoItems.Add(new Item("Pauldrons"));
        torsoItems.Add(new Item("Belt"));
        torso.Items = torsoItems;
        GroupedArmour.Add(torso);
        
        Group legs = new Group("Legs");
        List<Item> legsItems = new List<Item>();
        legsItems.Add(new Item("Greaves"));
        legsItems.Add(new Item("Boots"));
        legs.Items = legsItems;
        GroupedArmour.Add(legs);
        
        Group hands = new Group("Hands");
        List<Item> handsItems = new List<Item>();
        handsItems.Add(new Item("Gloves"));
        handsItems.Add(new Item("Rings"));
        handsItems.Add(new Item("Weapons"));
        hands.Items = handsItems;
        GroupedArmour.Add(hands);
            
        return GroupedArmour;
    }

    public string Name { get; set; }
    public int Intellect { get; set; } = 0;
    public int Strength { get; set; } = 0;
    public int Agility { get; set; } = 0;

    public override string ToString()
    {
        return $"{Name}: [{Intellect}, {Strength}, {Agility}]";
    }
}
