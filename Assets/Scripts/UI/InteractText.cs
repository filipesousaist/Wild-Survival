using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractText : MonoBehaviour
{
    private ActivistsManager activistsManager;
    private GameObject keyGameObject;
    private GameObject textGameObject;
    private Text text;

    private Interactable currentInteractable;

    private void Awake()
    {
        keyGameObject = transform.Find("E").gameObject;
        textGameObject = transform.Find("Text").gameObject;
        text = textGameObject.GetComponentInChildren<Text>();

        currentInteractable = null;
    }

    void Start()
    {
        SetActive(false);
    }

    private void SetActive(bool active)
    {
        keyGameObject.SetActive(active);
        textGameObject.SetActive(active);
    }

    void FixedUpdate()
    {
        if (currentInteractable != null && currentInteractable.isInteractable)
            UpdateText();
        else
            SetActive(false);
    }

    private void UpdateText()
    {
        string newText = currentInteractable.GetInteractText();
        bool hasText = newText != null;
        if (hasText)
            text.text = newText;
        SetActive(hasText);
    }

    public void SetInteractable(Interactable interactable)
    {
        currentInteractable = interactable;
        UpdateText();
    }
}
