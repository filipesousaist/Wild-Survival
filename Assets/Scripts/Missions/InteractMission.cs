using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractMission : HelpArrowMission
{
    public Interactable interactable;

    public override bool IsCompleted()
    {
        return interactable.hasInteracted;
    }

    public override void UpdateHelpArrow()
    {
        if (!IsCompleted())
        {
            SetArrowPosition(interactable.gameObject.transform.position - Camera.main.transform.position);
            helpArrow.SetActive(true);
        }
        else
            helpArrow.SetActive(false);
    }
}
