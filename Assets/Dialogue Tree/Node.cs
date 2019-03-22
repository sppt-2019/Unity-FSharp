using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DialogOutcome
{
    Continues,
    Companion,
    Hostile,
    Gift,
    None
}

public class Node
{
    private Node(string name, string line, (string,string) child) 
        : this(name, line, new List<(string, string)> {child}) {}

    private Node(string name, string line, List<(string, string)> children)
    {
        Name = name;
        Line = line;
        ChildNames = children;
    }

    private Node(string name, string line, DialogOutcome outcome)
    {
        Name = name;
        Line = line;
        Outcome = outcome;
        ChildNames = ChildNames;
    }
    
    public static List<Node> GetTree()
    {
        return new List<Node>
        {
            new Node("welcome", "Velkommen til vores lille by, hvad kan jeg hjælpe med?",
                new List<(string, string)>{("work", "Jeg leder efter arbejde..."), 
                    ("found", "Jeg fandt denne hund ude i skoven, kender du ejeren?"), ("move", "Flyt dig!")}),
            new Node("work",
                "Min hund er blevet væk og jeg skulle alligevel ud og lede efter den. Har du brug for hjælp?",
                new List<(string,string)> {("yes", "Ja"), ("no", "Nej")}),
            new Node("yes", "Fantastisk, mit sværd er dit!", DialogOutcome.Companion),
            new Node("no", "Ærgeligt, sig til hvis du ombestemmer dig.", ("welcome", "Ok")),
            new Node("found", "MIN LILLE HUND! Den har været væk i flere dage. Tusind tak!", 
                DialogOutcome.Gift),
            new Node("move", "Her i byen taler vi pænt til hinanden!",
                new List<(string,string)> {("die", "Så skal byen DØØØØØØØ!"), 
                    ("sorry", "Undskyld. Det har været en dårlig dag")}),
            new Node("die", "VI BLIVER ANGREBET!", DialogOutcome.Hostile),
            new Node("sorry", "Det er i orden. Det kan ske for alle", ("welcome", "Tak")),
            new Node("goodbye", "Held og lykke på dine rejser", DialogOutcome.None)
        };
    }
    
    public string Name { get; }
    public string Line { get; }
    public List<(string nodeToGoTo, string reply)> ChildNames { get; set; }
    public DialogOutcome Outcome { get; set; } = DialogOutcome.Continues;
}
