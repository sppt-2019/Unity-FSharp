using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Group : ScriptableObject
{
    public Group(string name)
    {
        Name = name;
        Items = new List<Item>();
    }

    public string Name { get; set; }

    public List<Item> Items { get; set; }

    public override string ToString()
    {
        var itms = "\n" + string.Join("\n", Items);

        return $"{Name:[}" + itms + "]";
    }
}
