using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlotManager : MonoBehaviour
{
    public Color availableColor = Color.green;
    public Color unAvailableColor = Color.red;
    public Sprite drySprite;
    public Sprite normalSprite;
    public Sprite unavailableSprite; // not yet bought
    public bool isBought = true;

    SpriteRenderer plot;
    SpriteRenderer plant;
    BoxCollider2D plantCollider;
    FarmManager fm;
    PlantObject selectedPlant;


    bool isPlanted = false;
    bool isDry = true;
    int plantStage = 0;
    int fertilizerCost = 10;
    int plotCost = 100;
    float timer;
    float speed;
    float speedDefault = 1f;
    float speedLimit = 2f;
    float speedIncrease = 0.2f;    


    // Start is called before the first frame update
    void Start()
    {
        plant = transform.GetChild(0).GetComponent<SpriteRenderer>();
        plot = GetComponent<SpriteRenderer>();
        plot.sprite = drySprite;
        plantCollider = transform.GetChild(0).GetComponent<BoxCollider2D>();
        fm = transform.parent.GetComponent<FarmManager>();
        speed = speedDefault;
        if (isBought)
        {
            plot.sprite = drySprite;
        }
        else
        {
            plot.sprite = unavailableSprite;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlanted && !isDry)
        {
            timer -= speed * Time.deltaTime;

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
            if(!fm.isPlanting && !fm.isSelecting && (plantStage == (selectedPlant.plantStages.Length - 1)))
                Harvest();
        }         
        else if (isBought && fm.isPlanting && (fm.selectPlant.plant.buyPrice <= fm.money))
        {
            Plant(fm.selectPlant.plant); // can't use selectedPlant here since it's not planted yet
        }

        if (fm.isSelecting)
        {
            switch (fm.selectedTool)
            {
                case 1: // watering
                    if (isBought)
                    {
                        isDry = false;
                        plot.sprite = normalSprite;
                        if (isPlanted)
                            UpdatePlant();
                    }
                    break;

                case 2: // fertilizer
                    if (isBought && (speed < speedLimit) && (fm.money >= fertilizerCost))
                    {
                        fm.Transaction(-fertilizerCost);
                        speed += speedIncrease;
                    }
                    break;
                case 3: // buy plot
                    if (!isBought && (fm.money >= plotCost))
                    {
                        fm.Transaction(-plotCost);
                        isBought = true;
                        plot.sprite = drySprite;
                    }
                    break;                
                default:
                    break;
            }
        }            
    }

    private void OnMouseOver()
        // deals with the visual cue color changes of the plot
    {
        if (fm.isPlanting)
        {
            if (isPlanted || (fm.selectPlant.plant.buyPrice > fm.money) || !isBought)
            {
                plot.color = unAvailableColor;
            }
            else
            {
                plot.color = availableColor;
            }
        }

        if (fm.isSelecting)
        {
            switch (fm.selectedTool)
            {
                case 1:
                case 2:
                    if (isBought && fm.money >= (fm.selectedTool-1) * fertilizerCost) // tool is 0 if watering, 1 if fertilizer
                        plot.color = availableColor;
                    else
                        plot.color = unAvailableColor;
                    break;
                case 3:
                    if (!isBought && fm.money >= plotCost)
                        plot.color = availableColor;
                    else
                        plot.color = unAvailableColor;
                    break;
                default:
                    plot.color = unAvailableColor;
                    break;
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
        isDry = true;
        plot.sprite = drySprite;
        speed = speedDefault; // remove the fertilizer effect when harvesting
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
        if (isDry)
        {
            plant.sprite = selectedPlant.dryPlanted;
        }
        else
        {
            plant.sprite = selectedPlant.plantStages[plantStage];
        }
        plantCollider.size = plant.sprite.bounds.size;
        plantCollider.offset = new Vector2(0, plant.bounds.size.y/2);
    }

    //Refactoring: in Start, assign plantStages[] = selectedPlant.plantStages etc does this work if selected plant changes?
    // Set up FarmManager as singleton --> consequences here?
}
