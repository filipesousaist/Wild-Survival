using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public Stat damage;
    public Stat armor;

    public void UpgradeDamage(float value)
    {
        damage.UpdateBaseValue(value);
    }

    public void UpgradeArmor(float value)
    {
        armor.UpdateBaseValue(value);
    }
}
