using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    public PlayerMovement[] players;
    private int currentPlayer = 0;
    private CameraMovement cam;
    void Start()
    {
        this.players = GetComponentsInChildren<PlayerMovement>();
        this.cam = Camera.main.GetComponent<CameraMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            this.players[currentPlayer].inputEnabled = false;
            this.players[currentPlayer].animator.SetBool("moving", false);
            this.players[currentPlayer].animator.SetBool("attacking", false);
            currentPlayer++;
            if (currentPlayer >= this.players.Length) {
                currentPlayer = 0;
            }
            this.players[currentPlayer].inputEnabled = true;
            this.cam.target = this.players[currentPlayer].transform;
        }
    }
}
