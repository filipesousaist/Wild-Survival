using UnityEngine;
using UnityEngine.UI;

public class CaptureRhinoMission : HelpArrowMission
{
    public GameObject rhinoObj;
    public Player player;
    protected override void OnBegin()
    {
        base.OnBegin();
        rhinoObj.SetActive(true);
        helpArrow.GetComponent<Image>().color = Colors.BLUE;
    }

    public override bool IsCompleted()
    {
        return player.HasRhino();
    }

    public override void UpdateHelpArrow()
    {
        if (!IsCompleted())
        {
            SetArrowPosition(rhinoObj.transform.position - Camera.main.transform.position);
            helpArrow.SetActive(true);
        }
        else
            helpArrow.SetActive(false);
    }
    public override string GetMessage()
    {
        int num = IsCompleted() ? 1 : 0;
        return "Find a rhino for " + player.entityName + ": " + num + "/1";
    }
}
