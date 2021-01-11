using UnityEngine;
using UnityEngine.UI;
public class FoodManager : MonoBehaviour
{
    // Food
    public GameObject foodBar;
    private Transform foodMidRect;
    private Transform foodFrontRect;
    private Text foodText;

    public FloatValue currentFood;

    protected ActivistsManager activistsManager;

    private float time;

    protected virtual void Awake()
    {
        InitFood();

        Transform foodTransform = foodBar.transform.Find("Bar");
        foodMidRect = foodTransform.Find("Middle Rect");
        foodFrontRect = foodTransform.Find("Front Rect");
        foodText = foodBar.transform.Find("Food Amount Text").GetComponent<Text>();

        activistsManager = FindObjectOfType<ActivistsManager>();
    }
    
    public void InitFood()
    {
        foodBar.SetActive(true);
        currentFood.value = Food.MAX;
    }

    public void Start()
    {
        UpdateFoodBar();
    }

    public void UpdateFoodBar()
    {
        float food = Mathf.Max(currentFood.value, 0);
        float percentage = food / Food.MAX;
        float newWidth = foodMidRect.localScale.x * percentage;
        foodFrontRect.localScale = new Vector3(newWidth, foodMidRect.localScale.y);

        foodText.text = food + "/" + Food.MAX;
    }

    private void FixedUpdate() {
        time += Time.deltaTime;
        if (time > 30) {
            time = 0;
            currentFood.value --;
            UpdateFoodBar();
            if (currentFood.value == 0)
                DieFromHunger();
        }
    }

    private void DieFromHunger()
    {
        foreach (Player player in activistsManager.players)
        {
            if (player.HasRhino())
                player.rhino.TakeDamage(player.rhino.maxHealth);
            player.TakeRadiationDamage(player.maxHealth);
            player.UpdateBarHealth();
            player.rhino.UpdateBarHealth();
        }
    }
}
