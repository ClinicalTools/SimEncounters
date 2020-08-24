using UnityEngine;

/// <summary>
/// Handles hiding/showing an object when logged in as a guest, as well as changing the dimensions of this object.
/// </summary>
public class HideOnGuest : MonoBehaviour
{
    [SerializeField] private GameObject hiddenObj = null;
    [SerializeField] private float guestWidth = 0f;
    [SerializeField] private float largeWidth = 0f;


    private bool wasGuest;
    private RectTransform RectTrans => (RectTransform)transform;
    private void Start()
    {
        // Ensures that it always detects was and isGuest on Awake
        wasGuest = GlobalData.username != "Guest";

        Update();
    }

    private void OnEnable()
    {
        Update();
    }

    private void Update()
    {
        var isGuest = GlobalData.username == "Guest";
        hiddenObj.SetActive(!isGuest);

        if (isGuest != wasGuest) {
            if (isGuest) {
                if (guestWidth >= 1f)
                    RectTrans.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, guestWidth);
            } else {
                if (largeWidth >= 1f)
                    RectTrans.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, largeWidth);
            }
            wasGuest = isGuest;
        }
    }
}
