using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Inventory : MonoBehaviour
{
    /// <summary>
    /// UI�� ������ �κ��丮
    /// </summary>
    Inventory inven;

    /// <summary>
    /// �κ��丮�� �������ִ� ������ ����
    /// </summary>
    public UI_InvenSlot slotUI;

    /// <summary>
    /// �κ��丮�� �����ڸ� Ȯ���ϱ� ���� ������Ƽ
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
