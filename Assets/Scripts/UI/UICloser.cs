using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICloser : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            CloseUI();
    }
    protected virtual void CloseUI()
    {
        gameObject.SetActive(false);
    }
}
