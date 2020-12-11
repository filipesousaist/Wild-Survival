using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhinosManager : MonoBehaviour
{
    public RhinoMovement[] rhinos;

    // Start is called before the first frame update
    void Start()
    {
        rhinos = GetComponentsInChildren<RhinoMovement>();
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
