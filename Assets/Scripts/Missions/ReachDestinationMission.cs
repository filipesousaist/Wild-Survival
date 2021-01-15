using UnityEngine.UI;
using UnityEngine;

public class ReachDestinationMission : HelpArrowMission
{
    public Vector2 exitCoords;

    public ActivistsManager activistsManager;

    private bool hasExited;

    protected override void OnBegin()
    {
        base.OnBegin();
        helpArrow.GetComponent<Image>().color = Colors.BLUE;
        hasExited = false;
    }

    private void Update()
    {
        Vector2 playerPos = activistsManager.GetCurrentPlayerMovement().transform.position;

        if ((playerPos - exitCoords).magnitude < 0.5)
            hasExited = true;
    }

    public override bool IsCompleted()
    {
        return hasExited;
    }

    public override void UpdateHelpArrow()
    {
        UpdateHelpArrow(exitCoords);
    }

    public override string GetMessage()
    {
        int num = IsCompleted() ? 1 : 0;
        return "Investigate path that opened up: " + num + "/1";
    }
}
