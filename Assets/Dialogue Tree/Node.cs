using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : ScriptableObject
{
    public Node(string name, string line)
    {
        Name = name;
        Line = line;
        Children = new List<string>();
    }
    public Node(string name, string line, string child)
    {
        Name = name;
        Line = line;
        Children = new List<string>();
        Children.Add(child);
    }
    public Node(string name, string line, List<string> children)
    {
        Name = name;
        Line = line;
        Children = Children;
    }
    public string Name { get; }
    public string Line { get; }
    public List<string> Children { get; set; }
}
