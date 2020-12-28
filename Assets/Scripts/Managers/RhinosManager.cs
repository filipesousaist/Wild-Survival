using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class RhinosManager : MonoBehaviour
{
    public RhinoMovement[] rhinos;
    public Light2D[] light2Ds;

    // Start is called before the first frame update
    void Start()
    {
        rhinos = GetComponentsInChildren<RhinoMovement>();
        light2Ds = GetComponentsInChildren<Light2D>();
    }

    void Update()
    {
        for (var i = 0; i < light2Ds.Length; i++) {
            light2Ds[i].transform.position = rhinos[i].transform.position;
        }
    }

    public void HealAll()
    {
        for (int i = 0; i < rhinos.Length; i++)
        {
            Rhino rhino = rhinos[i].GetComponent<Rhino>();
            rhino.FullRestore();
        }
    }
}
