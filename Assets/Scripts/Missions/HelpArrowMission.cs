using UnityEngine;

public abstract class HelpArrowMission : Mission
{
    public GameObject helpArrow;

    protected override void OnFinish()
    {
        helpArrow.SetActive(false);
    }

    protected void SetArrowPosition(Vector3 difference)
    {
        float angle = Vector3.SignedAngle(Vec3.E_Y, difference.normalized, Vec3.E_Z);
        helpArrow.transform.eulerAngles =
            new Vector3(0, 0, angle);
        helpArrow.transform.localPosition = new Vector3(
            -Window.QUARTER_H * Mathf.Sin(angle * Mathf.Deg2Rad),
            Window.QUARTER_H * Mathf.Cos(angle * Mathf.Deg2Rad),
            0);
    }

    protected void UpdateHelpArrow(Vector3 destination)
    {
        if (!IsCompleted() && !Cam.IsPointInView(destination))
        {
            SetArrowPosition(destination - Camera.main.transform.position);
            helpArrow.SetActive(true);
        }
        else
            helpArrow.SetActive(false);
    }
}
