using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class ActivistsManager : MonoBehaviour
{
    public PlayerMovement[] playerMovs;
    [ReadOnly] public int currentPlayer = 0;
    public Signal changePlayerSignal;
    public SelectPartyManager partyManager;

    private CameraMovement cam;
    public PostProcessVolume postVolume;
    private PostProcessingScript dangerAnimation;
    [SerializeField] private GameObject gameOverUI;
    public int activistsDead = 0;

    // Awake is called before every Start method
    private void Awake()
    {
        playerMovs = GetComponentsInChildren<PlayerMovement>();
        cam = Camera.main.GetComponent<CameraMovement>();
        dangerAnimation = postVolume.GetComponent<PostProcessingScript>();
        dangerAnimation.players = transform;

        partyManager = FindObjectOfType<SelectPartyManager>();
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
            if (activistsDead == partyManager.partySize) {
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

        UpdateCharactersInfo();
    }

    private void UpdateCharactersInfo()
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

    public void UpdateCamera()
    {
        if (playerMovs[currentPlayer].currentState == PlayerState.disabled)
        {
            ChangePlayer();
        }
    }

    public void UpdatePartyPosition()
    {
        Vector3 currentPlayerPos = playerMovs[currentPlayer].transform.position;
        foreach(PlayerMovement player in playerMovs)
        {
            if (player != playerMovs[currentPlayer] && player.currentState != PlayerState.disabled)
            {
                player.transform.position = currentPlayerPos;
                player.TeleportRhino();
            }
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
