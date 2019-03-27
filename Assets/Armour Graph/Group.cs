using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Group : ScriptableObject
{
    public Group(ItemGroup itemGroup, IEnumerable<Item> itemsInGroup)
    {
        IemmGroup = itemGroup;
        Items = itemsInGroup.ToList();
    }

    public ItemGroup IemmGroup { get; set; }

    public List<Item> Items { get; set; }

    public override string ToString()
    {
        var itms = "\n" + string.Join("\n", Items);

        return $"{IemmGroup.ToString()}" + itms + "]";
    }
}
