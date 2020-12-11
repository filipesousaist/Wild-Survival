using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HealthFrameManager : MonoBehaviour
{
    public Image healthBar;
    public Sprite fullHealth;
    public Sprite threeQuartersHealth;
    public Sprite twoQuartersHealth;
    public Sprite oneQuartersHealth;
    public FloatValue playerCurrentHealth;

    public Image portrait;
    public Sprite[] activistPortraits;
    public IntValue currentPlayer;
    

    // Start is called before the first frame update
    void Start()
    {
        InitHealth();
    }
    
    public void InitHealth()
    {
        healthBar.gameObject.SetActive(true);
        healthBar.sprite = fullHealth;
    }

    public void UpdateBar()
    {
        float tempHealth = playerCurrentHealth.value;
        
        if (tempHealth == 4)
        {
            healthBar.sprite = fullHealth;
        }
        else if (tempHealth == 3)
        {
            healthBar.sprite = threeQuartersHealth;
        }
        else if (tempHealth == 2)
        {
            healthBar.sprite = twoQuartersHealth;
        }
        else if (tempHealth == 1)
        {
            healthBar.sprite = oneQuartersHealth;
        }
        
        healthBar.gameObject.SetActive(tempHealth > 0);
    }

    public void UpdatePortrait()
    {
        portrait.sprite = activistPortraits[currentPlayer.value];
    }
}
