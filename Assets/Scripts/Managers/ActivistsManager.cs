using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;

public class ActivistsManager : MonoBehaviour
{
    public delegate void OnPlayerChanged();
    public OnPlayerChanged onPlayerChangedCallback;
    public Player[] players;
    public PlayerMovement[] playerMovs;
    [ReadOnly] public int currentPlayer = 0;
    public Signal changePlayerSignal;
    public SelectParty partyManager;

    private CameraMovement cam;
    public PostProcessVolume postVolume;
    private PostProcessingScript dangerAnimation;
    [SerializeField] private GameObject gameOverUI;

    public int activistsDead = 0;

    public float[] walkOffset;
    public Vector3[] warpOffset;

    // Awake is called before every Start method
    private void Awake()
    {
        playerMovs = GetComponentsInChildren<PlayerMovement>();
        players = GetComponentsInChildren<Player>();
        cam = Camera.main.GetComponent<CameraMovement>();
        dangerAnimation = postVolume.GetComponent<PostProcessingScript>();
        dangerAnimation.players = transform;

        partyManager = FindObjectOfType<SelectParty>();
    }
    private void Start()
    {
        UpdateCharactersInfo();
    }
    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
            ChangePlayer();
    }

    public void ChangePlayer()
    {
        PlayerMovement playerMov = playerMovs[currentPlayer];
        playerMov.GetComponent<SpriteRenderer>().color = Color.white;
        playerMov.inputEnabled = false;
        playerMov.animator.SetBool("moving", false);
        playerMov.animator.SetBool("attacking", false);

        currentPlayer = (currentPlayer + 1) % playerMovs.Length;
        playerMov = playerMovs[currentPlayer];
        while (playerMov.currentState == PlayerState.disabled || 
            playerMov.GetComponent<Player>().health <= 0) {
            if (activistsDead >= partyManager.partySize) {
                Time.timeScale = 0;
                gameOverUI.SetActive(true);
                return;
            }
            currentPlayer = (currentPlayer + 1) % playerMovs.Length;
            playerMov = playerMovs[currentPlayer];
        }

        if (onPlayerChangedCallback != null)
        {
            onPlayerChangedCallback.Invoke();
        }

        playerMov.inputEnabled = true;
        cam.target = playerMov.transform;
        dangerAnimation.currentPlayer = currentPlayer;

        UpdateInteractables();

        UpdateCharactersInfo();
        UpdateOffset();
    }

    public void UpdateCharactersInfo()
    {
        changePlayerSignal.Raise();

        Player player = GetCurrentPlayer();

        player.UpdateBarHealth();
        player.ReceiveXp(0); // To update XP bar
        if (player.rhino != null)
        {
            player.rhino.UpdateBarHealth();
            player.rhino.ReceiveXp(0);
        }
    }

    public void HealAll()
    {
        int j = 0;
        for (int i = 0; i < playerMovs.Length; i ++)
        {
            Player player = playerMovs[i].GetComponent<Player>();
            player.FullRestore();
            if (i != currentPlayer && playerMovs[i].currentState != PlayerState.disabled)
                playerMovs[i].Revive(playerMovs[currentPlayer].transform.position + warpOffset[j++]);
            
            else if (i == currentPlayer)
            {
                player.UpdateBarHealth();
                playerMovs[i].inputEnabled = true;
            }
            activistsDead = 0;

        }    
    }

    public void UpdateCamera()
    {
        if (playerMovs[currentPlayer].currentState == PlayerState.disabled)
        {
            ChangePlayer();
        }
    }

    public void UpdateOffset()
    {
        int i = 0;
        foreach (PlayerMovement player in playerMovs)
        {
            if (player != playerMovs[currentPlayer] && player.currentState != PlayerState.disabled)
            {
                player.SetWalkOffset(walkOffset[i++]);
            }
        }
    }

    public void UpdatePartyPosition()
    {
        Vector3 currentPlayerPos = playerMovs[currentPlayer].transform.position;
        int i = 0;
        foreach(PlayerMovement player in playerMovs)
        {
            if (player != playerMovs[currentPlayer] && player.currentState != PlayerState.disabled)
            {
                
                player.SetWalkOffset(walkOffset[i]);
                player.TeleportPlayer(currentPlayerPos + warpOffset[i++]);
                player.TeleportRhino();
            }
        }
    }

    private void UpdateInteractables()
    {
        BoxCollider2D collider = GetCurrentPlayerMovement().GetComponent<BoxCollider2D>();

        foreach (Interactable interactable in FindObjectsOfType<Interactable>())
        {
            interactable.TestPlayerNear(collider);
            interactable.TestPlayerFar(collider);
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

    public Vector3 GetCurrentPlayerOrientation()
    {
        Vector3 orientation = new Vector3(playerMovs[currentPlayer].animator.GetFloat("moveX"), playerMovs[currentPlayer].animator.GetFloat("moveY"));
        return orientation;
    }

    public Player GetCurrentPlayer()
    {
        return playerMovs[currentPlayer].GetComponent<Player>();
    }
}
