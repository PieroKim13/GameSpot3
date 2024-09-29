using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Inventory : MonoBehaviour
{
    /// <summary>
    /// UI가 보여줄 인벤토리
    /// </summary>
    Inventory inven;

    /// <summary>
    /// 인벤토리가 가지고있는 아이템 슬롯
    /// </summary>
    public UI_InvenSlot slotUI;

    /// <summary>
    /// 인벤토리의 소유자를 확인하기 위한 프로퍼티
    /// </summary>
    public Player Owner => inven.Owner;

    private void Awake()
    {
        Transform temp = transform.GetChild(0);
        slotUI = temp.GetComponent<UI_InvenSlot>();
    }

    public void InitializeInventory(Inventory playerInven)
    {
        inven = playerInven;

        slotUI.InitializeSlot(inven[0]);

    }
}
