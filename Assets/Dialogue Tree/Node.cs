using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public Node(string name, string line)
    {
        Name = name;
        Line = line;
        ChildNames = new List<string>();
    }
    public Node(string name, string line, string child)
    {
        Name = name;
        Line = line;
        ChildNames = new List<string>();
        ChildNames.Add(child);
    }
    public Node(string name, string line, List<string> children)
    {
        Name = name;
        Line = line;
        ChildNames = ChildNames;
    }

    public static List<Node> GetTree()
    {
        return new List<Node>
        {
            new Node("welcome", "Velkommen til vores lille by, hvad kan jeg hjælpe med?",
                new List<string> {"work", "found", "move"}),
            new Node("work",
                "Min hund er blevet væk og jeg skulle alligevel ud og lede efter den. Har du brug for hjælp?",
                new List<string> {"yes", "no"}),
            new Node("yes", "Fantastisk, mit sværd er dit!", "companion"),
            new Node("no", "Ærgeligt, sig til hvis du ombestemmer dig.", "work"),
            new Node("found", "MIN LILLE HUND! Den har været væk i flere dage. Tusind tak!", "gift"),
            new Node("move", "Her i byen taler vi pænt til hinanden!",
                new List<string> {"die", "sorry"}),
            new Node("die", "VI BLIVER ANGREBET!", "hostile"),
            new Node("sorry", "Det er i orden. Det kan ske for alle", "work"),
            new Node("goodbye", "Held og lykke på dine rejser", "no-reaction"),
            new Node("companion", "Karakteren bliver ens følgesvend"),
            new Node("hostile", "Karakteren bliver fjendtlig"),
            new Node("gift", "Karakteren giver spilleren en gave"),
            new Node("no-reaction", "Ingen reaktion")
        };
    }
    
    public string Name { get; }
    public string Line { get; }
    public List<string> ChildNames { get; set; }
}
