using UnityEngine;
using UnityEngine.UI;
public abstract class FoodManager : MonoBehaviour
{
    public Image portrait;

    // Name/level
    public GameObject nameLevelBar;
    private Text nameText;
    private Text levelText;

    // Food
    public GameObject foodBar;
    private Transform foodMidRect;
    private Transform foodFrontRect;
    private Text foodText;

    public FloatValue currentFood;
    public float maxFood;

    protected ActivistsManager activistsManager;

    protected virtual void Awake()
    {
        InitFood();

        nameText = nameLevelBar.transform.Find("NameBar").Find("Text").GetComponent<Text>();
        levelText = nameLevelBar.transform.Find("LevelBar").Find("Text").GetComponent<Text>();

        Transform foodTransform = foodBar.transform.Find("Bar");
        foodMidRect = foodTransform.Find("Middle Rect");
        foodFrontRect = foodTransform.Find("Front Rect");
        foodText = foodBar.transform.Find("Food Amount Text").GetComponent<Text>();

        activistsManager = FindObjectOfType<ActivistsManager>();
    }
    
    public void InitFood()
    {
        foodBar.SetActive(true);
    }


    public void UpdateFoodBar()
    {

        float food = Mathf.Max(currentFood.value, 0);
        float percentage = food / maxFood;
        float newPosition = - foodMidRect.localScale.x / 2;
        float newWidth = foodMidRect.localScale.x * percentage;
        foodFrontRect.localScale = new Vector3(newWidth, foodMidRect.localScale.y);
        foodFrontRect.localPosition = new Vector3(newPosition, foodMidRect.localPosition.y);

        // foodFrontRect.GetComponent<Image>().color = ChooseBarColor(percentage);

        foodText.text = food + "/" + maxFood;
    }


    // public void UpdatePortrait()
    // {
    //     // Can remove when all activists have a rhino
    //     FindObjectOfType<RhinosManager>().ToggleRhinoInfo();

    //     Character character = GetCurrentCharacter();
    //     if (character != null)
    //     {
    //         portrait.sprite = character.portrait;
    //         nameText.text = character.entityName;
    //     }
    // }

    protected abstract Character GetCurrentCharacter();
}
