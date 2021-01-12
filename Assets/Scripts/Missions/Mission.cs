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

    public virtual void OnBegin() { }
    public virtual void OnFinish() { }
}
