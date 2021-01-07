using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectParty : MonoBehaviour
{
    private ActivistsManager manager;
    private PlayerMovement[] players;

    CharacterSlot[] slots;
    public GameObject listOfActivists;

    public GameObject selectParty;
    public GameObject scrollBar;
    public GameObject characterSlotPrefab;

    public int partySize = 0;

    // Start is called before the first frame update
    void Start()
    {
        manager = FindObjectOfType<ActivistsManager>();
        players = manager.GetComponentsInChildren<PlayerMovement>();
        slots = listOfActivists.GetComponentsInChildren<CharacterSlot>();
        InitChoices();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (partySize > 0)
            {
                selectParty.SetActive(!selectParty.activeSelf);
                scrollBar.SetActive(!scrollBar.activeSelf);
                if (!selectParty.activeSelf)
                    UpdateAll();
            }
        }
    }

    void UpdateParty()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].GetComponent<Toggle>().isOn)
            {
                if (players[i].IsDead())
                {
                    players[i].currentState = PlayerState.dead;
                }
                else
                {
                    players[i].currentState = PlayerState.walk;
                }
            }
            else
            {
                players[i].currentState = PlayerState.disabled;
            }
        }
    }

    private void InitChoices()
    {
        for (int i = 0; i < players.Length; i++)
        {
            slots[i].GetComponent<Image>().sprite = players[i].GetSelectPartySprite();
            
            if (players[i].currentState != PlayerState.disabled && partySize < 3)
            {
                slots[i].GetComponent<Toggle>().isOn = true;
            }
            else
            {
                slots[i].GetComponent<Toggle>().isOn = false;
                players[i].currentState = PlayerState.disabled;
            }
        }

        manager.UpdateOffset();
    }

    public void UpdateAll()
    {
        UpdateParty();
        manager.UpdateCamera();
        manager.UpdatePartyPosition();
    }
}
