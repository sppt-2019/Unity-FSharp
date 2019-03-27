using System.Collections.Generic;
using UnityEngine;

public class ArmourBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var Armour = Item.Exercise1();
        Debug.Log("Exercise 1:\n\t" + string.Join("\n\t", Armour));

        List<Group> GroupedArmour = Item.Exercise2();
        Debug.Log("Exercise 2:\n" + string.Join("\n", GroupedArmour));
    }

    // Update is called once per frame
    void Update()
    {
       
    }
}
