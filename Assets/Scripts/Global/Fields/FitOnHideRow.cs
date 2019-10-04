using UnityEngine.UI;

public class FitOnHideRow : TabRow {

    private ContentSizeFitter fitter;
    protected override void Awake()
    {
        fitter = GetComponent<ContentSizeFitter>();
        base.Awake();
    }
    protected virtual void Start()
    {
        Update();
    }

    private void Update()
    {
        if (Visible)
            fitter.enabled = false;
        else
            fitter.enabled = true;
    }
}
