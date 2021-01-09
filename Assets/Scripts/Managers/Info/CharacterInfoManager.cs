using UnityEngine;
using UnityEngine.UI;
public abstract class CharacterInfoManager : MonoBehaviour
{
    public Image portrait;

    // Name/level
    public GameObject nameLevelBar;
    private Text nameText;
    private Text levelText;

    // Health
    public GameObject healthBar;
    private Transform healthMidRect;
    private Transform healthFrontRect;
    private Text healthText;

    public FloatValue currentHealth;

    // XP
    public GameObject xpBar;
    private Transform xpMidRect;
    private Transform xpFrontRect;
    private Text xpText;

    public IntValue currentXp;

    protected ActivistsManager activistsManager;

    protected virtual void Awake()
    {
        InitHealth();
        InitXp();

        nameText = nameLevelBar.transform.Find("NameBar").Find("Text").GetComponent<Text>();
        levelText = nameLevelBar.transform.Find("LevelBar").Find("Text").GetComponent<Text>();

        Transform healthTransform = healthBar.transform.Find("Bar");
        healthMidRect = healthTransform.Find("Middle Rect");
        healthFrontRect = healthTransform.Find("Front Rect");
        healthText = healthBar.transform.Find("HP Amount Text").GetComponent<Text>();

        Transform xpTransform = xpBar.transform.Find("Bar");
        xpMidRect = xpTransform.Find("Middle Rect");
        xpFrontRect = xpTransform.Find("Front Rect");
        xpText = xpBar.transform.Find("XP Amount Text").GetComponent<Text>();

        activistsManager = FindObjectOfType<ActivistsManager>();
    }
    
    public void InitHealth()
    {
        healthBar.SetActive(true);
    }

    public void InitXp()
    {
        xpBar.SetActive(true);
    }

    public void UpdateHealthBar()
    {
        Character character = GetCurrentCharacter();

        float health = Mathf.Max(currentHealth.value, 0);
        float percentage = health / character.maxHealth;
        float newPosition = - healthMidRect.localScale.x / 2;
        float newWidth = healthMidRect.localScale.x * percentage;
        healthFrontRect.localScale = new Vector3(newWidth, healthMidRect.localScale.y);
        healthFrontRect.localPosition = new Vector3(newPosition, healthMidRect.localPosition.y);

        healthFrontRect.GetComponent<Image>().color = character.ChooseBarColor(percentage);

        healthText.text = health + "/" + character.maxHealth;
    }

    public void UpdateXpBar()
    {
        Character character = GetCurrentCharacter();

        float percentage = ((float) currentXp.value) / character.requiredXp;
        float newPosition = -xpMidRect.localScale.x / 2;
        float newWidth = xpMidRect.localScale.x * percentage;
        xpFrontRect.localScale = new Vector3(newWidth, xpMidRect.localScale.y);
        xpFrontRect.localPosition = new Vector3(newPosition, xpMidRect.localPosition.y);

        xpText.text = currentXp.value + "/" + character.requiredXp;

        levelText.text = "Lvl " + character.level;  
    }

    public void UpdatePortrait()
    {
        // Can remove when all activists have a rhino
        FindObjectOfType<RhinosManager>().ToggleRhinoInfo();

        Character character = GetCurrentCharacter();
        if (character != null)
        {
            portrait.sprite = character.portrait;
            nameText.text = character.entityName;
        }
    }

    protected abstract Character GetCurrentCharacter();
}
