using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalentTree : MonoBehaviour
{

    struct MyTalent
    {
        public int intellect;
        public int strength;
        public int agility;
        public bool Picked;
        public MyTalent[] SubTalents;
    }

    public Talent tree;
    private Stack<Talent> pickedTalents = new Stack<Talent>();

    private int myint;
    private int myagi;
    private int mystr;

    private bool begregnMax = true;
    
    void Start()
    {
        tree = Talent.BaseTalent;
        pickedTalents.Push(tree);
        while (pickedTalents.Count > 0)
        {
            Talent current = pickedTalents.Pop();
            foreach (var talent in current.SubTalents)
            {
                    pickedTalents.Push(talent);
            }

            myint += current.Intellect;
            mystr += current.Strength;
            myagi += current.Agility;
            
            
        }
        
        
        Debug.Log(myint);
        Debug.Log(mystr);
        Debug.Log(myagi);
    }

    void Update()
    {
        
    }
}
