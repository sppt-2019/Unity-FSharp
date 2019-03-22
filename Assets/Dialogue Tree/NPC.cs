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

        var traces = Solution2(nodeRoot);
        Debug.Log(traces);
        Debug.Log(traces.Count);
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

    private List<List<RepresentationNode>> Solution2(RepresentationNode root)
    {
        var traces = new List<List<RepresentationNode>>();
        Trace(new List<RepresentationNode>(), ref traces, root, root.Name,  true);
        return traces;
    }

    private static void Trace(List<RepresentationNode> partialTrace, ref List<List<RepresentationNode>> traces, RepresentationNode node, string rootName, bool first)
    {
        var trace = partialTrace;
        trace.Add(node);

        if (IsTraceEnd(node, rootName) && !first)
        {
            UniqueAddition(trace, ref traces);
            return;
        }
        
        foreach (var child in node.Children)
        {
            Trace(trace, ref traces, child, rootName, false);
        }
    }
    
    private static bool IsTraceEnd(RepresentationNode node, string rootName)
    {
        return node.Name == rootName || node.Children.Count <= 0;
    }

    private static void UniqueAddition(List<RepresentationNode> trace, ref List<List<RepresentationNode>> traces)
    {
        var existingTrace = traces.Find(t => Compare(t, trace));

        if (existingTrace != null)
            return;
        traces.Add(trace);
    }

    private static bool Compare(List<RepresentationNode> lst, List<RepresentationNode> lst2)
    {
        if (lst.Count != lst2.Count)
            return false;

        for (int i = lst.Count - 1; i >= 0; i--)
        {
            if (lst[i].Name != lst2[i].Name)
                return false;
        }

        return true;
    }

    #endregion

    // Update is called once per frame
    void Update()
    {
        
    }
}
