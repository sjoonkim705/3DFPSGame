using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Linq;

// 역할 : 아이템들을 관리해주는 관리자
// 데이터 관리 - > 데이터를 생성, 수정, 삭제, 조회(검색), 정렬

public class ItemManager : MonoBehaviour 
{
    public static ItemManager Instance { get; private set; }
    public GameObject Player;
    private Action OnDataChanged;
    public void Subscribe(Action action)
    {
      if(!OnDataChanged.GetInvocationList().Contains(action))
        {
            OnDataChanged += action;
        }
    }
    // 관찰자 패턴
    // 구독자가 구독하고 있는 유튜버의 상태가 변화할 때마다
    // 유튜버는 구독자에게 통지하고 이벤트를 통지하고, 구독자들은 이벤트 알림을 받아 적절하게
    // 행동하는 패턴


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
        if (OnDataChanged != null)
        {
            OnDataChanged.Invoke();

        }

    }


    public void AddItem(ItemType itemType)
    {
        for (int i = 0; i < ItemList.Count; i++)
        {
            if (ItemList[i].ItemType == itemType)
            {
                ItemList[i].Count++;
                if (OnDataChanged != null)
                {
                    OnDataChanged.Invoke();

                }
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
                if (OnDataChanged != null)
                {
                    OnDataChanged.Invoke();

                }
                return ItemList[i].TryUse();
            }
        }
        return false;
    }


}
