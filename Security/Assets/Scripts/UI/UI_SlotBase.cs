using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_SlotBase : MonoBehaviour
{
    /// <summary>
    /// UI�� ǥ���� ����
    /// </summary>
    InvenSlot invenSlot;

    /// <summary>
    /// ���� Ȯ�ο� ������Ƽ
    /// </summary>
    public InvenSlot InvenSlot => invenSlot;

    /// <summary>
    /// ������ ���° �������� Ȯ���ϱ� ���� ������Ƽ
    /// </summary>
    public uint Index => invenSlot.Index;

    /// <summary>
    /// ������ ������ ǥ�ÿ� �̹���
    /// </summary>
    Image itemIcon;

    /// <summary>
    /// ������ ���� ǥ�ÿ� �ؽ�Ʈ
    /// </summary>
    TextMeshProUGUI itemCount;

    protected virtual void Awake()
    {
        Transform temp = transform.GetChild(1);
        itemIcon = temp.GetComponent<Image>();

        temp = transform.GetChild(2);
        itemCount = temp.GetComponent<TextMeshProUGUI>();
    }

    /// <summary>
    /// ���� �ʱ�ȭ�� �Լ�
    /// </summary>
    /// <param name="slot"></param>
    public virtual void InitializeSlot(InvenSlot slot)
    {
        invenSlot = slot;
        invenSlot.onSlotItemChange = Refresh;
        Refresh();
    }

    /// <summary>
    /// ������ ���̴� ����� �����ϴ� �Լ�
    /// </summary>
    private void Refresh()
    {
        if (InvenSlot.IsEmpty)
        {
            itemIcon.color = Color.clear;
            itemIcon.sprite = null;
            itemCount.text = string.Empty;
        }
        else
        {
            itemIcon.sprite = InvenSlot.ItemData.itemIcon;
            itemIcon.color = Color.white;
            itemCount.text = InvenSlot.ItemCount.ToString();
        }

        OnRefresh();
    }

    /// <summary>
    /// ��ӹ��� Ŭ�������� ���������� �����ϰ� ���� �ڵ带 ��Ƴ��� �Լ�
    /// </summary>
    protected virtual void OnRefresh()
    {

    }
}
