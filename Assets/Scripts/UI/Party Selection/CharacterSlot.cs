using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSlot : MonoBehaviour
{
    public SelectParty manager;
    public Toggle toggleButton;

    private bool limitReached = false;
    private bool zeroSizeParty = false;

    private void Awake()
    {
        manager = FindObjectOfType<SelectParty>();
    }

    public void VerifySelection(bool change)
    {
        if (change && manager.partySize >= 3)
        {
            limitReached = true;
            toggleButton.isOn = false;
            limitReached = false;
        }
        else if (!change && manager.partySize == 1)
        {
            zeroSizeParty = true;
            toggleButton.isOn = true;
            zeroSizeParty = false;
        }
        else if (!change && manager.partySize > 0 && !limitReached)
        {
            manager.partySize--;
        }
        else if (change && manager.partySize < 3 && !zeroSizeParty)
        {
            manager.partySize++;
        }
    }
}
