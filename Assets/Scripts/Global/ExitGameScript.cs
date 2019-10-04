using UnityEngine;
using UnityEngine.UI;

public class ExitGameScript : MonoBehaviour {
    [SerializeField]
    private bool hideOnWindowed = false;
    private Button btn;
    private CanvasGroup grp;

    private void Awake()
    {
        btn = GetComponent<Button>();
        grp = GetComponent<CanvasGroup>();
    }

    private void OnEnable()
    {
        if (hideOnWindowed) {
            if (!Screen.fullScreen) {
                grp.interactable = false;
                grp.blocksRaycasts = false;
                grp.alpha = 0;
            } else {
                grp.interactable = true;
                grp.blocksRaycasts = true;
                grp.alpha = 1;
            }
        }
    }

    private void Update()
    {
        if (hideOnWindowed) {
            if (!Screen.fullScreen) {
                grp.interactable = false;
                grp.blocksRaycasts = false;
                grp.alpha = 0;
            } else {
                grp.interactable = true;
                grp.blocksRaycasts = true;
                grp.alpha = 1;
            }
        }
    }

    public void ConfirmExit()
    {
        GameObject panel = Instantiate(Resources.Load("Writer/Prefabs/Panels/ConfirmActionBG"), GameObject.Find("GaudyBG").transform) as GameObject;
        panel.name = "ConfirmActionBG";
        ConfirmationScript cs = panel.GetComponent<ConfirmationScript>();

        cs.actionText.text = "Are you sure you want to quit?";
        cs.obj = null;
        cs.MethodToConfirm = ExitApplication;
    }


    public void ExitApplication(Object INeedToPutThisHereSoItWillCompile)
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
}
