using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HealthXpFrameManager : MonoBehaviour
{
    public GameObject healthBar;
    public GameObject xpBar;

    public FloatValue playerCurrentHealth;
    public IntValue playerCurrentXp;

    public Image portrait;

    private ActivistsManager activistsManager;
    

    // Start is called before the first frame update
    void Start()
    {
        activistsManager = FindObjectOfType<ActivistsManager>();
        InitHealth();
        InitXp();
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
        Player player = activistsManager.GetCurrentPlayer();

        Transform midRect = healthBar.transform.Find("Middle Rect");
        Transform frontRect = healthBar.transform.Find("Front Rect");

        float health = Mathf.Max(playerCurrentHealth.value, 0);
        float percentage = health / player.maxHealth.value;
        float newPosition = - midRect.localScale.x / 2;
        float newWidth = midRect.localScale.x * percentage;
        frontRect.localScale = new Vector3(newWidth, midRect.localScale.y);
        frontRect.localPosition = new Vector3(newPosition, midRect.localPosition.y);

        frontRect.GetComponent<Image>().color = player.ChooseBarColor(percentage);

        GameObject.Find("HP Amount Text").GetComponent<Text>().text = 
            health + "/" + player.maxHealth.value;
    }

    public void UpdateXpBar()
    {
        Player player = activistsManager.GetCurrentPlayer();
        Transform midRect = xpBar.transform.Find("Middle Rect");
        Transform frontRect = xpBar.transform.Find("Front Rect");

        float percentage = playerCurrentXp.value / player.requiredXp;
        float newPosition = -midRect.localScale.x / 2;
        float newWidth = midRect.localScale.x * percentage;
        frontRect.localScale = new Vector3(newWidth, midRect.localScale.y);
        frontRect.localPosition = new Vector3(newPosition, midRect.localPosition.y);

        GameObject.Find("XP Amount Text").GetComponent<Text>().text =
            playerCurrentXp.value + "/" + player.requiredXp;
    }

    public void UpdatePortrait()
    {
        portrait.sprite = activistsManager.GetCurrentPlayer().portrait;
    }
}
