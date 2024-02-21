using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 역할 : 아이템들을 관리해주는 관리자
// 데이터 관리 - > 데이터를 생성, 수정, 삭제, 조회(검색), 정렬

public class ItemManager : MonoBehaviour 
{
    public static ItemManager Instance { get; private set; }
    public GameObject Player;
    public Text HealthItemCountTextUI;
    public Text StaminaCountTextUI;
    public Text BulletCountTextUI;

    public GameObject HealthItemPrefab;
    public GameObject StaminaItemPrefab;
    public GameObject BulletItemPrefab;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public List<Item> ItemList = new List<Item>();

    private void Start()
    {
        ItemList.Add(new Item(ItemType.Health, 3));
        ItemList.Add(new Item(ItemType.Stamina, 5));
        ItemList.Add(new Item(ItemType.Bullet, 7));

        ItemList[0].ItemType = ItemType.Health;
        ItemList[0].Count = GetItemCount(ItemType.Health);

        ItemList[1].ItemType = ItemType.Stamina;
        ItemList[1].Count = GetItemCount(ItemType.Stamina);

        ItemList[2].ItemType = ItemType.Bullet;
        ItemList[2].Count = GetItemCount(ItemType.Bullet);
        RefreshUI();

    }
    private void Update()
    {
        RefreshUI();
    }

    public void SpawnItem(ItemType itemType, Transform spawnPosition)
    {
        GameObject newItem = null;
        switch (itemType)
        {
            case ItemType.Health:
                newItem = Instantiate(HealthItemPrefab);
                break;
            case ItemType.Stamina:
                newItem = Instantiate(StaminaItemPrefab);
                break;
            case ItemType.Bullet:
                newItem = Instantiate(BulletItemPrefab);
                break;
        }
        newItem.transform.position = spawnPosition.position;
    }
    public void AddItem(ItemType itemType)
    {
        for (int i = 0; i < ItemList.Count; i++)
        {
            if (ItemList[i].ItemType == itemType)
            {
                ItemList[i].Count++;
                break;
            }
        }
    }

    public int GetItemCount(ItemType itemType)
    {
        for (int i = 0; i < ItemList.Count; i++)
        {
            if (ItemList[i].ItemType == itemType)
            {
                return ItemList[i].Count;
            }
        }
        return 0;

    }   

    public bool TryUseItem(ItemType itemType)
    {
        for (int i = 0; i < ItemList.Count; i++)
        {
            if (ItemList[i].ItemType == itemType)
            {
                return ItemList[i].TryUse();
            }
        }
        return false;
    }

    public void RefreshUI()
    {
        HealthItemCountTextUI.text = $"x{GetItemCount(ItemType.Health)}";
        StaminaCountTextUI.text = $"x{GetItemCount(ItemType.Stamina)}";
        BulletCountTextUI.text = $"x{GetItemCount(ItemType.Bullet)}";
    }

}
