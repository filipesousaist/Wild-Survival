using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Fade : MonoBehaviour
{
    public GameObject blackoutSquare;

    private Animator anim;

    private bool isFading = false;

    void Start()
    {
        anim = Canvas.FindObjectOfType<Animator>();
    }

    public IEnumerator FadeToClear() {
        isFading = true;
        anim.SetTrigger("FadeIn");

        while (isFading)
        {
            yield return null;
        }
    }

    public IEnumerator FadeToBlack () {
        isFading = true;
        anim.SetTrigger("FadeOut");

        while (isFading)
        {
            yield return null;
        }
    }

    public void AnimationCompleted()
    {
        isFading = false;

    }
}
