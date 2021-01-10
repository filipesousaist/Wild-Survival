using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Sign : Interactable
{
    private Building building;
    private GameObject textGameObject;
    private Text text;

    public GameObject textPrefab;

    protected override void OnAwake()
    {
        base.OnAwake();
        building = GetComponentInParent<Building>();
        InitText();
        text = textGameObject.GetComponentInChildren<Text>();
    }

    private void InitText()
    {
        textGameObject = Instantiate(textPrefab);
        textGameObject.transform.SetParent(FindObjectOfType<Canvas>().transform);
        UpdateTextPosition();
        textGameObject.SetActive(false);
    }

    // Start is called before the first frame update
    override protected void OnStart()
    {
        text.text = "Build " + building.entityName;
    }

    private void FixedUpdate()
    {
        UpdateText();
    }

    protected override void OnPlayerApproach()
    {
        textGameObject.SetActive(true);
        UpdateText();
    }

    protected override void OnPlayerMoveAway()
    {
        textGameObject.SetActive(false);
    }

    private void UpdateText()
    {
        if (textGameObject.activeSelf)
        {
            UpdateTextPosition();
            text.text = (building.level == 0 ? "Build " : "Repair ") + building.entityName;
        }
    }

    private void UpdateTextPosition()
    {
        textGameObject.transform.position = Camera.main.WorldToScreenPoint(transform.position + Vector3.up);
    }

    protected override IEnumerator OnInteract()
    {
        yield return base.OnInteract();
        textGameObject.SetActive(false);
    }

    protected override void AfterInteract()
    {
        if (building.level == 0)
            building.Upgrade();
        else
            building.Repair();
    }

    public override string GetInteractText()
    {
        return building.level == 0 ? "Build" : "Repair";
    }
}
