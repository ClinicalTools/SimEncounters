using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/**
 * Script to be called when using the section buttons
 */
public class SwapSectionScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI sectionName = null; //Provided section name

    protected string SectionKey { get; set; }
    public virtual void ChangeSection()
    {
        if (SectionKey == null)
            TabManager.Instance.SwitchSection(sectionName.text);
        else
            TabManager.Instance.SwitchSection(SectionKey);
    }

    public virtual void Initialize(string name, string key)
    {
        sectionName.text = name;
        SectionKey = key;
    }

    public virtual void SetImage(Color color)
    {
        GetComponent<Image>().color = color;
    }

    [SerializeField] private Image icon = null;
    public virtual void SetSprite(Sprite sprite)
    {
        icon.sprite = sprite;
    }
}
