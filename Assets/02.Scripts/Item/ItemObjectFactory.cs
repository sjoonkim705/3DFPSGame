using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// 아이템 공장의 역할: 아이템 오브젝트의 생성을 책임진다.
// 팩토리 패턴
// 객체 생성을 공장 클래스를 이용해 캡슐화 처리하여 대신 "생성"하게 하는 대신
// 객체 생성에 필요한 과정을 템플릿화 해놓고 외부에서 쉽게 사용한다.
// 장점:
// 1. 생성과 처리 로직을 분리하여 결합도를 낮출 수 있다.
// 2. 확장 및 유지보수가 편리하다.
// 3. 객체 생성 후 공통으로 할 일을 수행하도록 지정해 줄 수 있다.

// 단점:
// 1. 상대적으로 조금 복잡
// 2. 학습 필요

public class ItemObjectFactory : MonoBehaviour
{
    public static ItemObjectFactory Instance {  get; private set; }
    public List <GameObject> ItemPrefabs;

    private List <ItemObject> _itemPool;
    private int _poolSize = 10;


    private void Awake()
    {
        Instance = this;
        _itemPool = new List <ItemObject>();
    }

    private void PreparePool(int poolsize)
    {
        for (int i = 0; i < poolsize; i++)
        {
            GameObject item = null;
            foreach (GameObject prefab in ItemPrefabs)
            {
                item = Instantiate(prefab);
                item.transform.SetParent(this.transform);
                _itemPool.Add(item.GetComponent<ItemObject>());
                item.SetActive(false);
            }
        }
    }
    private void Start()
    {
        PreparePool(_poolSize); // poolsize 10인 풀 생성
    }
    private ItemObject Get(ItemType itemType)
    {
        foreach (ItemObject itemObject in _itemPool)
        {
            if(itemObject.gameObject.activeSelf == false && itemObject.ItemType == itemType)
            {
                return itemObject;
            }
        }
        return null;
    }
    public void MakePercent(Vector3 position)
    {
        int itemRandomFactor = Random.Range(0, 100);
        Debug.Log(itemRandomFactor);
        if (itemRandomFactor <= 20)
        {
            Make(ItemType.Health, position);
        }
        else if (itemRandomFactor <= 40)
        {
            Make(ItemType.Stamina, position);
        }
        else if (itemRandomFactor <= 50)
        {
            Make(ItemType.Bullet, position);
        }
    }
    public void Make(ItemType itemType, Vector3 position)
    {
       // GameObject newItem = null;
        ItemObject itemObject = Get(itemType);   
        if (itemObject != null)
        {
            itemObject.gameObject.SetActive(true);
            itemObject.Init();
            itemObject.transform.position = position;
        }
        
    }
}
