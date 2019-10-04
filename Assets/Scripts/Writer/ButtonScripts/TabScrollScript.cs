using UnityEngine;
using UnityEngine.UI;

public class TabScrollScript : MonoBehaviour {
    private Scrollbar scroll;
    private GameObject leftButton;
    private GameObject rightButton;
    private readonly float speed = 1;
    public bool GoLeft { get; set; }
    public bool GoRight { get; set; }

    void Start()
    {
        leftButton = transform.Find("TabScrollLeft").gameObject;
        rightButton = transform.Find("TabScrollRight").gameObject;

        var scrollbar = transform.Find("ScrollBar");
        scroll = GetComponent<Scrollbar>();


        if (!scroll)
            scroll = transform.Find("ScrollBar").GetComponent<Scrollbar>();
    }

    private void Update()
    {
        if (GoLeft) {
            scroll.value -= Time.deltaTime * speed / (1 - scroll.size);
            if (scroll.value <= 0.01) {
                GoLeft = false;
            }
        } else if (GoRight) {
            scroll.value += Time.deltaTime * speed / (1 - scroll.size);
            if (scroll.value >= .99) {
                GoRight = false;
            }
        }

        if (scroll.size < .99f) {
            if (scroll.value <= .01f) {
                leftButton.SetActive(false);
            } else {
                leftButton.SetActive(true);
            }
            if (scroll.value >= .99f) {
                rightButton.SetActive(false);
            } else {
                rightButton.SetActive(true);
            }
        } else {
            leftButton.SetActive(false);
            rightButton.SetActive(false);
        }
    }
}
