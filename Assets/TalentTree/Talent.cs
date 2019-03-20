using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Talent", menuName = "SPPT/Talent", order = 1)]
public class Talent : ScriptableObject
{
    public bool Picked;

    public int Strength;
    public int Intellect;
    public int Agility;

    public Sprite Sprite;

    public Talent[] SubTalents;
}
