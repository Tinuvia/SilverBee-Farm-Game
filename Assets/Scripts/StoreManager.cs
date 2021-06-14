using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreManager : MonoBehaviour
{
    public GameObject plantItem;
    List<PlantObject> plantObjects = new List<PlantObject>();

    private void Awake()
    {
        // filepath to our plants: Assets/Resources/Plants
        var loadPlants = Resources.LoadAll("Plants", typeof(PlantObject)); 
        foreach (var plant in loadPlants)
        {
            plantObjects.Add((PlantObject)plant);
        }
        plantObjects.Sort(SortByPrice);

        foreach (var plant in plantObjects)
        {
            PlantItem newPlant = Instantiate(plantItem, transform).GetComponent<PlantItem>();
            newPlant.plant = plant;
        }
    }

    int SortByPrice(PlantObject plantObject01, PlantObject plantObject02)
    {
        return plantObject01.buyPrice.CompareTo(plantObject02.buyPrice);
    }

    int SortByTime(PlantObject plantObject01, PlantObject plantObject02)
    {
        return plantObject01.timeBtwStages.CompareTo(plantObject02.timeBtwStages);
    }
}
