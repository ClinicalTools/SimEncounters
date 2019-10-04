using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class CollapseContentScript : MonoBehaviour {
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
        if (transform.Find("On")) {
            onImage = transform.Find("On").GetComponent<Image>();
        }

        if (transform.Find("Off")) {
            offImage = transform.Find("Off").GetComponent<Image>();
        }

        toggle = GetComponent<Toggle>();

        if (autoCollapse) {
            SetTarget(false);
        }
    }

    public void ToggleTarget()
    {
        if (toggle) {
            if (invertValue) {
                if (toggle.isOn) {
                    SetTarget(false);
                } else {
                    SetTarget(true);
                }
            } else {
                if (toggle.isOn) {
                    SetTarget(true);
                } else {
                    SetTarget(false);
                }
            }
        } else {
            Debug.Log(transform.gameObject.GetGameObjectPath() + ": Collapse script has no Toggle");
        }
    }

    public void SetTarget(bool toggleVal)
    {
        // This ensures the toggle's value always matches the correct value
        if (toggle) {
            if (toggle.isOn != toggleVal) {
                toggle.isOn = toggleVal;
                // This method is called on value change, so return stops it from being ran twice
                return;
            }
        }
        if (toggleVal) {
            foreach (GameObject target in targets) {

                if (target != null) {
                    target.SetActive(true);
                    var canvasGrp = target.GetComponent<CanvasGroup>();
                    if (canvasGrp) {
                        canvasGrp.alpha = 1.0f;
                        canvasGrp.interactable = true;
                        canvasGrp.blocksRaycasts = true;
                    }

                    if (target.GetComponent<HorizontalLayoutGroup>()) {
                        target.GetComponent<LayoutElement>().ignoreLayout = false;
                    }

                    if (target.GetComponent<HorizontalLayoutGroup>()) {
                        target.GetComponent<HorizontalLayoutGroup>().enabled = true;
                    }

                    if (target.GetComponent<VerticalLayoutGroup>()) {
                        target.GetComponent<LayoutElement>().ignoreLayout = false;
                    }

                    if (target.GetComponent<LayoutElement>()) {
                        target.GetComponent<LayoutElement>().ignoreLayout = false;
                    }

                    if (target.GetComponent<VerticalLayoutGroup>()) {
                        target.GetComponent<VerticalLayoutGroup>().enabled = true;
                    }


                    InputFieldResizer[] resizeScripts = target.GetComponentsInChildren<InputFieldResizer>();

                    foreach (InputFieldResizer input in resizeScripts) {
                        input.ResizeField();
                    }
                }

                ToggleImage(true);
            }
        } else {
            foreach (GameObject target in targets) {
                if (target != null) {
                    target.SetActive(true);
                    var canvasGrp = target.GetComponent<CanvasGroup>();
                    if (canvasGrp) {
                        canvasGrp.alpha = 0.0f;
                        canvasGrp.interactable = false;
                        canvasGrp.blocksRaycasts = false;
                    }

                    if (target.GetComponent<LayoutElement>()) {
                        target.GetComponent<LayoutElement>().ignoreLayout = true;
                    }

                    if (target.GetComponent<HorizontalLayoutGroup>()) {
                        //target.GetComponent<HorizontalLayoutGroup>().enabled = false;
                    }

                    if (target.GetComponent<VerticalLayoutGroup>()) {
                        //target.GetComponent<VerticalLayoutGroup>().enabled = false;
                    }
                }

                ToggleImage(false);
            }
        }
    }

    private void ToggleImage(bool toggle)
    {
        if (onImage == null) {
            if (transform.Find("On")) {
                onImage = transform.Find("On").GetComponent<Image>();
            }
        }

        if (offImage == null) {
            if (transform.Find("Off")) {
                offImage = transform.Find("Off").GetComponent<Image>();
            }
        }
        if (onImage == null || offImage == null) {
            return;
        }
        if (toggle) {
            if (GetComponent<Image>() != null && GetComponent<Image>().sprite != null) {
                GetComponent<Image>().sprite = onImage.sprite;
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
            if (GetComponent<Image>() != null && GetComponent<Image>().sprite != null) {
                GetComponent<Image>().sprite = offImage.sprite;
            } else {
                if (onImage != null && offImage != null) {
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
}
