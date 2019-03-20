using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Group : ScriptableObject
{
<<<<<<< HEAD
    public Group(ItemGroup group, IEnumerable<Item> itemsInGroup)
    {
        GroupName = group;
        Items = itemsInGroup.ToList();
    }

    public ItemGroup GroupName { get; set; }
=======
    public Group(ItemGroup itemGroup, IEnumerable<Item> itemsInGroup)
    {
        IemmGroup = itemGroup;
        Items = itemsInGroup.ToList();
    }

    public ItemGroup IemmGroup { get; set; }
>>>>>>> f99be3c8e09ae87a556e98535d01e12db80e5a20

    public List<Item> Items { get; set; }

    public override string ToString()
    {
        var itms = "\n" + string.Join("\n", Items);

<<<<<<< HEAD
        return $"{GroupName.ToString()}:[" + itms + "]";
=======
        return $"{IemmGroup.ToString()}" + itms + "]";
>>>>>>> f99be3c8e09ae87a556e98535d01e12db80e5a20
    }
}
