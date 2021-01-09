using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SelectParty : MonoBehaviour
{
    private ActivistsManager manager;
    private PlayerMovement[] players;

    private int[] lastParty;
    private int[] newParty;

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
        lastParty = new int[players.Length];
        newParty = new int[players.Length];
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
                players[i].transform.gameObject.SetActive(true);
                players[i].EnableRhino();
                newParty[i] = 1;

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
                newParty[i] = 0;
                players[i].currentState = PlayerState.disabled;
                players[i].DisableRhino();
                players[i].transform.gameObject.SetActive(false);
            }
        }
    }

    bool PartyChanged()
    {
        return !newParty.SequenceEqual(lastParty);
    }

    private void InitChoices()
    {
        for (int i = 0; i < players.Length; i++)
        {
            slots[i].GetComponent<Image>().sprite = players[i].GetSelectPartySprite();
            
            if (players[i].currentState != PlayerState.disabled && partySize < 3)
            {
                slots[i].GetComponent<Toggle>().isOn = true;
                lastParty[i] = 1;
            }
            else
            {
                slots[i].GetComponent<Toggle>().isOn = false;
                players[i].currentState = PlayerState.disabled;
                lastParty[i] = 0;
                players[i].DisableRhino();
                players[i].transform.gameObject.SetActive(false);
            }
        }

        manager.UpdateOffset();
    }

    public void UpdateAll()
    {
        UpdateParty();
        if (PartyChanged())
        {
            lastParty = newParty.ToArray();
            manager.UpdateCamera();
            manager.UpdatePartyPosition();
        }
    }
}
