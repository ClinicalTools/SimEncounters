using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CEScrollRect : ScrollRect
{
    private const float SNAP_BUFFER = 10;

    protected override void Awake()
    {
        // The movement is restricted in LateUpdate by this script
        // The restriction acts like clamped, but without restarting from the top when it moves too far down.
        movementType = MovementType.Unrestricted;

        base.Awake();
    }

    /// <summary>
    /// Moves the scrollrect so that the top target is in sight if it was previously below or the bottom target is in sight if it was previously above.
    /// </summary>
    /// <param name="topTarget">Topmost rectangle that should be in sight.</param>
    /// <param name="botTarget">Bottommost rectangle that should be in sight.</param>
    public void SnapTo(RectTransform topTarget, RectTransform botTarget)
    {
        Canvas.ForceUpdateCanvases();

        Vector3[] v = new Vector3[4];
        topTarget.GetWorldCorners(v);
        Vector3 topCorner = v[1];

        var topY2 = -viewport.transform.InverseTransformPoint(topCorner).y;

        var topOffset = topTarget.rect.y - topTarget.localPosition.y;
        var topY = -viewport.transform.InverseTransformPoint(topTarget.position).y + topOffset;
        var botOffset = botTarget.rect.y - botTarget.localPosition.y + botTarget.rect.height;
        var botY = -viewport.transform.InverseTransformPoint(botTarget.position).y + botOffset;

        if (topY2 < 0) {
            Vector2 pos =
                (Vector2)transform.InverseTransformPoint(content.position)
                - (Vector2)transform.InverseTransformPoint(topCorner);
            pos.x = content.anchoredPosition.x;
            pos.y -= SNAP_BUFFER;
            content.anchoredPosition = pos;
        } else {
            botTarget.GetWorldCorners(v);
            Vector3 botCorner = v[0];
            var botY2 = -viewport.transform.InverseTransformPoint(botCorner).y;
            if (botY2 > viewport.rect.height) {
                Vector2 pos =
                    (Vector2)transform.InverseTransformPoint(content.position)
                    - (Vector2)transform.InverseTransformPoint(botCorner);
                pos.x = content.anchoredPosition.x;
                pos.y -= viewport.rect.height;
                pos.y += SNAP_BUFFER;

                content.anchoredPosition = pos;
            }
        }
    }

    public override void OnScroll(PointerEventData data)
    {
        if (content.rect.height > viewport.rect.height)
            base.OnScroll(data);
    }

    private bool setNextPos;
    private float nextY;
    protected override void LateUpdate()
    {
        if (setNextPos) {
            content.anchoredPosition = new Vector2(content.anchoredPosition.x, nextY);
            setNextPos = false;
        }

        var yPos = content.anchoredPosition.y;
        // It needs a buffer to prevent looping when getting to the end. The buffer needed seems to scale with the heights
        // .9999f seems to work well without revealing that you can't scroll to the very end. .99999f is too small a buffer
        // Unfortunately this doesn't work for very small ones, so a hard buffer of .02 is used
        var heightDif = (content.rect.height - viewport.rect.height);
        var buffer = heightDif * .0001f;
        if (buffer < .02f)
            buffer = .02f;
        var maxY = heightDif - buffer;

        if (content.anchorMin.y > .5f) {
            if (yPos < 0) {
                content.anchoredPosition = new Vector2(content.anchoredPosition.x, 0);
                velocity = new Vector2();
            } else if (maxY > 1f && yPos > maxY) {
                content.anchoredPosition = new Vector2(content.anchoredPosition.x, maxY);
            }
        } else {
            if (maxY > 1f && yPos < -maxY) {
                content.anchoredPosition = new Vector2(content.anchoredPosition.x, -maxY);
                velocity = new Vector2();
            } else if (yPos > 0) {
                content.anchoredPosition = new Vector2(content.anchoredPosition.x, 0);
            }
        }

        base.LateUpdate();

        // If the position isn't set at the beginning of the next update, 
        // the content may be reset to the top after dragging to the bottom
        yPos = content.anchoredPosition.y;
        if (content.anchorMin.y > .5f) {
            if (maxY > 1f && yPos > maxY) {
                content.anchoredPosition = new Vector2(content.anchoredPosition.x, maxY);
                velocity = new Vector2();
                setNextPos = true;
                nextY = maxY;
            } else if (yPos < 0) {
                content.anchoredPosition = new Vector2(content.anchoredPosition.x, 0);
                velocity = new Vector2();
                setNextPos = true;
                nextY = 0;
            }
        } else {
            if (maxY > 1f && yPos < -maxY) {
                content.anchoredPosition = new Vector2(content.anchoredPosition.x, -maxY);
                velocity = new Vector2();
                setNextPos = true;
                nextY *= -maxY;
            } else if (yPos > 0) {
                content.anchoredPosition = new Vector2(content.anchoredPosition.x, 0);
                velocity = new Vector2();
                setNextPos = true;
                nextY = 0;
            }
        }
    }
}
