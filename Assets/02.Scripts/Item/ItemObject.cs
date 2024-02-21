using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour
{

    public ItemType ItemType;

    // Todo 1. 아이템 프리팹을 3개(Health, Stamina, Bullet) 만든다. (도형이나 색깔 다르게해서 구별되게)
    // Todo 2. 플레이어와 일정 거리가 되면 아이템이 먹어지고 사라진다.
    // 실습 과제31. 몬스터가 죽으면 아이템이 드랍 (Health:20%, Stamina: 20, bullet 10%)
    // 실습 과제32. 일정 거리가 되면 아이템이 베지어 곡선으로 날아온다.
    
        public float pickupDistance = 2f; // 플레이어와 아이템 간의 허용 거리

    void Update()
    {
        Vector3 playerPosition = ItemManager.Instance.Player.transform.position;
        float distanceToPlayer = Vector3.Distance(transform.position, playerPosition);
        if (distanceToPlayer <= pickupDistance)
        {
            DrawItem();
        }
    }


    private void DrawItem()
    {
        Debug.Log("EatItem");
    }

}
