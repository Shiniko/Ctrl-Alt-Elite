using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using System;

public class ProgressionManager : MonoBehaviour
{
    public int totalMessAmount = 10;
    [SerializeField]
    private GameObject messBar;
    [SerializeField]
    private GameObject starForSpecialItem;
    [SerializeField]
    private GameObject starForMessCompletion;
    [SerializeField]
    private GameObject starForDog;
    [SerializeField]
    private GameObject starForSpecialItemCollapsed;
    [SerializeField]
    private GameObject starForMessCompletionCollapsed;
    [SerializeField]
    private GameObject starForDogCollapsed;
    [SerializeField]
    private float percentDarken = 0.5f;
    private bool isStarSpecialItem;
    private bool isStarMessComplete;
    private bool isStarDog;
    private float messIncrement;
    private int starScore; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ResetProgress();
        messBar.GetComponent<Image>().fillAmount = 0f;
        messIncrement = 1f / totalMessAmount;
    }


    // Update is called once per frame
    void Update()
    {
        // For debugging
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            AddMess();
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            RemoveMess();
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            AddStarForSpecialItem();
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            RemoveStarForSpecialItem();
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            AddStarForMessComplete();
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            RemoveStarForMessComplete();
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            AddStarForDog();
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            RemoveStarForDog();
        }
    }

    public int GetStarScore()
    {
        return starScore;
    }

    public void AddMess()
    {
        messBar.GetComponent<Image>().fillAmount += messIncrement;

    }
    public void RemoveMess()
    {
        messBar.GetComponent<Image>().fillAmount -= messIncrement;

    }
    private Color darkenImage()
    {
        Color fadeColor = new(1f * percentDarken, 1f * percentDarken, 1f * percentDarken, 1f * percentDarken);
        return fadeColor;
    }

    public void RemoveStarForMessComplete()
    {
        if (isStarMessComplete)
        {
            starForMessCompletion.GetComponent<Image>().color = darkenImage();
            starForMessCompletionCollapsed.GetComponent<Image>().color = darkenImage();
            isStarMessComplete = false;
            starScore--;
        }
    }
    public void AddStarForMessComplete()
    {
        if (!isStarMessComplete)
        {
            starForMessCompletion.GetComponent<Image>().color = Color.white;
            starForMessCompletionCollapsed.GetComponent<Image>().color = Color.white;
            isStarMessComplete = true;
            starScore++;
        }
    }
    public void RemoveStarForSpecialItem()
    {
        if (isStarSpecialItem)
        {
            starForSpecialItem.GetComponent<Image>().color = darkenImage();
            starForSpecialItemCollapsed.GetComponent<Image>().color = darkenImage();
            isStarSpecialItem = false;
            starScore--;
        }
    }
    public void AddStarForSpecialItem()
    {
        if (!isStarSpecialItem)
        {
            starForSpecialItem.GetComponent<Image>().color = Color.white;
            starForSpecialItemCollapsed.GetComponent<Image>().color = Color.white;
            isStarSpecialItem = true;
            starScore++;
        }
    }
    public void RemoveStarForDog()
    {
        if (isStarDog)
        {
            starForDog.GetComponent<Image>().color = darkenImage();
            starForDogCollapsed.GetComponent<Image>().color = darkenImage();
            isStarDog = false;
            starScore--;
        }
    }
    public void AddStarForDog()
    {
        if (!isStarDog)
        {
            starForDog.GetComponent<Image>().color = Color.white;
            starForDogCollapsed.GetComponent<Image>().color = Color.white;
            isStarDog = true;
            starScore++;
        }
    }

    public void ResetProgress()
    {
        starForMessCompletion.GetComponent<Image>().color = darkenImage();
        starForMessCompletionCollapsed.GetComponent<Image>().color = darkenImage();
        starForSpecialItem.GetComponent<Image>().color = darkenImage();
        starForSpecialItemCollapsed.GetComponent<Image>().color = darkenImage();
        starForDog.GetComponent<Image>().color = darkenImage();
        starForDogCollapsed.GetComponent<Image>().color = darkenImage();
        isStarMessComplete = false;
        isStarSpecialItem = false;
        isStarDog = false;
        starScore = 0;
        messBar.GetComponent<Image>().fillAmount = 0f;
    }
}
