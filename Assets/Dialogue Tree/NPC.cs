using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class NPC : MonoBehaviour
{
    Node _currentNode;
    public UnityEngine.UI.Text outText;
    private List<Node> _nodes;
    
    public void Say(Node node)
    {
        _currentNode = node;
        outText.text = node.Line;
    }
    
    public void Say( )
    {
        outText.text = _currentNode.Line;
    }

    // Start is called before the first frame update
    void Start()
    {
        _nodes = Node.GetTree();
        _currentNode = _nodes[0];
        
        Say(_nodes[0]);
    }
}
