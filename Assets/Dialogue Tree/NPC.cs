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
    private List<RepresentationNode> existingNodes = new List<RepresentationNode>();
    
    public void say( )
    {
        outText.text = currentNode.Line;
    }
    
    public void Say(Node node)
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
        var nodeRoot = Solution1(nodes[0], 10);

        Say(nodeRoot);

        
    }


    #region Solution1

    private RepresentationNode Solution1(Node node, int limit)
    {
        if (limit <= 0)
        {
            Debug.Log("Limit Reached!");
            return null;
        }
        var res = new RepresentationNode(node.Name, node.Line);
        var eNode = Exists(res);
        if (eNode != null) return eNode;
        
        foreach (var childName in node.ChildNames)
        {
            var childNode = nodes.Find(n => n.Name == childName);
            if (childNode == null) continue;
            var existNode = Exists(childName);
            if (existNode != null)
            {
                res.Children.Add(existNode);
            } else {
                var child = Solution1(childNode, limit - 1);
                existingNodes.Add(child);
                res.Children.Add(child);
            }
        }
        return res;
    }

    private RepresentationNode Exists (RepresentationNode node)
    {
        var option = existingNodes.Find(n => n.Name == node.Name);
        return option != null ? option : null;
    }
    
    private RepresentationNode Exists (string name)
    {
        var option = existingNodes.Find(n => n.Name == name);
        return option;
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

    #endregion

    #region Solution2

    private List<RepresentationNode> Solution2(RepresentationNode node)
    {
        var giftNodes = new List<RepresentationNode>();
        Traverse(node, giftNodes);
        return giftNodes;
    }

    private static void Traverse(RepresentationNode node, List<RepresentationNode> giftNodes)
    {
        if(node.Reaction == Node.NPCReaction.Gift)
            giftNodes.Add(node);
        
        node.Children.ForEach(chld =>
        {
            if (chld.Reaction == Node.NPCReaction.Gift)
                giftNodes.Add(chld);
        });

        foreach (var child in node.Children)
        {
            Traverse(child, giftNodes);
        }
    }

    #endregion

    // Update is called once per frame
    void Update()
    {
        
    }
}
