using UnityEngine.UI;

public class RottenFleshMission : InteractMission
{
    protected override void OnBegin()
    {
        base.OnBegin();
        interactable.gameObject.SetActive(true);
        helpArrow.GetComponent<Image>().color = Colors.BLUE;
        FindObjectOfType<MissionsManager>().startedRottenFleshMission = true;
    }

    public override string GetMessage()
    {
        int num = IsCompleted() ? 1 : 0;
        return "Investigate strange sparkles at the edge of the savannah: " + num + "/1"; 
    }

    public override string GetFinishMessage()
    {
        return "Oh no! Rotten flesh will attract even more zombies!";
    }
}
