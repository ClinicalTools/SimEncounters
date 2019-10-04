using UnityEngine;

// Used to ensure this is only shown when the list view button is active
public class ViewBtnCheck : MonoBehaviour
{
    public ServerControls serverControls;
    void OnEnable()
    {
        if (serverControls.showingCourses)
            gameObject.SetActive(false);
    }
}