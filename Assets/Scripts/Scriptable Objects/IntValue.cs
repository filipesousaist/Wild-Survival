using UnityEngine;

[CreateAssetMenu]
public class IntValue : ScriptableObject, ISerializationCallbackReceiver
{
    public int initialValue;

    [HideInInspector]
    public int value;

    public void OnAfterDeserialize()
    {
        value = initialValue;
    }

    public void OnBeforeSerialize() { }
}
