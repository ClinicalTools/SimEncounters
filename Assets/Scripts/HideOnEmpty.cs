using UnityEngine;

public class HideOnEmpty : MonoBehaviour {
    public void Refresh()
    {
        foreach (Transform child in transform)
        {
            if (child.gameObject.activeSelf) 
                return;
        }

        gameObject.SetActive(false);
    }
}
