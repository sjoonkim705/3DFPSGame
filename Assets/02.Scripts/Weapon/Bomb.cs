using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    // 플레이어를 제외하고 물체에 닿으면 자기 자신을 사라지게 하는 코드 작성
    // 목표: 수류탄 폭발 범위 데미지 기능 구현
    // 범위
    public float ExplosionRadius = 3f;

    // 구현 순서
    // 1. 터질 때
    // 2. 범위 안에 있는 모든 콜라이더를 찾는다.

    // 3. 찾은 콜라이더 중에서 IHitable 인터페이스를 가진 오브젝트를 찾는다.
    // hit한다

    private Collider[] _colliders = new Collider[10];

    public GameObject BombEffectPrefab;
    private Rigidbody _bombRigid;
    public int Damage = 60;

    private void Awake()
    { 
        _bombRigid = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        int layer = (LayerMask.GetMask("Monster") | LayerMask.GetMask("Player"));
        int count = Physics.OverlapSphereNonAlloc(transform.position, ExplosionRadius, _colliders, layer);
        // -> Physics.overlap함수는 특정 영역(radius) 안에 있는 모든 게임오브젝트의
        // 콜라이더 컴포넌트들을 모두 찾아 배열로 반환하는 함수
        // 영역의 형태: 구, 큐브, 캡슐
        for (int i=0; i < count;i++)
        {
            Collider c = _colliders[i];
            IHitable hitableObject = c.GetComponent<IHitable>();
            if (hitableObject != null)
            {
                DamageInfo damageInfo = new DamageInfo(DamageType.Normal, Damage);
                hitableObject.Hit(damageInfo);
            }
        }

        if (!collision.collider.CompareTag("Player"))
        {
            GameObject bombEffect = GameObject.Instantiate(BombEffectPrefab);
            bombEffect.transform.position = this.transform.position;
            gameObject.SetActive(false);
        }
    }
}
