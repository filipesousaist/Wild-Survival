using System.Collections;
using UnityEngine;
using System.Linq;

public class Capture : Interactable
{
    Rhino rhino;

    Player player;

    protected override void OnAwake()
    {
        rhino = GetComponentInParent<Rhino>();
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        Player entity = other.gameObject.GetComponent<Player>();
        if (entity != null && !entity.HasRhino()) { 
            base.OnTriggerEnter2D(other);
            player = entity;
        }
    }

    protected void OnTriggerStay2D(Collider2D other)
    {
        Player entity = other.gameObject.GetComponent<Player>();
        if (entity != null && !entity.HasRhino())
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
        FindObjectOfType<RhinosManager>().rhinos.Add(rhino.GetComponent<RhinoMovement>());
    }

    public override string GetInteractText()
    {
        return "Capture";
    }
}
