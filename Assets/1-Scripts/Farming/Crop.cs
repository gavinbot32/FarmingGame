using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Crop : MonoBehaviour
{

    private CropData curCrop;
    private int plantDay;
    private int daysLastWatered;
    public SpriteRenderer sr;

    public static event UnityAction<CropData> onPlantCropEvent;
    public static event UnityAction<CropData> onHarvestCropEvent;



    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }
    public void plant(CropData crop)
    {
        curCrop = crop;
        plantDay = GameManager.instance.getCurDay();
        daysLastWatered = 1;
        updateCropSprite();
        onPlantCropEvent?.Invoke(crop);
    }

    public void newDayCheck()
    {
        

        daysLastWatered++;
        if(daysLastWatered > curCrop.getNoWaterTime())
        {
            Destroy(gameObject);
        }
        updateCropSprite() ;
    }
    public void updateCropSprite()
    {
        int cropProg = cropProgress();
       sr.sprite = curCrop.getSprite(cropProg);
    }
    private int cropProgress()
    {
        return GameManager.instance.getCurDay() - plantDay;
    }
    public void onWater()
    {
        daysLastWatered = 0;
    }
    public void harvest()
    {
        if (canHarvest())
        {
            onHarvestCropEvent?.Invoke(curCrop);
            Destroy(gameObject);

        }
    }
    public bool canHarvest()
    {
        return cropProgress() >= curCrop.getDaysToGrow();
    }

}
