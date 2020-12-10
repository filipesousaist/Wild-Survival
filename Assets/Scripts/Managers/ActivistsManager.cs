using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class ActivistsManager : MonoBehaviour
{
    // Start is called before the first frame update
    public PlayerMovement[] players;
    private int currentPlayer = 0;
    private CameraMovement cam;
    public PostProcessVolume postVolume;
    private PostProcessingScript dangerAnimation;

    void Start()
    {
        this.players = GetComponentsInChildren<PlayerMovement>();
        
        this.cam = Camera.main.GetComponent<CameraMovement>();
        this.dangerAnimation = this.postVolume.GetComponent<PostProcessingScript>();
        this.dangerAnimation.players = this.transform;
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
            this.ChangePlayer();
    }

    private void ChangePlayer()
    {
        this.players[currentPlayer].inputEnabled = false;
        this.players[currentPlayer].animator.SetBool("moving", false);
        this.players[currentPlayer].animator.SetBool("attacking", false);

        this.currentPlayer = (this.currentPlayer + 1) % this.players.Length;

        this.players[currentPlayer].inputEnabled = true;
        this.cam.target = this.players[currentPlayer].transform;
        this.dangerAnimation.currentPlayer = this.currentPlayer;
    }
}
