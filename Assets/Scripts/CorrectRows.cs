using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CorrectRows : MonoBehaviour
{
    [SerializeField]
    private LayoutElement[] rows = new LayoutElement[0];
    private CanvasGroup[] groups;
    public CanvasGroup[] Groups {
        get {
            if (groups == null) {
                groups = new CanvasGroup[rows.Length];
                for (var i = 0; i < rows.Length; i++)
                    groups[i] = rows[i].GetComponent<CanvasGroup>();
            }

            return groups;
        }
    }

    private TMP_Dropdown dropdown;
    public TMP_Dropdown Dropdown {
        get {
            if (dropdown == null)
                dropdown = GetComponent<TMP_Dropdown>();

            return dropdown;
        }
    }

    public void UpdateRows()
    {
        if (Dropdown.value == 0) {
            for (var i = 0; i < rows.Length; i++) {
                rows[i].ignoreLayout = false;
                Groups[i].alpha = 1;

                var resizers = rows[i].GetComponentsInChildren<InputFieldResizer>();
                foreach (var resizer in resizers)
                    resizer.ResizeField();
            }
        } else {
            for (var i = 0; i < rows.Length; i++) {
                rows[i].ignoreLayout = true;
                Groups[i].alpha = 0;
            }
        }
    }
}
