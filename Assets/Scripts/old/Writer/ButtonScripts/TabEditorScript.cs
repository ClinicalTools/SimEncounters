using UnityEngine;
using TMPro;

public class TabEditorScript : MonoBehaviour
{

    GameObject BG;
    EditTabScript ets;
    TMP_InputField titleValue;

    // Use this for initialization
    void Start()
    {
        BG = GameObject.Find("GaudyBG");
        ets = BG.GetComponent<EditTabScript>();
        if (transform.Find("TabEditorPanel/Content/Row0/TMPInputField/TitleValue")) {
            titleValue = transform.Find("TabEditorPanel/Content/Row0/TMPInputField/TitleValue").GetComponent<TMP_InputField>();
            titleValue.text = BG.GetComponent<TabManager>().getCurrentTab();
        }
    }

    public void enableTabSelection()
    {
        GameObject tabSelectorPrefab = Instantiate(Resources.Load("Writer/Prefabs/Panels/TabSelectorBG")) as GameObject;
        tabSelectorPrefab.transform.SetParent(BG.transform, false);
    }

    public void enableSectionSelection()
    {
        GameObject sectionSelectorPrefab = Instantiate(Resources.Load("Writer/Prefabs/Panels/SectionCreatorBG")) as GameObject;
        sectionSelectorPrefab.transform.SetParent(BG.transform, false);
    }

    public void disableTabSelection()
    {
        Destroy(this.gameObject);
    }

    public void OpenTabEditor(TextMeshProUGUI t)
    {
        ets.OpenTabPanel(t);
    }

    public void Submit()
    {
        ets.SubmitChanges();
    }

    public void Remove()
    {
        ets.removeTab();
    }
}
