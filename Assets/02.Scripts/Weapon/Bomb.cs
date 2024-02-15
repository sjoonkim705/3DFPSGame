using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    // 플레이어를 제외하고 물체에 닿으면 자기 자신을 사라지게 하는 코드 작성
    public GameObject BombEffectPrefab;

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.collider.CompareTag("Player"))
        {
            GameObject bombEffect = GameObject.Instantiate(BombEffectPrefab);
            bombEffect.transform.position = this.transform.position;
            Destroy(gameObject);
        }
    }
}
