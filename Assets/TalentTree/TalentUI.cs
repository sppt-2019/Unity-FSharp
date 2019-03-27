using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class TalentUI : MonoBehaviour
{
    [SerializeField]
    private Image TalentIcon;
    [SerializeField]
    private Text TalentText;
    [SerializeField]
    private Image Background;
    [SerializeField]
    private Sprite PickedBackground;

    public void SetTalent(Talent talent)
    {
        TalentIcon.sprite = talent.Sprite;
        TalentText.text = GetTalentDescription(talent);
        if (talent.Picked)
            Background.sprite = PickedBackground;
    }

    private string GetTalentDescription(Talent talent)
    {
        var tsb = new StringBuilder();
        if (talent.Strength > 0)
            tsb.AppendLine(talent.Strength + " Strength");
        if (talent.Intellect > 0)
            tsb.AppendLine(talent.Intellect + " Intellect");
        if (talent.Agility > 0)
            tsb.AppendLine(talent.Agility + " Agility");

        return tsb.ToString();
    }

    public void Click()
    {
        Background.sprite = PickedBackground;
    }
}
