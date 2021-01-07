using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability : MonoBehaviour
{
    public float cooldown;
    protected float lastUsed = 0.0f; //usado para calcular o tempo em que esta ativo e o tempo em que fica em cooldown

    protected virtual void Start()
    {
        lastUsed = 0.0f;
    }

    protected virtual void Update()
    {
        
    }
    public virtual void Activate()
    {
        if (Time.time - lastUsed > cooldown)
        {
            Effect();
        }
    }

    protected virtual void Effect() {
        lastUsed = Time.time;
    }

    protected virtual void Deactivate() { }
}
