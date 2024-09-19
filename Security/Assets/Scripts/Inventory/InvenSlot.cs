using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InvenSlot
{
    /// <summary>
    /// �κ��丮 �ε���
    /// </summary>
    uint slotIndex;

    /// <summary>
    /// �κ��丮�� �ε����� Ȯ���ϱ� ���� ������Ƽ
    /// </summary>
    public uint Index => slotIndex;

    /// <summary>
    /// �� ���Կ� ����ִ� �������� ����
    /// </summary>
    ItemData slotItemData = null;

    /// <summary>
    /// �� ���Կ� ����ִ� �������� ������ Ȯ���ϱ� ���� ������Ƽ
    /// </summary>
    public ItemData ItemData
    {
        get => slotItemData;
        private set
        {
            if(slotItemData != value)   // ������ ����� ����
            {
                slotItemData = value;   // ������ ������ ����
                onSlotItemChange?.Invoke();   // ������ ����!!!
            }
        }
    }

    /// <summary>
    /// ���Կ� ����ִ� �������� �����Ͱ� ������� �˸��� ��������Ʈ
    /// </summary>
    public Action onSlotItemChange;

    /// <summary>
    /// ���Կ� �������� �����ϴ��� Ȯ���ϴ� ������Ƽ(��������� false, ������ true)
    /// </summary>
    public bool IsEmpty => slotItemData == null;

    /// <summary>
    /// �� ���Կ� ����ִ� ������ ����
    /// </summary>
    uint itemCount = 0;

    public uint ItemCount
    {
        get => itemCount;
        private set
        {
            if(itemCount != value)  // ���� ����� ����
            {
                itemCount = value;  // ������ ���� ����
                onSlotItemChange?.Invoke(); // ������ ����!!!
            }
        }
    }

    /// <summary>
    /// �κ��丮 ���� ������
    /// </summary>
    /// <param name="index">�� ������ �ε���</param>
    public InvenSlot(uint index)
    {
        slotIndex = index;
        itemCount = 0;
    }

    /// <summary>
    /// �� ���Կ� �������� �����ϴ� �Լ�
    /// </summary>
    /// <param name="data">������ ������ ����</param>
    /// <param name="count">������ ������ ����</param>
    public void AssingSlotItem(ItemData data, uint count = 1)
    {
        if(data != null)
        {
            ItemData = data;
            ItemCount = count;
        }
        else
        {
            ClearSlotItem();    // data�� null�̸� �ش� ������ �ʱ�ȭ
        }
    }

    /// <summary>
    /// �� ������ ���� �Լ�
    /// </summary>
    public void ClearSlotItem()
    {
        ItemData = null;
        ItemCount = 0;
    }
    /// <summary>
    /// �� ���Կ� ������ ������ �����ô� �Լ�
    /// </summary>
    /// <param name="overCount">�߰��ϴٰ� ��ģ ����</param>
    /// <param name="increaseCount">������ų ����</param>
    public bool IncreaseSlotItem(out uint overCount, uint increaseCount = 1)
    {
        bool result;
        int over;

        uint newCount = ItemCount + increaseCount;
        over = (int)newCount - (int)ItemData.maxStackCount;

        if (over > 0)
        {
            // �ִ� ������ ���� ��
            ItemCount = ItemData.maxStackCount;
            overCount = (uint)over;
            result = false;
        }
        else
        {
            // �ƴ� ��
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
            // ���� ������ ������ 0������ ��
            ClearSlotItem();
        }
        else
        {
            // ������ ������ 1�� �̻� ���� ��
            ItemCount = (uint)newCount;
        }
    }


}
