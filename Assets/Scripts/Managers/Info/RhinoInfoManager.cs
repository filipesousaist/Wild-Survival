using UnityEngine;

public class RhinoInfoManager : CharacterInfoManager
{
    protected override Character GetCurrentCharacter()
    {
        return activistsManager.GetCurrentPlayer().rhino;
    }
}
