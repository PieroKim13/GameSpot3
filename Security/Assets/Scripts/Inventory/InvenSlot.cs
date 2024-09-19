using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InvenSlot
{
    /// <summary>
    /// 인벤토리 인덱스
    /// </summary>
    uint slotIndex;

    /// <summary>
    /// 인벤토리의 인덱스를 확인하기 위한 프로퍼티
    /// </summary>
    public uint Index => slotIndex;

    /// <summary>
    /// 이 슬롯에 들어있는 아이템의 종류
    /// </summary>
    ItemData slotItemData = null;

    /// <summary>
    /// 이 슬롯에 들어있는 아이템의 종류르 확인하기 위한 프로퍼티
    /// </summary>
    public ItemData ItemData
    {
        get => slotItemData;
        private set
        {
            if(slotItemData != value)   // 종류가 변경될 때만
            {
                slotItemData = value;   // 아이템 데이터 변경
                onSlotItemChange?.Invoke();   // 데이터 변경!!!
            }
        }
    }

    /// <summary>
    /// 슬롯에 들어있는 아이템의 데이터가 변경됨을 알리는 델리게이트
    /// </summary>
    public Action onSlotItemChange;

    /// <summary>
    /// 슬롯에 아이템이 존재하는지 확인하는 프로퍼티(들어있으면 false, 없으면 true)
    /// </summary>
    public bool IsEmpty => slotItemData == null;

    /// <summary>
    /// 이 슬롯에 들어있는 아이템 갯수
    /// </summary>
    uint itemCount = 0;

    public uint ItemCount
    {
        get => itemCount;
        private set
        {
            if(itemCount != value)  // 갯수 변경될 때만
            {
                itemCount = value;  // 아이템 갯수 변경
                onSlotItemChange?.Invoke(); // 데이터 변경!!!
            }
        }
    }

    /// <summary>
    /// 인벤토리 슬롯 생성자
    /// </summary>
    /// <param name="index">이 슬롯의 인덱스</param>
    public InvenSlot(uint index)
    {
        slotIndex = index;
        itemCount = 0;
    }

    /// <summary>
    /// 이 슬롯에 아이템을 설정하는 함수
    /// </summary>
    /// <param name="data">설정할 아이템 종류</param>
    /// <param name="count">설정할 아이템 갯수</param>
    public void AssingSlotItem(ItemData data, uint count = 1)
    {
        if(data != null)
        {
            ItemData = data;
            ItemCount = count;
        }
        else
        {
            ClearSlotItem();    // data가 null이면 해당 슬롯은 초기화
        }
    }

    /// <summary>
    /// 이 슬롯을 비우는 함수
    /// </summary>
    public void ClearSlotItem()
    {
        ItemData = null;
        ItemCount = 0;
    }
    /// <summary>
    /// 이 슬롯에 아이템 갯수를 증가시는 함수
    /// </summary>
    /// <param name="overCount">추가하다가 넘친 갯수</param>
    /// <param name="increaseCount">증가시킬 갯수</param>
    public bool IncreaseSlotItem(out uint overCount, uint increaseCount = 1)
    {
        bool result;
        int over;

        uint newCount = ItemCount + increaseCount;
        over = (int)newCount - (int)ItemData.maxStackCount;

        if (over > 0)
        {
            // 최대 개수를 넘을 때
            ItemCount = ItemData.maxStackCount;
            overCount = (uint)over;
            result = false;
        }
        else
        {
            // 아닐 때
            ItemCount = newCount;
            overCount = 0;
            result = true;
        }

        return result;
    }

    public void DecreaseSlotItem(uint decreaseCount = 1)
    {
        int newCount = (int)ItemCount - (int)decreaseCount;
        if(newCount < 1)
        {
            // 슬롯 아이템 갯수가 0이하일 때
            ClearSlotItem();
        }
        else
        {
            // 아이템 갯수가 1개 이상 남을 때
            ItemCount = (uint)newCount;
        }
    }


}
