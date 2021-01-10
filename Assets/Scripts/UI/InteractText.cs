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
    private List<Interactable> interactables;

    private Interactable currentInteractable;

    private void Awake()
    {
        keyGameObject = transform.Find("E").gameObject;
        textGameObject = transform.Find("Text").gameObject;
        text = textGameObject.GetComponentInChildren<Text>();
        interactables = new List<Interactable>();

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

    public void AddInteractable(Interactable interactable)
    {
        currentInteractable = interactable;
        string newText = interactable.GetInteractText();
        if (newText != null)
        {
            text.text = newText;
            SetActive(true);
        }
        else
            SetActive(false);

        //interactables.Add(interactable);
    }

    public void RemoveInteractable(Interactable interactable)
    {
        //interactables.Add(interactable);
    }
}
