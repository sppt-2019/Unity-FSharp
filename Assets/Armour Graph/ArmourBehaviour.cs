using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmourBehaviour : MonoBehaviour
{
    List<Item> Armour = new List<Item>();
    List<Group> GroupedArmour = new List<Group>();
    // Start is called before the first frame update
    void Start()
    {
        Armour.Add(new Item("Helm"));
        Armour.Add(new Item("Amulet"));
        Armour.Add(new Item("Chest"));
        Armour.Add(new Item("Pauldrons"));
        Armour.Add(new Item("Belt"));
        Armour.Add(new Item("Greaves"));
        Armour.Add(new Item("Boots"));
        Armour.Add(new Item("Gloves"));
        Armour.Add(new Item("Rings"));
        Armour.Add(new Item("Weapons"));
        
        
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
        
        Debug.Log(Armour);
        Debug.Log(GroupedArmour);
    }

    // Update is called once per frame
    void Update()
    {
       
    }
}
