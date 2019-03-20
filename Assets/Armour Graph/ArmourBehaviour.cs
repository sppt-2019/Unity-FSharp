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
    }

    // Update is called once per frame
    void Update()
    {
       
    }
}
