using System.Linq;
public class ActivistsLevelUpMission : Mission
{
    public int targetLevel;
    public int numberOfActivists;

    public ActivistsManager activistsManager;

    private int CountLeveledActivists()
    {
        return activistsManager.players.Count(player => player.level >= targetLevel);
    }

    public override bool IsCompleted()
    {
        return CountLeveledActivists() >= numberOfActivists;
    }

    public override string GetMessage()
    {
        return "Reach level " + targetLevel + " with " + numberOfActivists + " activists: " +
            CountLeveledActivists() + "/" + numberOfActivists;
    }
}
