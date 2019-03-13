using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    Node currentNode;
    List<Node> nodes = new List<Node>();

    public void say(Node node)
    {
        currentNode = node;
        outText.text = node.Line;
    }
    
    public void say( )
    {
        outText.text = currentNode.Line;
    }

    public UnityEngine.UI.Text outText;
    
    
    // Start is called before the first frame update
    void Start()
    {
        nodes.Add(new Node("welcome", "Velkommen til vores lille by, hvad kan jeg hjælpe med?", "work"));
        nodes.Add(new Node("work", "Min hund er blevet væk og jeg skulle alligevel ud og lede efter den. Har du brug for hjælp?", 
            new List<string>{ "yes", "no"}));
        nodes.Add(new Node("yes", "Fantastisk, mit sværd er dit!", "companion"));
        nodes.Add(new Node("no", "Ærgeligt, sig til hvis du ombestemmer dig.", "work"));
        nodes.Add(new Node("found", "MIN LILLE HUND! Den har været væk i flere dage. Tusind tak!", "gift"));
        nodes.Add(new Node("move", "Her i byen taler vi pænt til hinanden!", 
            new List<string>{ "die", "sorry"}));
        nodes.Add(new Node("die", "VI BLIVER ANGREBET!", "hostile"));
        nodes.Add(new Node("sorry", "Det er i orden. Det kan ske for alle", "work"));
        nodes.Add(new Node("goodbye", "Held og lykke på dine rejser", "no"));

        currentNode = nodes[0];
        say();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
