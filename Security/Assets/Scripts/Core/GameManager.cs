using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    private static GameManager instance;
    public static GameManager Inst
    {
        get
        {
            Init();
            return instance;
        }
    }

    public Sprite crossHair1;
    public Sprite crossHair2;

    Player player;
    ItemDataManager itemDataManager;
    UI_Inventory uI_Inventory;

    public Player Player => Inst.player;
    public ItemDataManager itemData =>Inst.itemDataManager;
    public UI_Inventory UI_Inventory => Inst.uI_Inventory;

    private void Awake()
    {
        Init();

        if(instance != this)
        {
            Destroy(this.gameObject);
        }
        itemDataManager = GetComponent<ItemDataManager>();
        player = FindAnyObjectByType<Player>();
    }

    private void Start()
    {
        player = FindObjectOfType<Player>(true);

        
    }

    static void Init()
    {
        if(instance == null)
        {
            GameObject obj = GameObject.Find("GameManager");
            if(obj == null)
            {
                obj = new GameObject { name = $"GameManager" };
                obj.AddComponent<GameManager>();
            }
            DontDestroyOnLoad(obj);
            instance = obj.GetComponent<GameManager>();
        }
    }

    public void ChangeCrosshair(bool duringInteraction)
    {
        if (duringInteraction)
        {

        }
        else
        {

        }
    }
}
