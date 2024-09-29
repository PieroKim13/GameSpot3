using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_SlotBase : MonoBehaviour
{
    /// <summary>
    /// UI가 표현할 슬롯
    /// </summary>
    InvenSlot invenSlot;

    /// <summary>
    /// 슬롯 확인용 프로퍼티
    /// </summary>
    public InvenSlot InvenSlot => invenSlot;

    /// <summary>
    /// 슬롯이 몇번째 슬롯인지 확인하기 위한 프로퍼티
    /// </summary>
    public uint Index => invenSlot.Index;

    /// <summary>
    /// 아이템 아이콘 표시용 이미지
    /// </summary>
    Image itemIcon;

    /// <summary>
    /// 아이템 개수 표시용 텍스트
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
    /// 슬롯 초기화용 함수
    /// </summary>
    /// <param name="slot"></param>
    public virtual void InitializeSlot(InvenSlot slot)
    {
        invenSlot = slot;
        invenSlot.onSlotItemChange = Refresh;
        Refresh();
    }

    /// <summary>
    /// 슬롯이 보이는 모습을 갱신하는 함수
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
    /// 상속받은 클래스에서 개별적으로 실행하고 싶은 코드를 모아놓은 함수
    /// </summary>
    protected virtual void OnRefresh()
    {

    }
}
