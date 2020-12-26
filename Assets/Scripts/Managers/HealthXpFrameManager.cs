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
    public Sprite[] activistPortraits;
    public IntValue currentPlayer;
    

    // Start is called before the first frame update
    void Start()
    {
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

    public void UpdateBar()
    {
        Player player = FindObjectOfType<ActivistsManager>().players[currentPlayer.value].GetComponent<Player>();
        Transform midRect = healthBar.transform.Find("Middle Rect");
        Transform frontRect = healthBar.transform.Find("Front Rect");

        float percentage = Mathf.Max(playerCurrentHealth.value / player.maxHealth.value, 0);
        float newPosition = - midRect.localScale.x / 2;
        float newWidth = midRect.localScale.x * percentage;
        frontRect.localScale = new Vector3(newWidth, midRect.localScale.y);
        frontRect.localPosition = new Vector3(newPosition, midRect.localPosition.y);

        frontRect.GetComponent<Image>().color = player.ChooseBarColor(percentage);
    }

    public void UpdateXpBar()
    {
        Player player = FindObjectOfType<ActivistsManager>().players[currentPlayer.value].GetComponent<Player>();
        Transform midRect = xpBar.transform.Find("Middle Rect");
        Transform frontRect = xpBar.transform.Find("Front Rect");

        float percentage = Mathf.Max((float)playerCurrentXp.value / player.requiredXp, 0);
        float newPosition = -midRect.localScale.x / 2;
        float newWidth = midRect.localScale.x * percentage;
        frontRect.localScale = new Vector3(newWidth, midRect.localScale.y);
        frontRect.localPosition = new Vector3(newPosition, midRect.localPosition.y);

        frontRect.GetComponent<Image>().color = new Color(1, 1, 0);
    }

    public void UpdatePortrait()
    {
        portrait.sprite = activistPortraits[currentPlayer.value];
    }
}
