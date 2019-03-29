using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Group
{
    public Group(ItemGroup itemGroup, IEnumerable<Item> itemsInGroup)
    {
        ItmGroup = itemGroup;
        Items = itemsInGroup.ToList();
    }

    public ItemGroup ItmGroup { get; set; }

    public List<Item> Items { get; set; }

    public override string ToString()
    {
        var itms = "\n" + string.Join("\n", Items);

        return $"{ItmGroup.ToString()}" + itms + "]";
    }
}
