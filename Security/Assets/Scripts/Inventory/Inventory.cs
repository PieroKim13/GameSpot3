using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    /// <summary>
    /// 인벤토리에 들어있는 인벤 슬롯의 기본 갯수
    /// </summary>
    public const int Defualt_Inventory_Size = 8;

    /// <summary>
    /// 임시 슬롯용 인덱스
    /// </summary>
    public const uint Temp_Inventory_Index = 64;

    /// <summary>
    /// 인벤토리 슬롯 배열
    /// </summary>
    InvenSlot[] slots;

    /// <summary>
    /// 인벤토리 슬롯에 접근하기 위한 인덱서
    /// </summary>
    /// <param name="index">슬롯의 인덱스</param>
    /// <returns></returns>
    public InvenSlot this[uint index] => slots[index];

    /// <summary>
    /// 인벤토리 슬롯 갯수
    /// </summary>
    public int SlotCount => slots.Length;

    /// <summary>
    /// 임시 슬롯
    /// </summary>
    InvenSlot tempSlot;
    public InvenSlot TempSlot => tempSlot;

    /// <summary>
    /// 아이템 데이터 매니저
    /// </summary>
    ItemDataManager itemDataManager;

    /// <summary>
    /// 인벤토리 소유자
    /// </summary>
    Player owner;
    public Player Owner => owner;

    /// <summary>
    /// 인벤토리 생성자
    /// </summary>
    /// <param name="owner">인벤토리 소유자</param>
    /// <param name="size">인벤토리 크기</param>
    public Inventory(Player owner, uint size = Defualt_Inventory_Size)
    {
        slots = new InvenSlot[size];

        for (uint i = 0; i < size; i++)
        {
            slots[i] = new InvenSlot(i);    // 슬롯 만들어서 저장
        }
        tempSlot = new InvenSlot(Temp_Inventory_Index);
        itemDataManager = GameManager.Inst.itemData;    // 아이템 데이터 매니저 캐싱
        this.owner = owner;
    }

    /// <summary>
    /// 인벤토리에 아이템을 하나 추가하는 함수
    /// </summary>
    /// <param name="code">추가할 아이템 종류</param>
    /// <returns>성공하면 true, 실패하면 false</returns>
    public bool AddItem(ItemCode code)
    {
        bool result = false;
        ItemData data = itemDataManager[code];

        InvenSlot sameDataSlot = FindSameItem(data);
        if(sameDataSlot != null)
        {
            // 같은 종류의 아이템이 존재
            // 아이템 개수 1 증가시키고 결과 받기
            result = sameDataSlot.IncreaseSlotItem(out _);  // 넘치는 갯수가 의미 없어서 따로 받지 않음
        }
        else
        {
            // 같은 종류의 아이템이 없다.
            InvenSlot emptySlot = FindEmptySlot();
            if(emptySlot != null)
            {
                emptySlot.AssingSlotItem(data); // 빈 슬롯이 있으면 아이템 추가
                result = true;
            }
            else
            {
                // 비어있는 슬롯이 없다.
            }
        }

        return result;
    }

    /// <summary>
    /// 인벤토리의 특정 슬롯에 아이템을 하나 추가하는 함수
    /// </summary>
    /// <param name="code">추가할 아이템의 종류</param>
    /// <param name="slotIndex">아이템을 추가할 인덱스</param>
    /// <returns></returns>
    public bool AddItem(ItemCode code, uint slotIndex)
    {
        bool result = false;

        if (IsValidIndex(slotIndex))    // 인덱스가 적절한지 확인
        {
            ItemData data = itemDataManager[code];  // 아이템 데이터 가져오기
            InvenSlot slot = slots[slotIndex];  // 아이템을 추가할 슬롯 가져오기
            if (slot.IsEmpty)
            {
                slot.AssingSlotItem(data);  // 슬롯이 비어있다면 아이템 할당
            }
            else
            {
                // 슬롯에 아이템이 있다면
                if (slot.ItemData == data)  // 아이템 종류가 같다면
                {
                    result = slot.IncreaseSlotItem(out _);  // 아이템 갯수 증가
                }
                else
                {
                    // 아이템 종류가 다르면 실패
                }
            }
        }
        else
        {
            // 인덱스가 잘못된 경우도 실패
        }

        return result;
    }

    /// <summary>
    /// 인벤토리에서 아이템을 일정 갯수만큼 감소시키는 함수
    /// </summary>
    /// <param name="slotIndex">아이템을 감소시킬 슬롯의 인덱스</param>
    /// <param name="decreaseCount">감소시킬 갯수</param>
    public void RemoveItem(uint slotIndex, uint decreaseCount = 1)
    {
        if (IsValidIndex(slotIndex))
        {
            InvenSlot invenSlot = slots[slotIndex];
            invenSlot.DecreaseSlotItem(decreaseCount);
        }
        else
        {

        }
    }

    /// <summary>
    /// 인벤토리에서 아이템을 삭제하는 함수
    /// </summary>
    /// <param name="slotIndex">아이템을 삭제할 슬롯의 인덱스</param>
    public void ClearSlot(uint slotIndex)
    {
        if (IsValidIndex(slotIndex))
        {
            InvenSlot invenslot = slots[slotIndex];
            invenslot.ClearSlotItem();
        }
        else
        {

        }
    }

    /// <summary>
    /// 인벤토리를 전부 비우는 함수
    /// </summary>
    public void ClearInventory()
    {
        foreach(var slot in slots)
        {
            slot.ClearSlotItem();
        }
    }

    
    public void MoveItem(uint from, uint to)
    {
        if((from != to)&&IsValidIndex(from) && IsValidIndex(to))
        {
            InvenSlot fromSlot = (from == Temp_Inventory_Index) ? TempSlot : slots[from];   // 임시 슬롯을 감안해서삼향연선자로 처리
            if (!fromSlot.IsEmpty)
            {
                InvenSlot toSlot = (to == Temp_Inventory_Index) ? TempSlot : slots[to];
                if(fromSlot.ItemData == toSlot.ItemData)    // 같은 종류의 아이템 이면
                {
                    toSlot.IncreaseSlotItem(out uint overCount, fromSlot.ItemCount);    // 일단 from이 가진 갯수만큼 to 감소
                    fromSlot.DecreaseSlotItem(fromSlot.ItemCount - overCount);  // from에서 넘어간 갯수만큼 from 감소
                }
                else
                {
                    // 다른 종류의 아이템이면 서로 스왑
                    ItemData tempData = fromSlot.ItemData;
                    uint tempCount = fromSlot.ItemCount;
                    fromSlot.AssingSlotItem(toSlot.ItemData, toSlot.ItemCount);
                    toSlot.AssingSlotItem(tempData, tempCount);
                }
            }
        }
    }

    public void SlotSorting(ItemSortyBy sortBy, bool isAcending = true)
    {
        List<InvenSlot> beforeSlots = new List<InvenSlot>(slots);   // slots 배열을 이용해서 리스트 만들기

        switch(sortBy)  // 정려 기준에 따라 처리
        {
            case ItemSortyBy.Code:
                beforeSlots.Sort((x, y) =>  // x, y는 서로 비교할 2개
                {
                    if (x.ItemData == null) // itemData는 비어있을 수 있으니 비어있으면 비어있는 것이 뒤쪽으로 설정
                        return 1;
                    if (y.ItemData == null)
                        return -1;
                    if (isAcending)
                    {
                        return x.ItemData.code.CompareTo(y.ItemData.code);  // enum이 가지는 CompareTo 함수로 비교(오름차순일 때)
                    }
                    else
                    {
                        return y.ItemData.code.CompareTo(x.ItemData.code);  // ""(내림차순일 때)
                    }

                });
                break;
            case ItemSortyBy.Name:
                beforeSlots.Sort((x, y) =>
                {
                    if (x.ItemData == null)
                        return 1;
                    if (y.ItemData == null)
                        return -1;
                    if (!isAcending)
                    {
                        return x.ItemData.itemName.CompareTo(y.ItemData.itemName);
                    }
                    else
                    {
                        return y.ItemData.itemName.CompareTo(x.ItemData.itemName);
                    }
                });
                break;
        }

        //beforeSlots은 정해진 기준에 따라 정렬 완료

        //아이템 종류와 갯수를 따로 저장하기
        List<(ItemData, uint)> sortedData = new List<(ItemData, uint)> (SlotCount);
        foreach(var slot in beforeSlots)
        {
            sortedData.Add((slot.ItemData, slot.ItemCount));
        }

        // 슬롯에 아이템 종류와 갯수를 순서대로 할당하기
        int index = 0;
        foreach(var data in sortedData)
        {
            slots[index].AssingSlotItem(data.Item1, data.Item2);
            index++;
        }

        RefreshInventory();
    }

    /// <summary>
    /// 모든 슬롯이 변경되었음을 알리는 함수
    /// </summary>
    void RefreshInventory()
    {
        foreach(var slot in slots)
        {
            slot.onSlotItemChange?.Invoke();
        }
    }

    /// <summary>
    /// 인벤토리 파라메터와 같은 종류의 아이템이 들어있는 슬롯을 찾는 함수
    /// </summary>
    /// <param name="data">찾을 아이템 종류</param>
    /// <returns>같은 종류의 아이템이 인벤토리에 없으면 null, 있으면 반환</returns>
    InvenSlot FindSameItem(ItemData data)
    {
        InvenSlot findSlot = null;
        foreach(var slot in slots)
        {
            if(slot.ItemData == data && slot.ItemCount < slot.ItemData.maxStackCount)
            {
                findSlot = slot;
                break;
            }
        }

        return findSlot;
    }

    /// <summary>
    /// 인벤토리에서 비어 있는 슬롯을 찾는 함수
    /// </summary>
    /// <returns>비어있는 슬롯 위치</returns>
    InvenSlot FindEmptySlot()
    {
        InvenSlot findSlot = null;
        foreach(var slot in slots)
        {
            if (slot.IsEmpty)
            {
                findSlot = slot;
                break;
            }
        }

        return findSlot;
    }

    /// <summary>
    /// 빈 슬롯을 못찾을 때 인덱스
    /// </summary>
    const uint NotFindEmptySlot = uint.MaxValue;

    /// <summary>
    /// 비어있는 슬롯의 인덱스를 돌려주는 함수
    /// </summary>
    /// <param name="index">출력용 파라메터, 빈 슬롯을 찾았을 때 인덱스 값</param>
    /// <returns>빈 슬롯을 찾았을 때 true, 못찾았을 때 false</returns>
    public bool FindEmptySlotIndex(out uint index)
    {
        bool result = false;
        index = NotFindEmptySlot;

        InvenSlot slot = FindEmptySlot();   // 빈 슬롯을 찾아서
        if(slot != null)
        {
            index = slot.Index; // 빈 슬롯이 있으면 인덱스 설정
            result = true;
        }

        return result;
    }

    /// <summary>
    /// 적절한 인덱스인지 확인하는 함수
    /// </summary>
    /// <param name="index">확인할 인덱스</param>
    /// <returns>적절한 인덱스면 true, 적절하지 않다면 false</returns>
    bool IsValidIndex(uint index) => (index < SlotCount) || (index == Temp_Inventory_Index);

    /// <summary>
    /// 테스트용 : 인벤토리 안의 내용을 콘솔창에 출력하는 함수
    /// </summary>
    public void PrintInventory()
    {
        // 예시
        // [ 루비(1/3), 사파이어(1/5), 에메랄드(2/5), (빈칸), (빈칸), (빈칸) ]

        string printText = "[ ";

        for (int i = 0; i < SlotCount - 1; i++)
        {
            if (!slots[i].IsEmpty)
            {
                printText += $"{slots[i].ItemData.itemName}({slots[i].ItemCount}/{slots[i].ItemData.maxStackCount})";
            }
            else
            {
                printText += "(빈칸)";
            }
            printText += ", ";
        }
        InvenSlot last = slots[SlotCount - 1];
        if (!last.IsEmpty)
        {
            printText += $"{last.ItemData.itemName}({last.ItemCount}/{last.ItemData.maxStackCount})";
        }
        else
        {
            printText += "(빈칸)";
        }
        printText += " ]";

        Debug.Log(printText);
    }
}
