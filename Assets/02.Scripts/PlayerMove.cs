using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    // 목표 : (키보드 방향키에 따라 캐릭터를 바라보는 방향 기준으로 이동시키고 싶다.
    // 속성
    // - 이동속도
    // 1. 키 입력
    // 2. '캐릭터가 바라보는 방향'을 기준으로 방향구하기
    // 3. 이동하기 
    public float MoveSpeed = 5f;
    public float Stamina;
    public const float MAX_STAMINA = 10f;

    private void Start()
    {
        Stamina = MAX_STAMINA;
    }
    private void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");


        Vector3 dir = new Vector3(h, 0, v); // 글로벌 좌표계 (세상의 동서남북)
        //dir.Normalize();
        dir = Camera.main.transform.TransformDirection(dir);
        transform.position += MoveSpeed * dir * Time.deltaTime;
    }
    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.LeftShift) && Stamina > 0)
        {
            MoveSpeed = 10f;
            Stamina -= 3f * Time.fixedDeltaTime;
        }
        else if (Stamina < 10f)
        {
            Stamina += 7f * Time.fixedDeltaTime;
            MoveSpeed = 5f;
        }
        else
        {
            MoveSpeed = 5f;
        }

        //Debug.Log(Stamina);
    }
}   
