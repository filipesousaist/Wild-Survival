using UnityEngine;
using System.Linq;

public class CollectMaterialsMission : Mission
{
    public MatDict[] materials;

    public override bool IsCompleted()
    {
        return materials.All(mat => Inventory.instance.Count(mat.item) >= mat.n);
    }

    public string GetMatCountStr(MatDict mat)
    {
        return mat.item.name + ": " + Inventory.instance.Count(mat.item) + "/" + mat.n;
    }

    public override string GetMessage()
    {
        string message = "Collect materials. " + GetMatCountStr(materials[0]);
        foreach (MatDict mat in materials.Skip(1))
            message += ", " + GetMatCountStr(mat);
        return message;
    }
}