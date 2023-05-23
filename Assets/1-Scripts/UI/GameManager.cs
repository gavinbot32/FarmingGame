using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using Unity.VisualScripting;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class GameManager : MonoBehaviour
{
    public int curDay;
    public float money;
    public CropData cropSelected;
    public CropData[] cropOptions;
    public int cropInventory;

    public event UnityAction onNewDayEvent;


    public int hour;
    public int minute;
    public int updateCounter = 0;
    public int scaleValue = 60;

    public static GameManager instance;
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI seedText;
    public TextMeshProUGUI dateText;
    public TextMeshProUGUI timeText;
    public TMP_Dropdown cropSelector;

    public PlayerController player;

    public Light2D globalLight;

    private void Awake()
    {
        print(cropSelector.value);

        if(instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
        cropSelected = cropOptions[cropSelector.value];
        curDay = 0;
        setNextDay();
    }

    private void FixedUpdate()
    {
        updateCounter++;
        if(updateCounter >= scaleValue)
        {
            updateCounter = 0;
            minute++;
            globalLight.intensity += 0.01f;
        }
        if( minute >= 60)
        {
            hour++;
            minute = 0;
        }
        if(hour >= 24)
        {
            setNextDay();
        }
        updateUI();
    }

    private void OnEnable()
    {
        // listening for events
        Crop.onPlantCropEvent += onPlantCrop;
        Crop.onHarvestCropEvent += onHarvestCrop;
    }
    private void OnDisable()
    {
        //stop listening for events
        Crop.onPlantCropEvent -= onPlantCrop;
        Crop.onHarvestCropEvent -= onHarvestCrop;

    }
    public void onPlantCrop(CropData crop)
    {
        player.seedInventory[cropSelector.value]--;
        cropInventory = player.seedInventory[cropSelector.value];
        updateUI();
    }

    public void onHarvestCrop(CropData crop)
    {
        money += crop.sellPrice;
        updateUI();
    }
    private void Start()
    {
        updateUI();
    }

   
    public void updateUI()
    {
        string minuteText;

        //Change seed count
        moneyText.text = this.money.ToString();
        seedText.text = player.seedInventory[cropSelector.value].ToString();

        minuteText = minute.ToString();
        if(minute <= 9)
        {
            minuteText = 0.ToString() +minute.ToString();
        }

        //Change money text
        timeText.text = hour.ToString() + ":" + minuteText;
        dateText.text = curDay.ToString();
    }

    public void setNextDay()
    {
        curDay++;
        hour = 4;
        minute = 30;
        globalLight.intensity = 0.1f;
        onNewDayEvent ?.Invoke();
        updateUI();
    }

    public void purchaseCrops(CropData crop)
    {
        money -= crop.getPurchasePrice();
        player.seedInventory[cropSelector.value]++;
        cropInventory = player.seedInventory[cropSelector.value];
        updateUI();
    }
    public bool canPlantCrop()
    {
            return cropInventory > 0;
    }
    public void onBuyCropBttn() {
        


        if(money >= cropSelected.getPurchasePrice())
        {
            purchaseCrops(cropSelected);
        }
    }
    public void onSwitchCropBttn()
    {
        cropSelected = cropOptions[cropSelector.value];
        cropInventory = player.seedInventory[cropSelector.value];
        updateUI();

    }
    public int getCurDay()
    {
        return curDay;
    }
    public CropData getselectedCrop()
    {
        return this.cropSelected;
    }

}

