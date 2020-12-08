using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float lastAttackTime;
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        lastAttackTime = Time.time;
    }

}
