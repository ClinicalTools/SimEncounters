using UnityEngine;
using UnityEngine.UI;

/**
 * Script to be called when using the section buttons
 */
public class ReaderSwapSectionScript : MonoBehaviour
{

    private ReaderTabManager BG;
    public TMPro.TextMeshProUGUI sectionName;	//Provided section name
    public TMPro.TextMeshProUGUI sectionDisplayText;
    public Image stepIcon;
    private Image sectionBtnBackground;

    // Use this for initialization
    void Awake()
    {
        BG = GameObject.Find("GaudyBG").GetComponentInChildren<ReaderTabManager>();
        sectionBtnBackground = GetComponent<Image>();
    }

    public void ChangeSection()
    {
        BG.SwitchSection(sectionName.text);
    }

    private Color secondaryColor = new Color(255, 255, 255);
    internal void SetColor(Color color, bool active)
    {
        if (active)
        {
            sectionBtnBackground.color = color;
            stepIcon.color = secondaryColor;
            sectionDisplayText.color = secondaryColor;
        }
        else
        {
            sectionBtnBackground.color = secondaryColor;
            stepIcon.color = color;
            sectionDisplayText.color = color;
        }
    }
}
