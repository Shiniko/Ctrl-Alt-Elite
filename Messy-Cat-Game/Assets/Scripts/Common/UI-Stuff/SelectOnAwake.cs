using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SelectOnAwake : MonoBehaviour
{
    public Button primaryButton;

    public Navigation nav;

    void Awake()
    {
        SetSelected();
    }

    public void SetSelected()
    {
        if (primaryButton != null)
        {
            primaryButton.Select();
        }
    }
}
