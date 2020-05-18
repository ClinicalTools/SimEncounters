using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class CollapseContentScript : MonoBehaviour
{
    private Image onImage;
    private Image offImage;
    private Toggle toggle;

    public List<GameObject> targets;

    public bool invertValue = false;
    public bool autoCollapse = true;
    public bool toggleGameObjects = false;

    // Use this for initialization
    void Awake()
    {
        if (transform.Find("On"))
            onImage = transform.Find("On").GetComponent<Image>();

        if (transform.Find("Off"))
            offImage = transform.Find("Off").GetComponent<Image>();

        toggle = GetComponent<Toggle>();

        if (autoCollapse)
            SetTarget(false);
    }

    public void ToggleTarget()
    {
        if (toggle) {
            var on = toggle.isOn;
            if (invertValue)
                on = !on;
            SetTarget(on);
        } else {
            Debug.Log(transform.gameObject.GetGameObjectPath() + ": Collapse script has no Toggle");
        }
    }

    public void SetTarget(bool toggleVal)
    {
        // This ensures the toggle's value always matches the correct value
        if (toggle && toggle.isOn != toggleVal) {
            toggle.isOn = toggleVal;
            // This method is called on value change, so return stops it from being ran twice
            return;
        }
        if (toggleVal) {
            foreach (GameObject target in targets) {
                ToggleImage(true);

                if (target == null)
                    continue;

                target.SetActive(true);
                var canvasGrp = target.GetComponent<CanvasGroup>();
                if (canvasGrp) {
                    canvasGrp.alpha = 1.0f;
                    canvasGrp.interactable = true;
                    canvasGrp.blocksRaycasts = true;
                }

                var layoutElement = target.GetComponent<LayoutElement>();
                if (layoutElement)
                    layoutElement.ignoreLayout = false;

                var horizontalLayoutGroup = target.GetComponent<HorizontalLayoutGroup>();
                if (horizontalLayoutGroup) {
                    horizontalLayoutGroup.enabled = true;
                }

                var verticalLayoutGroup = target.GetComponent<VerticalLayoutGroup>();
                if (verticalLayoutGroup)
                    verticalLayoutGroup.enabled = true;


                InputFieldResizer[] resizeScripts = target.GetComponentsInChildren<InputFieldResizer>();

                foreach (InputFieldResizer input in resizeScripts)
                    input.ResizeField();
            }
        } else {
            foreach (GameObject target in targets) {
                ToggleImage(false);
                if (target == null)
                    continue;

                target.SetActive(true);
                var canvasGrp = target.GetComponent<CanvasGroup>();
                if (canvasGrp) {
                    canvasGrp.alpha = 0.0f;
                    canvasGrp.interactable = false;
                    canvasGrp.blocksRaycasts = false;
                }

                var layoutElement = target.GetComponent<LayoutElement>();
                if (layoutElement) {
                    layoutElement.ignoreLayout = true;
                }
            }
        }
    }

    private void ToggleImage(bool toggle)
    {
        if (onImage == null) {
            var onTransform = transform.Find("On");
            if (onTransform)
                onImage = onTransform.GetComponent<Image>();
        }

        if (offImage == null) {
            var offTransform = transform.Find("Off");
            if (transform.Find("Off"))
                offImage = offTransform.GetComponent<Image>();
        }

        if (onImage == null || offImage == null)
            return;

        if (toggle) {
            var image = GetComponent<Image>();
            if (image != null && image.sprite != null) {
                image.sprite = onImage.sprite;
            } else {
                if (onImage != null && offImage != null) {
                    if (toggleGameObjects) {
                        onImage.gameObject.SetActive(true);
                        offImage.gameObject.SetActive(false);
                    } else {
                        onImage.enabled = true;
                        offImage.enabled = false;
                    }
                }
            }
        } else {
            var image = GetComponent<Image>();
            if (image != null && image.sprite != null) {
                image.sprite = offImage.sprite;
            } else if (onImage != null && offImage != null) {
                if (toggleGameObjects) {
                    onImage.gameObject.SetActive(false);
                    offImage.gameObject.SetActive(true);
                } else {
                    onImage.enabled = false;
                    offImage.enabled = true;
                }
            }
        }
    }
}
