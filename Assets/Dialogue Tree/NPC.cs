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

        var giftNodes = Solution2(nodeRoot);
        
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
        var existNodes = new List<RepresentationNode>();
        TreeBuilder(res, node, existNodes, nodes, limit);
        return res;
    }

    private static void TreeBuilder(RepresentationNode parentRep, Node parentNode, List<RepresentationNode> exist, List<Node> nodes, int limit)
    {
        
        foreach (var childName in parentNode.ChildNames)
        {
            var node = nodes.Find(n => n.Name == childName);
            var child = Get(node, exist);
            parentRep.Children.Add(child);
            exist.Add(child);
            TreeBuilder(child, node, exist, nodes, limit -1);
        }
    }

    private static RepresentationNode Get(Node node, List<RepresentationNode> exist)
    {
        var rep = exist.Find(n => n.Name == node.Name);
        return rep ?? new RepresentationNode(node.Name, node.Line, node.Reaction);
    }

    private class RepresentationNode : Node
    {
        public List<RepresentationNode> Children { get; set; }

        public RepresentationNode(string name, string line) : base(name, line)
        {
            Children = new List<RepresentationNode>();
        }
        
        public RepresentationNode(string name, string line, NPCReaction reaction) : base(name, line, reaction)
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

    private static List<RepresentationNode> Solution2(RepresentationNode node)
    {
        var giftNodes = new List<RepresentationNode>();
        Traverse(node, giftNodes);
        return giftNodes;
    }

    private static void Traverse(RepresentationNode node, List<RepresentationNode> giftNodes)
    {
        if(node.Reaction == Node.NPCReaction.Gift && !giftNodes.Contains(node))
            giftNodes.Add(node);
        
        node.Children.ForEach(chld =>
        {
            if (chld.Reaction == Node.NPCReaction.Gift && !giftNodes.Contains(chld))
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
