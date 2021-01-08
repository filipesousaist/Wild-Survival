using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockWave : Ability
{
    public ParticleSystem particleSystem;

    float shockWaveStart;

    public CircleCollider2D col;

    public float maxRadius;

    RhinoMovement rhino;

    protected override void Start()
    {
        base.Start();
        particleSystem.Stop();
        rhino = GetComponentInParent<RhinoMovement>();
    }

    protected override void Update()
    {
        base.Update();
        if (active && Time.time - shockWaveStart >= particleSystem.main.duration)
        {
            Deactivate();
        }
        else if (active)
        {
            col.radius = Mathf.Min(col.radius + maxRadius * Time.deltaTime, maxRadius);
            rhino.attackedRecently = false;
        }
    }
    protected override void Effect()
    {
        base.Effect();
        rhino.attackedRecently = false;
        //particleSystem.enableEmission = true;
        particleSystem.Play();
        shockWaveStart = Time.time;
        col.gameObject.SetActive(true);
    }

    protected override void Deactivate()
    {
        base.Deactivate();
        col.radius = 0.01f;
        col.gameObject.SetActive(false);
        rhino.attackedRecently = false;
        //particleSystem.emission.enabled = false;
    }
}
