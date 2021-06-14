using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FarmManager : MonoBehaviour
{
    public PlantItem selectPlant;
    public bool isPlanting = false;
    public int money = 100;
    public Text moneyTxt;

    public Color buyColor = Color.green;
    public Color cancelColor = Color.red;

    public bool isSelecting = false;
    public int selectedTool = 0; // #1 water, #2 fertilizer, #3 buy plot

    public Image[] buttonsImg;
    public Sprite normalButton;
    public Sprite selectedButton;


    // Start is called before the first frame update
    void Start()
    {
        moneyTxt.text = "$" + money;
    }

    public void SelectPlant(PlantItem newPlant)
    {
        // deselect if clicked while already selected
        if(selectPlant == newPlant)
        {
            CheckSelection();
        }
        else
        {
            CheckSelection();
            selectPlant = newPlant;
            selectPlant.btnImage.color = cancelColor;
            selectPlant.btnTxt.text = "Cancel";
            isPlanting = true;
        }
    }

    // needs to be public since we call it from the button
    public void SelectTool(int toolNumber)
    {
        if(toolNumber == selectedTool)
        {
            CheckSelection();
        } else
        {
            // select tool number and check to see if anything else was also selected
            CheckSelection();
            isSelecting = true;
            selectedTool = toolNumber;
            buttonsImg[toolNumber - 1].sprite = selectedButton;
        }
    }

    void CheckSelection()
    {
        if (isPlanting)
        {
            isPlanting = false;
            if (selectPlant != null)
            {
                //deselecting
                selectPlant.btnImage.color = buyColor;
                selectPlant.btnTxt.text = "Buy";
                selectPlant = null; 
            }
        }
        if (isSelecting)
        {
            if (selectedTool > 0)
            {
                buttonsImg[selectedTool - 1].sprite = normalButton;
            }
            isSelecting = false;
            selectedTool = 0;
        }
    }

    public void Transaction(int value)
    {
        money += value;
        moneyTxt.text = "$" + money; 
    }

    // REFACTORING 
    // function UpdateMoney: moneyTxt.text = "$" + money;
    // Set up FarmManager as singleton
    //  function UpdateSelectionButton(bool isBuying): 
    // --- selectPlant.btnImage.color = buyColor; cancelColor
    // --- selectPlant.btnTxt.text = "Buy";
    // CheckSelection called outside of if statements since it's in both cases
}
