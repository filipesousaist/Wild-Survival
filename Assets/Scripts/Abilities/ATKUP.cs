using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ATKUP : Ability
{
    public float buffValue;

    RhinosManager rhinosManager;

    ActivistsManager activistsManager;

    Rhino rhino;

    public ParticleSystem buffEffect;

    float buffStart;

    float baseValue;

    public float duration;

    protected override void Start()
    {
        base.Start();
        rhino = GetComponentInParent<Rhino>();
        rhinosManager = FindObjectOfType<RhinosManager>();
        activistsManager = FindObjectOfType<ActivistsManager>();
        //duration = atkPrefab.main.duration;
        buffEffect.Stop();
        buffEffect.transform.localPosition = new Vector3(0, -1, 0);
        buffEffect.transform.Rotate(-90, 0, 0);
        buffEffect.transform.localScale = new Vector3(1.5f, 1.5f, 1);
    }

    protected override void Update()
    {
        base.Update();
        if (active && Time.time - buffStart >= duration)
        {
            Deactivate();
        }
    }
    protected override void Effect()
    {
        base.Effect();
        buffEffect.Play();
        baseValue = rhino.stats.damage.GetBaseValue();
        rhino.stats.damage.SetBaseValue(baseValue + buffValue);
        buffStart = Time.time;
    }

    protected override void Deactivate()
    {
        rhino.stats.damage.SetBaseValue(baseValue);
        base.Deactivate();
    }
}
