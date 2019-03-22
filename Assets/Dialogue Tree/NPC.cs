using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
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
        Debug.Log(node.ChildNames);
        
        ClearButtons();

        if (node.Outcome != DialogOutcome.Continues)
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
    }
}
