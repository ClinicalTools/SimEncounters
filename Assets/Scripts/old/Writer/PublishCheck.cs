using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PublishCheck : MonoBehaviour
{
    private Button publishBtn;
    public TextMeshProUGUI publishText;

    private void Awake()
    {
        publishBtn = GetComponent<Button>();
    }

    private void OnEnable()
    {
        if (GlobalData.role == GlobalData.Roles.Guest) {
            publishText.text = "Cannot publish cases as a guest";
            publishBtn.interactable = false;
        } else if (GlobalData.offline) {
            publishText.text = "Cannot publish cases while offline";
            publishBtn.interactable = false;
        } else {
            publishText.text = "";
            publishBtn.interactable = true;
        }
    }
}
