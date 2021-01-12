using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Mission : MonoBehaviour
{
    public abstract bool IsCompleted();
    public virtual string GetMessage()
    {
        return "";
    }

    public IEnumerator Begin()
    {
        yield return null;
        gameObject.SetActive(true);
        OnBegin();
    }

    public IEnumerator Finish()
    {
        OnFinish();
        gameObject.SetActive(false);
        yield return null;
    }


    protected virtual void OnBegin() { }
    protected virtual void OnFinish() { }

    public virtual void UpdateHelpArrow(GameObject arrow) { }
}
