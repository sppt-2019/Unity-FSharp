using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NPC : MonoBehaviour
{
    Node currentNode;
    List<Node> nodes;
    
    public void say( )
    {
        outText.text = currentNode.Line;
    }
    
    public void say(Node node)
    {
        currentNode = node;
        outText.text = node.Line;
    }
    
    public void say(Node node, List<Node> options)
    {
        outText.text = currentNode.Line;
    }

    public UnityEngine.UI.Text outText;
    
    
    // Start is called before the first frame update
    void Start()
    {
        nodes = Node.GetTree();
        currentNode = nodes[0];

        myNode nodeRoot = CreateTree(nodes[0]);

        say(nodeRoot);
    }

    private myNode CreateTree(Node node)
    {
        var res = new myNode(node.Name, node.Line);
        
        foreach (var childName in node.ChildNames)
        {
            myNode child = CreateTree(nodes.Find(n => n.Name == childName));
            res.Children.Add(child);
        }
        
        return res;
    }

    private class myNode : Node
    {
        public List<myNode> Children { get; set; }

        public myNode(string name, string line) : base(name, line)
        {
        }

        private myNode(string name, string line, string child) : base(name, line, child)
        {
        }

        public myNode(string name, string line, List<string> children) : base(name, line, children)
        {
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
