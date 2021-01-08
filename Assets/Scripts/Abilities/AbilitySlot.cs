using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//https://www.youtube.com/watch?v=1fBKVWie8ew
public class AbilitySlot : MonoBehaviour
{
    Ability ability;
    public Image icon;
    public TMP_Text textCD;
    public Image iconCD;

    void Start() {
        textCD.gameObject.SetActive(false);
        iconCD.fillAmount = 0.0f;
    }

    public void AddAbility(Ability ab) {
        ability = ab;
        icon.sprite = ab.icon;
    }

    public bool IsActive() { 
        return ability.active;
    }

    public void UpdateCooldown() {
        if (ability.cooldownTimer > 0)
        {
            textCD.gameObject.SetActive(true);
            textCD.text = Mathf.RoundToInt(ability.cooldownTimer).ToString();
            iconCD.fillAmount = ability.cooldownTimer / ability.cooldown;
        }
        else {
            textCD.gameObject.SetActive(false);
            iconCD.fillAmount = 0.0f;
        }
    }


    
}
