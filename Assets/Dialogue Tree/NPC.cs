using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class NPC : MonoBehaviour
{
    Node _currentNode;
    public Text outText;
    public Button[] Buttons;
    private List<Node> _nodes;

    private void ClearButtons()
    {
        foreach (var b in Buttons)
        {
            b.gameObject.SetActive(false);
        }
    }
    
    private void Say(Node node)
    {
        _currentNode = node;
        outText.text = node.Line;
        
        ClearButtons();

        if (node.ChildNames == null || !node.ChildNames.Any())
        {
            Debug.Log("Samtalen er slut med udfaldet: " + node.Outcome);
            return;
        }

        for (var i = 0; i < node.ChildNames.Count; i++)
        {
            var childNode = _nodes.Find(n => n.Name == node.ChildNames[i].nodeToGoTo);
            Buttons[i].GetComponentInChildren<Text>().text = node.ChildNames[i].reply;
            Buttons[i].onClick = new Button.ButtonClickedEvent();
            Buttons[i].onClick.AddListener(() =>
            {
                Say(childNode);
            });
            Buttons[i].gameObject.SetActive(true);
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        _nodes = Node.GetTree();
        _currentNode = _nodes[0];
        
        Say(_nodes[0]);

        var rootNode = Solution1(_nodes[0]);
        var giftNodes = Solution2(rootNode);
        
    }


    #region Solution1

    private RepresentationNode Solution1(Node node)
    {
        var res = new RepresentationNode(node.Name, node.Line);
        var existNodes = new List<RepresentationNode>();
        TreeBuilder(res, node, existNodes, _nodes);
        return res;
    }

    private static void TreeBuilder(RepresentationNode parentRep, Node parentNode, List<RepresentationNode> exist, List<Node> nodes)
    {
        if (parentNode.ChildNames == null || parentNode.ChildNames.Count == 0)
            return;
        
        foreach (var childName in parentNode.ChildNames)
        {
            var node = nodes.Find(n => n.Name == childName.nodeToGoTo);
            var child = Get(node, exist);
            parentRep.Children.Add(child);
            exist.Add(child);
            TreeBuilder(child, node, exist, nodes);
        }
    }

    private static RepresentationNode Get(Node node, List<RepresentationNode> exist)
    {
        var rep = exist.Find(n => n.Name == node.Name);
        return rep ?? new RepresentationNode(node.Name, node.Line, node.Outcome);
    }

    private class RepresentationNode
    {
        public List<RepresentationNode> Children { get; set; }

        public string Line { get; set; }
        public string Name { get; set; }
        public Node.DialogOutcome Outcome { get; set; }

        public RepresentationNode(string name, string line, Node.DialogOutcome outcome = Node.DialogOutcome.None)
        {
            Name = name;
            Line = line;
            Children = new List<RepresentationNode>();
        }

        public RepresentationNode(string name, string line, Node.DialogOutcome outcome, List<RepresentationNode> children) : this(name, line, outcome)
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
        if(node.Outcome == Node.DialogOutcome.Gift && !giftNodes.Contains(node))
            giftNodes.Add(node);
        
        node.Children.ForEach(chld =>
        {
            if (chld.Outcome == Node.DialogOutcome.Gift && !giftNodes.Contains(chld))
                giftNodes.Add(chld);
        });

        foreach (var child in node.Children)
        {
            Traverse(child, giftNodes);
        }
    }

    #endregion
}
