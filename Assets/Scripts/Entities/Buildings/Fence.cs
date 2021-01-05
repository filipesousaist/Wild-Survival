
using System.Collections;

public class Fence : TilemapBuilding
{
    protected override void OnUpgrade()
    {
        maxHealth += 50;
    }
}
