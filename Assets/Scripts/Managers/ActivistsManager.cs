using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class ActivistsManager : MonoBehaviour
{
    // Start is called before the first frame update
    public PlayerMovement[] players;
    public IntValue currentPlayer;
    public Signal changePlayerSignal;

    private CameraMovement cam;
    public PostProcessVolume postVolume;
    private PostProcessingScript dangerAnimation;

    void Start()
    {
        players = GetComponentsInChildren<PlayerMovement>();
        players[currentPlayer.value].GetComponent<Player>().UpdateBarHealth();
        
        cam = Camera.main.GetComponent<CameraMovement>();
        dangerAnimation = postVolume.GetComponent<PostProcessingScript>();
        dangerAnimation.players = transform;
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
            ChangePlayer();
    }

    private void ChangePlayer()
    {
        PlayerMovement playerMov = players[currentPlayer.value];
        playerMov.inputEnabled = false;
        playerMov.animator.SetBool("moving", false);
        playerMov.animator.SetBool("attacking", false);

        currentPlayer.value = (currentPlayer.value + 1) % players.Length;
        playerMov = players[currentPlayer.value];

        playerMov.inputEnabled = true;
        cam.target = playerMov.transform;
        dangerAnimation.currentPlayer = currentPlayer.value;

        // Send signals
        playerMov.GetComponent<Player>().UpdateBarHealth();
        changePlayerSignal.Raise();
    }

    public void HealAll()
    {
        for (int i = 0; i < players.Length; i ++)
        {
            Player player = players[i].GetComponent<Player>();
            player.FullRestore();
            players[i].Revive();
            if (i == currentPlayer.value)
            {
                player.UpdateBarHealth();
                players[i].inputEnabled = true;
            }

        }    
    }

    public bool IsCurrentActivist(Player activist)
    {
        return players[currentPlayer.value].GetComponent<Player>() == activist;
    }
}
