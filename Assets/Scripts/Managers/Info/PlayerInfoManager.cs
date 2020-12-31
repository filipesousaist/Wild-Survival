public class PlayerInfoManager : CharacterInfoManager
{
    protected override Character GetCurrentCharacter()
    {
        return activistsManager.GetCurrentPlayer();
    }
}
