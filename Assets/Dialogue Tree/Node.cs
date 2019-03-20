using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : ScriptableObject
{
    public Node(string name, string line)
    {
        Name = name;
        Line = line;
        ChildNameNames = new List<string>();
    }
    public Node(string name, string line, string child)
    {
        Name = name;
        Line = line;
        ChildNameNames = new List<string>();
        ChildNameNames.Add(child);
    }
    public Node(string name, string line, List<string> childNames)
    {
        Name = name;
        Line = line;
        ChildNameNames = childNames;
    }

    public static List<Node> GetTree()
    {
        List<Node> nodes = new List<Node>();
        nodes.Add(new Node("welcome", "Velkommen til vores lille by, hvad kan jeg hjælpe med?", 
            new List<string>{"work", "found", "move"}));
        nodes.Add(new Node("work", "Min hund er blevet væk og jeg skulle alligevel ud og lede efter den. Har du brug for hjælp?", 
            new List<string>{ "yes", "no"}));
        nodes.Add(new Node("yes", "Fantastisk, mit sværd er dit!", "companion"));
        nodes.Add(new Node("no", "Ærgeligt, sig til hvis du ombestemmer dig.", "work"));
        nodes.Add(new Node("found", "MIN LILLE HUND! Den har været væk i flere dage. Tusind tak!", "gift"));
        nodes.Add(new Node("move", "Her i byen taler vi pænt til hinanden!", 
            new List<string>{ "die", "sorry"}));
        nodes.Add(new Node("die", "VI BLIVER ANGREBET!", "hostile"));
        nodes.Add(new Node("sorry", "Det er i orden. Det kan ske for alle", "work"));
        nodes.Add(new Node("goodbye", "Held og lykke på dine rejser", "no-reaction"));
        nodes.Add(new Node("companion", "Karakteren bliver ens følgesvend"));
        nodes.Add(new Node("hostile", "Karakteren bliver fjendtlig"));
        nodes.Add(new Node("gift", "Karakteren giver spilleren en gave"));
        nodes.Add(new Node("no-reaction", "Ingen reaktion"));

        return nodes;
    }
    
    public string Name { get; }
    public string Line { get; }
    public List<string> ChildNameNames { get; set; }
}
