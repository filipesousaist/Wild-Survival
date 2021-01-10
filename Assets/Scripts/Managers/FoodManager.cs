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
    public float maxFood;

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
        currentFood.value=5;
    }


    public void UpdateFoodBar()
    {

        float food = Mathf.Max(currentFood.value, 0);
        float percentage = food / maxFood;
        float newPosition = - foodMidRect.localScale.x / 2;
        float newWidth = foodMidRect.localScale.x * percentage;
        foodFrontRect.localScale = new Vector3(newWidth, foodMidRect.localScale.y);
        foodFrontRect.localPosition = new Vector3(newPosition, foodMidRect.localPosition.y);

        foodText.text = food + "/" + maxFood;
    }

    private void FixedUpdate(){
        time+=Time.deltaTime;
        if(time>10){
            time=0;
            currentFood.value--;
            UpdateFoodBar();
            if(currentFood.value==0){
                foreach(var player in activistsManager.players)
                    player.TakeDamage(100);
            }
        }
    }

}
