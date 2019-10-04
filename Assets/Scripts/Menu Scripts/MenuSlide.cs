using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSlide : MonoBehaviour {
    public void DisableBoolInAnimator(Animator anim)
    {
        anim.SetBool("isDisplayed", false);
    }

    public void EnableBoolInAnimator(Animator anim)
    {
        anim.SetBool("isDisplayed", true);
    }
}
