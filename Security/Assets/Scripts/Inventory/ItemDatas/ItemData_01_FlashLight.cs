using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Data - FlashLight", menuName = "Scriptable Object/Item Data - FlashLight", order = 2)]
public class ItemData_01_FlashLight : ItemData
{
    [Header("손전등 아이템 데이터")]
    public float Battery_value = 100;

    public GameObject mFlash_Off;
    public GameObject mFlash_On;

    public void Awake()
    {
        mFlash_Off = modelPregab.gameObject.transform.GetChild(0).gameObject;
        mFlash_Off.SetActive(true);

        mFlash_On = modelPregab.gameObject.transform.GetChild(1).gameObject;
        mFlash_On.SetActive(false);


    }

    public void Used()
    {
        
    }
}
