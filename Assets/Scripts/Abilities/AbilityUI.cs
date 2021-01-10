using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//https://www.youtube.com/watch?v=1fBKVWie8ew
public class AbilityUI : MonoBehaviour
{
    public RhinosManager rhinos;

    List<AbilitySlot> abilities;

    public Transform parent;

    public AbilitySlot abilitySlotPrefab;

    Rhino currentRhino;
    // Start is called before the first frame update
    void Start()
    {
        abilities = new List<AbilitySlot>();
        currentRhino = rhinos.GetCurrentRhino();
        if (currentRhino != null)
        {
            UpdateUI();
        }
    }

    // Update is called once per frame
    void Update()
    {
        var newRhino = rhinos.GetCurrentRhino();
        if (newRhino != null)
        {
            if (currentRhino != newRhino)
            {
                currentRhino = newRhino;
                UpdateUI();
            }
            foreach (var item in abilities)
            {
                item.UpdateCooldown();
            }
        }
        else {
            for (int i = 0; i < abilities.Count; i++)
            {
                Destroy(abilities[i].gameObject);
            }
            abilities.Clear();
        }
    }

    public void UpdateUI() {
        List<Ability> abs = currentRhino.GetAbilities();
        for (int i = 0; i < abilities.Count; i++)
        {
            if (i >= abs.Count)
            {
                break;
            }
            abilities[i].AddAbility(abs[i]);
        }
        if (abilities.Count < abs.Count)
        {
            for (int i = abilities.Count; i < abs.Count; i++)
            {
                AbilitySlot slot = Instantiate(abilitySlotPrefab);
                slot.transform.SetParent(parent);
                slot.transform.localScale = new Vector3(1, 1, 1);
                slot.AddAbility(abs[i]);
                abilities.Add(slot);
            }
        }
        if (abilities.Count > abs.Count)
        {
            for (int i = abilities.Count -1; i >= abs.Count; i--)
            {
                var slot = abilities[i];
                abilities.RemoveAt(i);
                Destroy(slot.gameObject);
            }
        }
    }
}
