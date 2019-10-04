using UnityEngine;
using UnityEngine.EventSystems;

public class TabScrollButtonScript : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {
    private TabScrollScript tabScroll;
    private bool isLeft;
    private bool isRight;

    // Use this for initialization
    void Start()
    {
        tabScroll = transform.GetComponentInParent<TabScrollScript>();
        isLeft = transform.name.Equals("TabScrollLeft");
        isRight = transform.name.Equals("TabScrollRight"); ;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (isLeft) {
            tabScroll.GoLeft = true;
            tabScroll.GoRight = false;
        } else if (isRight) {
            tabScroll.GoRight = true;
            tabScroll.GoLeft = false;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        tabScroll.GoLeft = false;
        tabScroll.GoRight = false;
    }
}
