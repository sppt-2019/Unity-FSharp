using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Group : ScriptableObject
{
    public Group(ItemGroup group, IEnumerable<Item> itemsInGroup)
    {
        GroupName = group;
        Items = itemsInGroup.ToList();
    }

    public ItemGroup GroupName { get; set; }

    public List<Item> Items { get; set; }

    public override string ToString()
    {
        var itms = "\n" + string.Join("\n", Items);
        
        return $"{GroupName.ToString()}:[" + itms + "]";
    }
}
