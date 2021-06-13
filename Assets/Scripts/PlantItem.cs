﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlantItem : MonoBehaviour
{
    public PlantObject plant;
    public Text nameTxt;
    public Text priceTxt;
    public Image icon;

    FarmManager fm;

    // Start is called before the first frame update
    void Start()
    {
        fm = FindObjectOfType<FarmManager>();
        InitializeUI();
    }

    public void BuyPlant()
    {
        fm.SelectPlant(this);
    }

    void InitializeUI()
    {
        nameTxt.text = plant.plantName;
        priceTxt.text = "$" + plant.price;
        icon.sprite = plant.icon;
    }


    // REFACTORING --- Set up FarmManager as singleton
}