using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlotManager : MonoBehaviour
{  
    bool isPlanted = false;
    SpriteRenderer plant;
    BoxCollider2D plantCollider;
    int plantStage = 0;
    float timer;
    FarmManager fm;
    PlantObject selectedPlant;

    // Start is called before the first frame update
    void Start()
    {
        plant = transform.GetChild(0).GetComponent<SpriteRenderer>();
        plantCollider = transform.GetChild(0).GetComponent<BoxCollider2D>();
        fm = transform.parent.GetComponent<FarmManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlanted)
        {
            timer -= Time.deltaTime;

            if (timer < 0 && (plantStage < (selectedPlant.plantStages.Length - 1)))
            {
                timer = selectedPlant.timeBtwStages;
                plantStage++;
                UpdatePlant();
            }
        }        
    }

    private void OnMouseDown()
    {
        if (isPlanted)
        {
            if(plantStage == (selectedPlant.plantStages.Length - 1))
                Harvest();
        }         
        else if (fm.isPlanting)
        {
            Plant(fm.selectPlant.plant);
        }
            
    }

    void Harvest()
    {
        isPlanted = false;
        Debug.Log("Harvested");            
        plant.gameObject.SetActive(false);

    }

    void Plant(PlantObject newPlant)
    {
        selectedPlant = newPlant;
        isPlanted = true;
        Debug.Log("Planted");
        plantStage = 0;
        UpdatePlant();
        plant.gameObject.SetActive(true);
        timer = selectedPlant.timeBtwStages;

    }

    void UpdatePlant()
    {
        plant.sprite = selectedPlant.plantStages[plantStage];
        plantCollider.size = plant.sprite.bounds.size;
        plantCollider.offset = new Vector2(0, plant.bounds.size.y/2);
    }

    //Refactoring: in Start, assign plantStages[] = selectedPlant.plantStages etc
}
