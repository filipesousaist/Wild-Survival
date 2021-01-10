using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Capture : Interactable
{
    Rhino rhino;

    RhinoMovement rhinoMovement;

    Player player;

    private void Start()
    {
        rhino = GetComponentInParent<Rhino>();
        rhinoMovement = GetComponentInParent<RhinoMovement>();
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        var entity = other.gameObject.GetComponent<Player>();
        if (!entity.HasRhino()) { 
            base.OnTriggerEnter2D(other);
            player = entity;
        }
    }

    protected void OnTriggerStay2D(Collider2D other)
    {
        var entity = other.gameObject.GetComponent<Player>();
        if (!entity.HasRhino())
        {
            base.OnTriggerEnter2D(other);
            player = entity;
        }
    }

    protected override IEnumerator OnInteract()
    {
        yield return base.OnInteract();
        if (!player.HasRhino())
        {
            CaptureRhino();
        }
    }

    void CaptureRhino() {
        player.SetRhino(rhino);
        rhino.SetOwner(player);
    }
}
