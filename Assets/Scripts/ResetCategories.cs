using UnityEngine;
using UnityEngine.UI;

public class ResetCategories : MonoBehaviour
{
    public GameObject filtersPanel;

    // Update is called once per frame
    private void OnEnable()
    {
        foreach (var filter in filtersPanel.GetComponentsInChildren<Toggle>())
            filter.isOn = false;
    }
}
