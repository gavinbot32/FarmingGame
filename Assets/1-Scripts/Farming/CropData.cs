using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Crop Data",menuName = "New Crop Data")]

public class CropData : ScriptableObject
{
    [Header("Crop Info")]
    public int daysToGrow;
    public Sprite[] growProgressSprites;
    public Sprite harvestSprite;
    public int noWaterTime;

    [Header("Econ")]
    public float purchasePrice;
    public float sellPrice;
    
    public int getNoWaterTime()
    {
        return noWaterTime;
    }

    public int getDaysToGrow()
    {
           return daysToGrow;
    }

    public Sprite getSprite(int num)
    {
        if( num < daysToGrow)
        {
            return growProgressSprites[num];
        }
        else
        {
            return harvestSprite;
        }
    }

    public float getPurchasePrice()
    {
        return purchasePrice;
    }
    public float getSellPrice()
    {
        return sellPrice;
    }

}
