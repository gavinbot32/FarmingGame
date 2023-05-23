using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class FieldTile : MonoBehaviour
{
    [Header("Prefab")]
    public GameObject cropPrefab;


    [Header("Crop")]
    private Crop currentCrop;


    [Header("Components")]
    public SpriteRenderer sr;

    [Header("Tile States")]
    public bool isTilled;
    public bool isWatered;
    public bool hasCrop;


    public Sprite[] sprites;
    public int spriteIndex = 0;


    private void OnDisable()
    {
        GameManager.instance.onNewDayEvent -= onNewDay;
    }

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        spriteIndex = 0;
        sr.sprite = sprites[spriteIndex];
    }

    public void onNewDay()
    {
        isWatered = false;


        if (currentCrop == null)
        {
            isTilled = false;
            if (spriteIndex > 0)
            { spriteIndex--; }
            if(spriteIndex == 0)
            {
                GameManager.instance.onNewDayEvent -= onNewDay;

            }
        }
        else if(currentCrop != null)
        {
            spriteIndex = 1;
            currentCrop.newDayCheck();
        }
        updateSprite();
    }

    public void updateSprite()
    {
        sr.sprite = sprites[spriteIndex];
    }
    void Start()
    {
       

        
    }

    void Update()
    {
        
    }
    public void till()
    {
        isTilled = true;
        spriteIndex = 1;
        GameManager.instance.onNewDayEvent += onNewDay;

    }
    public void water()
    {
        isWatered = true;
        spriteIndex = 2;
        if (hasCrop)
        {
            currentCrop.onWater();

        }
        
    }
    public void interact()
    {
        /*
        spriteIndex++;
        if(spriteIndex >= sprites.Length)
        {
            spriteIndex = sprites.Length - 1;
        }
        sr.sprite = sprites[spriteIndex];

        switch (spriteIndex)
        {
            case 0:
                isTilled = false;
                isWatered = false;
                break;
            case 1:
                isTilled = true;
                isWatered = false;
                   break;
            case 2:
                isTilled = true;
                isWatered = true;
                break;

        }
*/

        if(!isTilled)
        {
            till();
        }else if(!hasCrop && GameManager.instance.canPlantCrop())
        {

            plantNewCrop(GameManager.instance.getselectedCrop());
        }
        else if(hasCrop && currentCrop.canHarvest())
        {
            hasCrop = false;
            currentCrop.harvest();
        }
        else
        {
            water();
        }
        updateSprite();
    }

    public void plantNewCrop(CropData crop)
    {
        if (!isTilled)
        {
            return;
        }
        hasCrop = true;
        currentCrop = Instantiate(cropPrefab, transform).GetComponent<Crop>();
        currentCrop.plant(crop);

    }

}
