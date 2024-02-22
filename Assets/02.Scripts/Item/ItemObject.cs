using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
public enum ItemState
{
    Stand,
    Draw,
    Taken
}
public class ItemObject : MonoBehaviour
{
    private ItemState _currentstate = ItemState.Stand;
    public ItemType ItemType;

    public float _drawSpeed = 5.0f;
    private float _drawProgress = 0;
    private const float DRAW_DURATION = 0.5f;

    private Vector3 _drawStartPosition;
    private Vector3 _drawEndPosition;

    // Todo 1. 아이템 프리팹을 3개(Health, Stamina, Bullet) 만든다. (도형이나 색깔 다르게해서 구별되게)
    // Todo 2. 플레이어와 일정 거리가 되면 아이템이 먹어지고 사라진다.
    // 실습 과제31. 몬스터가 죽으면 아이템이 드랍 (Health:20%, Stamina: 20, bullet 10%)
    // 실습 과제32. 일정 거리가 되면 아이템이 베지어 곡선으로 날아온다.

    public float pickupDistance = 10f;


    private void Stand()
    {
        Vector3 playerPosition = ItemManager.Instance.Player.transform.position;
        float distanceToPlayer = Vector3.Distance(transform.position, playerPosition);
        if (distanceToPlayer <= pickupDistance)
        {
            Debug.Log(distanceToPlayer);
            Debug.Log("Stand -> Draw");
            _currentstate = ItemState.Draw;
        }
    }
    private void Draw()
    {
        // Vector3 dir = ItemManager.Instance.Player.transform.position - transform.position;
        if (_drawProgress == 0)
        {
            _drawStartPosition = transform.position;
        }
            _drawEndPosition = ItemManager.Instance.Player.transform.position;

        _drawProgress += Time.deltaTime / DRAW_DURATION;
        transform.position = Vector3.Slerp(_drawStartPosition, _drawEndPosition, _drawProgress);
        if (_drawProgress > 1)
        {
            _drawProgress = 0;
        }


        // transform.position += dir * drawSpeed * Time.deltaTime;


    }

    void Update()
    {
        switch (_currentstate)
        {
            case ItemState.Stand:
                Stand();
                break;
            case ItemState.Draw:
                Draw();
                break;
            case ItemState.Taken:
                //Take();
                break;

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ItemManager.Instance.AddItem(ItemType);
            gameObject.SetActive(false);
        }
    }

    private void DrawItem()
    {
     //   Debug.Log("EatItem");
    }

}
