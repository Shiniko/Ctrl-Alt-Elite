using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Choice_Menu : MonoBehaviour
{
    public SelectOnAwake awakeSelector;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private Button selectedButton;
    [SerializeField] private Button[] selectedButtons;

    public Navigation nav;

    void Awake()
    {
        ChangeSelectedButton();
    }

    public void SetSelectedButton(Button button)
    {
        if(button != null)
        {
            button.Select();

            selectedButton = button;
        }
    }

    public void ChangeSelectedButton()
    {
        if (pauseMenu != null)
        {
            Button[] allButtons = pauseMenu.GetComponentsInChildren<Button>();
            selectedButtons = allButtons;
        }

        for (int i = 0; i < selectedButtons.Length; i++)
        {
            Button button = selectedButtons[i];

            if (button.gameObject.activeInHierarchy)
            {
                SetSelectedButton(button);

                break;
            }
        }
    }
}
