using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AccountDropdown : MonoBehaviour
{
    public TextMeshProUGUI username;

    public Image userImg, offlineImg;
    public ContentTipInfoScript offlineHover;

    public RectTransform dropdown;
    public GameObject blocker;

    public CanvasGroup optionGroup;

    public GameObject settingsBtn, reconnectBtn, switchAppBtn, fullscreenBtn;

    public GameObject sceneSwitch;

    private TextMeshProUGUI fullscreenText;

    private void Awake()
    {
        fullscreenText = fullscreenBtn.GetComponentInChildren<TextMeshProUGUI>();
    }


    private void Update()
    {
        username.text = GlobalData.username;

        userImg.enabled = !GlobalData.offline;
        offlineImg.enabled = GlobalData.offline;
        offlineHover.enabled = GlobalData.offline && !expanded;

        fullscreenText.text = Screen.fullScreen ? "Windowed" : "Fullscreen";

        switchAppBtn.SetActive(!sceneSwitch.activeInHierarchy);
        reconnectBtn.SetActive(GlobalData.offline);
        settingsBtn.SetActive(!(GlobalData.role == GlobalData.Roles.Guest || GlobalData.offline));
    }
    
    private bool expanded = false;
    public void ToggleExpand()
    {
        if (GlobalData.offline)
            offlineHover.OnPointerExit(null);

        if (expanded)
            StartCoroutine(Shrinking());
        else
            StartCoroutine(Expanding());

        expanded = !expanded;
        blocker.SetActive(expanded);
    }

    public void Close()
    {
        expanded = false;
        var offset = dropdown.offsetMax;
        offset.y = 1000;
        dropdown.offsetMax = offset;
        blocker.SetActive(false);
    }

    private readonly float speed = .11f;
    private bool moving = false;
    private IEnumerator Expanding()
    {
        while (moving)
            yield return null;

        moving = true;
        var height = dropdown.rect.height;
        var offset = dropdown.offsetMax;
        if (offset.y > height) {
            offset.y = height;
            dropdown.offsetMax = offset;
        }

        while (dropdown.offsetMax.y > 0) {
            yield return new WaitForEndOfFrame();

            if (!expanded) {
                moving = false;
                yield break;
            }

            offset.y -= height * Time.deltaTime / speed;
            dropdown.offsetMax = offset;
        }

        offset.y = 0;
        dropdown.offsetMax = offset;

        moving = false;
    }

    private IEnumerator Shrinking()
    {
        while (moving)
            yield return null;

        moving = true;
        var height = dropdown.rect.height;
        var offset = dropdown.offsetMax;
        while (dropdown.offsetMax.y < height) {
            yield return new WaitForEndOfFrame();

            if (expanded) {
                moving = false;
                yield break;
            }

            offset.y += height * Time.deltaTime / speed;
            dropdown.offsetMax = offset;
        }

        offset.y = 1000;
        dropdown.offsetMax = offset;
        moving = false;
    }
}
