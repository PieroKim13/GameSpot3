using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    public ItemData itemData = null;
    public ItemData ItemData
    {
        get => itemData;
        set
        {
            if (itemData == null)
            {
                itemData = value;
            }
        }
    }
}
