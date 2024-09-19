using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    /// <summary>
    /// �κ��丮�� ����ִ� �κ� ������ �⺻ ����
    /// </summary>
    public const int Defualt_Inventory_Size = 8;

    /// <summary>
    /// �ӽ� ���Կ� �ε���
    /// </summary>
    public const uint Temp_Inventory_Index = 64;

    /// <summary>
    /// �κ��丮 ���� �迭
    /// </summary>
    InvenSlot[] slots;

    /// <summary>
    /// �κ��丮 ���Կ� �����ϱ� ���� �ε���
    /// </summary>
    /// <param name="index">������ �ε���</param>
    /// <returns></returns>
    public InvenSlot this[uint index] => slots[index];

    /// <summary>
    /// �κ��丮 ���� ����
    /// </summary>
    public int SlotCount => slots.Length;

    /// <summary>
    /// �ӽ� ����
    /// </summary>
    InvenSlot tempSlot;
    public InvenSlot TempSlot => tempSlot;

    /// <summary>
    /// ������ ������ �Ŵ���
    /// </summary>
    ItemDataManager itemDataManager;

    /// <summary>
    /// �κ��丮 ������
    /// </summary>
    Player owner;
    public Player Owner => owner;

    /// <summary>
    /// �κ��丮 ������
    /// </summary>
    /// <param name="owner">�κ��丮 ������</param>
    /// <param name="size">�κ��丮 ũ��</param>
    public Inventory(Player owner, uint size = Defualt_Inventory_Size)
    {
        slots = new InvenSlot[size];

        for (uint i = 0; i < size; i++)
        {
            slots[i] = new InvenSlot(i);    // ���� ���� ����
        }
        tempSlot = new InvenSlot(Temp_Inventory_Index);
        itemDataManager = GameManager.Inst.itemData;    // ������ ������ �Ŵ��� ĳ��
        this.owner = owner;
    }

    /// <summary>
    /// �κ��丮�� �������� �ϳ� �߰��ϴ� �Լ�
    /// </summary>
    /// <param name="code">�߰��� ������ ����</param>
    /// <returns>�����ϸ� true, �����ϸ� false</returns>
    public bool AddItem(ItemCode code)
    {
        bool result = false;
        ItemData data = itemDataManager[code];

        InvenSlot sameDataSlot = FindSameItem(data);
        if(sameDataSlot != null)
        {
            // ���� ������ �������� ����
            // ������ ���� 1 ������Ű�� ��� �ޱ�
            result = sameDataSlot.IncreaseSlotItem(out _);  // ��ġ�� ������ �ǹ� ��� ���� ���� ����
        }
        else
        {
            // ���� ������ �������� ����.
            InvenSlot emptySlot = FindEmptySlot();
            if(emptySlot != null)
            {
                emptySlot.AssingSlotItem(data); // �� ������ ������ ������ �߰�
                result = true;
            }
            else
            {
                // ����ִ� ������ ����.
            }
        }

        return result;
    }

    /// <summary>
    /// �κ��丮�� Ư�� ���Կ� �������� �ϳ� �߰��ϴ� �Լ�
    /// </summary>
    /// <param name="code">�߰��� �������� ����</param>
    /// <param name="slotIndex">�������� �߰��� �ε���</param>
    /// <returns></returns>
    public bool AddItem(ItemCode code, uint slotIndex)
    {
        bool result = false;

        if (IsValidIndex(slotIndex))    // �ε����� �������� Ȯ��
        {
            ItemData data = itemDataManager[code];  // ������ ������ ��������
            InvenSlot slot = slots[slotIndex];  // �������� �߰��� ���� ��������
            if (slot.IsEmpty)
            {
                slot.AssingSlotItem(data);  // ������ ����ִٸ� ������ �Ҵ�
            }
            else
            {
                // ���Կ� �������� �ִٸ�
                if (slot.ItemData == data)  // ������ ������ ���ٸ�
                {
                    result = slot.IncreaseSlotItem(out _);  // ������ ���� ����
                }
                else
                {
                    // ������ ������ �ٸ��� ����
                }
            }
        }
        else
        {
            // �ε����� �߸��� ��쵵 ����
        }

        return result;
    }

    /// <summary>
    /// �κ��丮���� �������� ���� ������ŭ ���ҽ�Ű�� �Լ�
    /// </summary>
    /// <param name="slotIndex">�������� ���ҽ�ų ������ �ε���</param>
    /// <param name="decreaseCount">���ҽ�ų ����</param>
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
    /// �κ��丮���� �������� �����ϴ� �Լ�
    /// </summary>
    /// <param name="slotIndex">�������� ������ ������ �ε���</param>
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
    /// �κ��丮�� ���� ���� �Լ�
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
            InvenSlot fromSlot = (from == Temp_Inventory_Index) ? TempSlot : slots[from];   // �ӽ� ������ �����ؼ����⿬���ڷ� ó��
            if (!fromSlot.IsEmpty)
            {
                InvenSlot toSlot = (to == Temp_Inventory_Index) ? TempSlot : slots[to];
                if(fromSlot.ItemData == toSlot.ItemData)    // ���� ������ ������ �̸�
                {
                    toSlot.IncreaseSlotItem(out uint overCount, fromSlot.ItemCount);    // �ϴ� from�� ���� ������ŭ to ����
                    fromSlot.DecreaseSlotItem(fromSlot.ItemCount - overCount);  // from���� �Ѿ ������ŭ from ����
                }
                else
                {
                    // �ٸ� ������ �������̸� ���� ����
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
        List<InvenSlot> beforeSlots = new List<InvenSlot>(slots);   // slots �迭�� �̿��ؼ� ����Ʈ �����

        switch(sortBy)  // ���� ���ؿ� ���� ó��
        {
            case ItemSortyBy.Code:
                beforeSlots.Sort((x, y) =>  // x, y�� ���� ���� 2��
                {
                    if (x.ItemData == null) // itemData�� ������� �� ������ ��������� ����ִ� ���� �������� ����
                        return 1;
                    if (y.ItemData == null)
                        return -1;
                    if (isAcending)
                    {
                        return x.ItemData.code.CompareTo(y.ItemData.code);  // enum�� ������ CompareTo �Լ��� ��(���������� ��)
                    }
                    else
                    {
                        return y.ItemData.code.CompareTo(x.ItemData.code);  // ""(���������� ��)
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

        //beforeSlots�� ������ ���ؿ� ���� ���� �Ϸ�

        //������ ������ ������ ���� �����ϱ�
        List<(ItemData, uint)> sortedData = new List<(ItemData, uint)> (SlotCount);
        foreach(var slot in beforeSlots)
        {
            sortedData.Add((slot.ItemData, slot.ItemCount));
        }

        // ���Կ� ������ ������ ������ ������� �Ҵ��ϱ�
        int index = 0;
        foreach(var data in sortedData)
        {
            slots[index].AssingSlotItem(data.Item1, data.Item2);
            index++;
        }

        RefreshInventory();
    }

    /// <summary>
    /// ��� ������ ����Ǿ����� �˸��� �Լ�
    /// </summary>
    void RefreshInventory()
    {
        foreach(var slot in slots)
        {
            slot.onSlotItemChange?.Invoke();
        }
    }

    /// <summary>
    /// �κ��丮 �Ķ���Ϳ� ���� ������ �������� ����ִ� ������ ã�� �Լ�
    /// </summary>
    /// <param name="data">ã�� ������ ����</param>
    /// <returns>���� ������ �������� �κ��丮�� ������ null, ������ ��ȯ</returns>
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
    /// �κ��丮���� ��� �ִ� ������ ã�� �Լ�
    /// </summary>
    /// <returns>����ִ� ���� ��ġ</returns>
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
    /// �� ������ ��ã�� �� �ε���
    /// </summary>
    const uint NotFindEmptySlot = uint.MaxValue;

    /// <summary>
    /// ����ִ� ������ �ε����� �����ִ� �Լ�
    /// </summary>
    /// <param name="index">��¿� �Ķ����, �� ������ ã���� �� �ε��� ��</param>
    /// <returns>�� ������ ã���� �� true, ��ã���� �� false</returns>
    public bool FindEmptySlotIndex(out uint index)
    {
        bool result = false;
        index = NotFindEmptySlot;

        InvenSlot slot = FindEmptySlot();   // �� ������ ã�Ƽ�
        if(slot != null)
        {
            index = slot.Index; // �� ������ ������ �ε��� ����
            result = true;
        }

        return result;
    }

    /// <summary>
    /// ������ �ε������� Ȯ���ϴ� �Լ�
    /// </summary>
    /// <param name="index">Ȯ���� �ε���</param>
    /// <returns>������ �ε����� true, �������� �ʴٸ� false</returns>
    bool IsValidIndex(uint index) => (index < SlotCount) || (index == Temp_Inventory_Index);

    /// <summary>
    /// �׽�Ʈ�� : �κ��丮 ���� ������ �ܼ�â�� ����ϴ� �Լ�
    /// </summary>
    public void PrintInventory()
    {
        // ����
        // [ ���(1/3), �����̾�(1/5), ���޶���(2/5), (��ĭ), (��ĭ), (��ĭ) ]

        string printText = "[ ";

        for (int i = 0; i < SlotCount - 1; i++)
        {
            if (!slots[i].IsEmpty)
            {
                printText += $"{slots[i].ItemData.itemName}({slots[i].ItemCount}/{slots[i].ItemData.maxStackCount})";
            }
            else
            {
                printText += "(��ĭ)";
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
            printText += "(��ĭ)";
        }
        printText += " ]";

        Debug.Log(printText);
    }
}
