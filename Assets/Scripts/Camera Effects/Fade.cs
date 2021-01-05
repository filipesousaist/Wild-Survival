using System.Collections;
using UnityEngine;


public class Fade : MonoBehaviour
{
    private Animator anim;

    private bool isFading = false;

    void Start()
    {
        anim = GetComponent<Animator>();
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
