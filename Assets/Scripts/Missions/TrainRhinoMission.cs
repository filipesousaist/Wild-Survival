public class TrainRhinoMission : Mission
{
    public int numAbilities;

    public ActivistsManager activistsManager;

    private int GetTotalAbilities()
    {
        int total = 0;
        foreach (Player activist in activistsManager.players)
            if (activist.HasRhino())
                total += activist.rhino.GetAbilities().Count;
        return total;
    }

    public override bool IsCompleted()
    {
        return GetTotalAbilities() >= numAbilities;
    }

    public override string GetMessage()
    {
        int num = GetTotalAbilities();
        string end = numAbilities == 1 ? "y" : "ies";
        return "Learn " + numAbilities + " rhino abilit" + end + ": " + num + "/" + numAbilities;
    }
}
