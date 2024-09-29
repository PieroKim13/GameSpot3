using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    Player player;
    public Player Player => player;

    ItemDataManager itemDataManager;
    public ItemDataManager itemData => itemDataManager;

    UI_Inventory uI_Inventory;
    public UI_Inventory UI_Inventory => uI_Inventory;

    protected override void OnPreInitialize()
    {
        base.OnPreInitialize();
        itemDataManager = GetComponent<ItemDataManager>();
    }

    protected override void OnIntialize()
    {
        base.OnIntialize();
        player = FindObjectOfType<Player>();
        uI_Inventory = FindObjectOfType<UI_Inventory>();
    }
}
