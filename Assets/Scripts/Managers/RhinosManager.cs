using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class RhinosManager : MonoBehaviour
{
    public List<RhinoMovement> rhinos;
    public Light2D[] light2Ds;

    private GameObject rhinoInfoHolder;
    private ActivistsManager activistsManager;

    void Awake()
    {
        rhinos = new List<RhinoMovement>(GetComponentsInChildren<RhinoMovement>());
        light2Ds = GetComponentsInChildren<Light2D>();

        rhinoInfoHolder = GameObject.Find("RhinoInfoHolder");
        activistsManager = FindObjectOfType<ActivistsManager>();
    }

    void Update()
    {
        // can be changed to a light script where each light will have the script avoiding this for cycle
        for (var i = 0; i < light2Ds.Length; i++) {
            light2Ds[i].transform.position = rhinos[i].transform.position;
        }
    }

    public void HealAll()
    {
        for (int i = 0; i < rhinos.Count; i++)
        {
            Rhino rhino = rhinos[i].GetComponent<Rhino>();
            rhino.FullRestore();
        }
    }

    public Rhino GetCurrentRhino()
    {
        return activistsManager.GetCurrentPlayer().rhino;
    }

    // Can remove when all activists have a rhino
    public void ToggleRhinoInfo()
    {
        rhinoInfoHolder.transform.localScale = GetCurrentRhino() != null ? Vector3.one : Vector3.zero;
    }
}
