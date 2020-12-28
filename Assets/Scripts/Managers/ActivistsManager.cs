using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class ActivistsManager : MonoBehaviour
{
    public PlayerMovement[] playerMovs;
    [ReadOnly]
    public int currentPlayer = 0;
    public Signal changePlayerSignal;

    private CameraMovement cam;
    public PostProcessVolume postVolume;
    private PostProcessingScript dangerAnimation;
    [SerializeField]
    private GameObject gameOverUI;
    public int activistsDead = 0;

    void Start()
    {
        playerMovs = GetComponentsInChildren<PlayerMovement>();
        playerMovs[currentPlayer].GetComponent<Player>().UpdateBarHealth();
        
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

    public void ChangePlayer()
    {
        PlayerMovement playerMov = playerMovs[currentPlayer];
        playerMov.inputEnabled = false;
        playerMov.animator.SetBool("moving", false);
        playerMov.animator.SetBool("attacking", false);

        currentPlayer = (currentPlayer + 1) % playerMovs.Length;
        playerMov = playerMovs[currentPlayer];
        while (playerMov.GetComponent<Player>().health <= 0) {
            if (activistsDead == playerMovs.Length) {
                Time.timeScale = 0;
                gameOverUI.SetActive(true);
                gameObject.SetActive(false);
                return;
            }
            currentPlayer = (currentPlayer + 1) % playerMovs.Length;
            playerMov = playerMovs[currentPlayer];
        }

        playerMov.inputEnabled = true;
        cam.target = playerMov.transform;
        dangerAnimation.currentPlayer = currentPlayer;

        // Send signals
        playerMov.GetComponent<Player>().UpdateBarHealth();
        changePlayerSignal.Raise();
        Debug.Log("Current player: " + playerMov.GetComponent<Player>().entityName);
    }

    public void HealAll()
    {
        for (int i = 0; i < playerMovs.Length; i ++)
        {
            Player player = playerMovs[i].GetComponent<Player>();
            player.FullRestore();
            playerMovs[i].Revive();
            if (i == currentPlayer)
            {
                player.UpdateBarHealth();
                playerMovs[i].inputEnabled = true;
            }
            activistsDead = 0;

        }    
    }

    public bool IsCurrentActivist(Player activist)
    {
        return playerMovs[currentPlayer].GetComponent<Player>() == activist;
    }

    public PlayerMovement GetCurrentPlayerMovement()
    {
        return playerMovs[currentPlayer];
    }

    public Player GetCurrentPlayer()
    {
        return playerMovs[currentPlayer].GetComponent<Player>();
    }
}
