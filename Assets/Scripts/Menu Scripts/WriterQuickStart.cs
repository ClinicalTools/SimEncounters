using UnityEngine;

public class WriterQuickStart : MonoBehaviour
{
    [SerializeField]
    private GameObject TemplateCasePage = null;
    [SerializeField]
    private GameObject NewCasePage = null;
    [SerializeField]
    private GameObject RegisterPage = null;
    [SerializeField]
    private GameObject TutorialPage = null;

    public void NextPage()
    {
        if (TemplateCasePage.activeSelf) {
            TemplateCasePage.SetActive(false);
            NewCasePage.SetActive(true);
        } else if (NewCasePage.activeSelf) {
            NewCasePage.SetActive(false);

            if (GlobalData.username == "Guest")
                RegisterPage.SetActive(true);
            else
                TutorialPage.SetActive(true);
        } else if (RegisterPage.activeSelf) {
            RegisterPage.SetActive(false);
            TutorialPage.SetActive(true);
        } else if (TutorialPage.activeSelf) {
            TutorialPage.SetActive(false);
            TemplateCasePage.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
