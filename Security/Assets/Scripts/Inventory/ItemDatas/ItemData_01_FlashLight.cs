using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Data - FlashLight", menuName = "Scriptable Object/Item Data - FlashLight", order = 2)]
public class ItemData_01_FlashLight : ItemData
{
    [Header("������ ������ ������")]
    public float maxValue = 100;
    public float currentValue;

    public bool switching;

    public bool Use(GameObject target)
    {
        bool result = false;

        if(currentValue > 0.0f)
        {

        }

        return result;
    }


    public void Lighting(GameObject target)
    {

    }


}
