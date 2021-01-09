using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heal : Ability
{
    public float radius;

    public float healValue;

    public ParticleSystem healPrefab;
    
    RhinosManager rhinosManager;

    ActivistsManager activistsManager;
    
    RhinoMovement rhino;

    List<ParticleSystem> heals;

    float healStart;

    float duration;

    protected override void Start()
    {
        base.Start();
        rhino = GetComponentInParent<RhinoMovement>();
        rhinosManager = FindObjectOfType<RhinosManager>();
        activistsManager = FindObjectOfType<ActivistsManager>();
        duration = healPrefab.main.duration;
        heals = new List<ParticleSystem>();
        healPrefab.Play();
    }

    protected override void Update()
    {
        base.Update();
        if (active && Time.time - healStart >= duration)
        {
            Deactivate();
        }
    }
    protected override void Effect()
    {
        base.Effect();
        foreach (var item in rhinosManager.rhinos)
        {
            var difference = item.transform.position - rhino.transform.position;
            if(difference.magnitude <= radius)
            {
                item.GetComponent<Rhino>().Heal(healValue);
                var heal = Instantiate(healPrefab, item.transform.position, Quaternion.identity);
                heal.transform.parent = item.transform;
                heal.transform.localPosition = new Vector3(0, -1, 0);
                heal.transform.Rotate(-90, 0, 0);
                heal.transform.localScale = Vector3.one;
                var healPart = heal;
                healPart.Play();
                heals.Add(healPart);
            }
        }

        foreach (var item in activistsManager.playerMovs)
        {
            var difference = item.transform.position - rhino.transform.position;
            if (difference.magnitude <= radius)
            {
                item.GetComponent<Player>().Heal(healValue);
                var heal = Instantiate(healPrefab, item.transform.position, Quaternion.identity);
                heal.transform.parent = item.transform;
                heal.transform.localPosition = new Vector3(0, -1, 0);
                heal.transform.Rotate(-90, 0, 0);
                heal.transform.localScale = Vector3.one;
                var healPart = heal;
                healPart.Play();
                heals.Add(healPart);
            }
        }


        healStart = Time.time;
    }

    protected override void Deactivate()
    {
        base.Deactivate();
        for (int i = 0; i < heals.Count; i++)
        {
            Destroy(heals[i].gameObject);
        }
        heals.Clear();
    }
}
