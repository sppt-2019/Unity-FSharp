using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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
        var nodeRoot = CreateTree(nodes[0]);

        Debug.Log(nodeRoot);
        
        //say(nodeRoot);
    }

    private RepresentationNode CreateTree(Node node)
    {
        Debug.Log(node.Name);
        var res = new RepresentationNode(node.Name, node.Line);
        
        foreach (var childName in node.ChildNameNames)
        {
            var childNode = nodes.Find(n => n.Name == childName);
            if (childNode == null) continue;
            var child = CreateTree(childNode);
            res.Children.Add(child);
        }
        return res;
    }

    private class RepresentationNode : Node
    {
        public List<RepresentationNode> Children { get; set; }

        public RepresentationNode(string name, string line) : base(name, line)
        {
            Children = new List<RepresentationNode>();
        }

        private RepresentationNode(string name, string line, string child) : base(name, line, child)
        {
            Children = new List<RepresentationNode>();
            throw new NotImplementedException("You are not supposed to use this.");
        }

        public RepresentationNode(string name, string line, List<string> childNames, List<RepresentationNode> children) : base(name, line, childNames)
        {
            Children = children;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
