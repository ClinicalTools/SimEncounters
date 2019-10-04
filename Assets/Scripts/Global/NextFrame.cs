using System.Collections;
using UnityEngine;
using System;

public class NextFrame : MonoBehaviour {
    public static NextFrame instance;

    // Use this for initialization
    void Awake() {
        instance = this;
    }

    /// <summary>
    /// Calls the specified function at the very end of the current frame.
    /// </summary>
    /// <param name="doThis"> Type the function you want to call. Does not support functions that require arguements.</param>
	public static void Function(Action doThis)
    {
        if (doThis != null && instance)
            instance.StartCoroutine(WaitforNext(doThis));
    }

    static IEnumerator WaitforNext(Action doThis)
    {
        yield return new WaitForEndOfFrame();
        doThis();
    }
}
