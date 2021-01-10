public class UpgradeTable : SimpleBuilding
{
    protected override void OnUpgrade()
    {
        if (level > 1)
            level = 1;
    }
}
