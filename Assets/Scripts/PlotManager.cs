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

    public Color availableColor = Color.green;
    public Color unAvailableColor = Color.red;
    SpriteRenderer plot;

    FarmManager fm;
    PlantObject selectedPlant;

    // Start is called before the first frame update
    void Start()
    {
        plant = transform.GetChild(0).GetComponent<SpriteRenderer>();
        plot = GetComponent<SpriteRenderer>();
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
            if(!fm.isPlanting && plantStage == (selectedPlant.plantStages.Length - 1))
                Harvest();
        }         
        else if (fm.isPlanting && fm.selectPlant.plant.buyPrice <= fm.money)
        {
            Plant(fm.selectPlant.plant); // can't use selectedPlant here since it's not planted yet
        }
            
    }

    private void OnMouseOver()
    {
        if (fm.isPlanting)
        {
            if (isPlanted || fm.selectPlant.plant.buyPrice > fm.money)
            {
                plot.color = unAvailableColor;
            }
            else
            {
                plot.color = availableColor;
            }
        }
    }

    private void OnMouseExit()
    {
        plot.color = Color.white; // makes the SpriteRenderer go back to default color
    }

    void Harvest()
    {
        isPlanted = false;        
        plant.gameObject.SetActive(false);
        fm.Transaction(selectedPlant.sellPrice);

    }

    void Plant(PlantObject newPlant)
    {
        selectedPlant = newPlant;
        isPlanted = true;

        fm.Transaction(-selectedPlant.buyPrice);

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

    //Refactoring: in Start, assign plantStages[] = selectedPlant.plantStages etc does this work if selected plant changes?
    // Set up FarmManager as singleton --> consequences here?
}
