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

    public static Talent TalentTreeRoot => UnityEngine.Resources.Load<Talent>("TalentAssets/Base");
}
