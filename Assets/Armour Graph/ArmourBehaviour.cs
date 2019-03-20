using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmourBehaviour : MonoBehaviour
{
    List<Item> Armour;
    List<Group> GroupedArmour = new List<Group>();
    // Start is called before the first frame update
    void Start()
    {
        Armour = new List<Item>{
            new Item("Helm"), new Item("Amulet"), new Item("Chest"),
            new Item("Pauldrons"), new Item("Belt"), new Item("Greaves"),
            new Item("Boots"), new Item("Gloves"), new Item("Rings"), new Item("Weapons")
        };

        var head = new Group("Head"){
            Items = new List<Item>{
            new Item("Helm"), new Item("Amulet")
        }};
        GroupedArmour.Add(head);

        var torso = new Group("Torso"){
            Items = new List<Item>{
            new Item("Chest"),
            new Item("Pauldrons"),
            new Item("Belt")
        }};
        GroupedArmour.Add(torso);

        var legs = new Group("Legs"){
            Items = new List<Item>{
            new Item("Greaves"),
            new Item("Boots")
        }};
        GroupedArmour.Add(legs);

        var hands = new Group("Hands"){
            Items = new List<Item>{
            new Item("Gloves"),
            new Item("Rings"),
            new Item("Weapons")
        }};
        GroupedArmour.Add(hands);
        
        Debug.Log(Armour);
        Debug.Log(GroupedArmour);
    }

    // Update is called once per frame
    void Update()
    {
       
    }
}
