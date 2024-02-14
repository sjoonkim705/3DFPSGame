using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour
{
    // 목표: 키보드 방향키(wasd)를 누르면 캐릭터를 바라보는 방향 기준으로 이동시키고 싶다. 
    // 속성:
    // - 이동속도
    public float MoveSpeed = 5;     // 일반 속도
    public float RunSpeed = 10;    // 뛰는 속도

    public float Stamina = 100;             // 스태미나
    public const float MaxStamina = 100;    // 스태미나 최대량
    public float StaminaConsumeSpeed = 33f; // 초당 스태미나 소모량
    public float StaminaChargeSpeed = 50;  // 초당 스태미나 충전량


    [Header("스태미나 슬라이더 UI")]
    public Slider StaminaSliderUI;

    private CharacterController _characterController;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();

    }
    private void Start()
    {
        Stamina = MaxStamina;
    }

    // 구현 순서
    // 1. 키 입력 받기
    // 2. '캐릭터가 바라보는 방향'을 기준으로 방향구하기
    // 3. 이동하기

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            // FPS 카메라 모드로 전환
            CameraManager.Instance.SetCameraMode(CameraMode.FPS);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            // TPS 카메라 모드로 전환
            CameraManager.Instance.SetCameraMode(CameraMode.TPS);
        }


        // 1. 키 입력 받기
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        // 2. '캐릭터가 바라보는 방향'을 기준으로 방향구하기
        Vector3 dir = new Vector3(h, 0, v);             // 로컬 좌표꼐 (나만의 동서남북) 
        dir.Normalize();
        // Transforms direction from local space to world space.
        dir = Camera.main.transform.TransformDirection(dir); // 글로벌 좌표계 (세상의 동서남북)

        // 실습 과제 1. Shift 누르고 있으면 빨리 뛰기
        float speed = MoveSpeed; // 5
        if (Input.GetKey(KeyCode.LeftShift)) // 실습 과제 2. 스태미너 구현
        {
            // - Shfit 누른 동안에는 스태미나가 서서히 소모된다. (3초)
            Stamina -= StaminaConsumeSpeed * Time.deltaTime; // 초당 33씩 소모
            if (Stamina > 0)
            {
                speed = RunSpeed;
            }
        }
        else
        {
            // - 아니면 스태미나가 소모 되는 속도보다 빠른 속도로 충전된다 (2초)
            Stamina += StaminaChargeSpeed * Time.deltaTime; // 초당 50씩 충전
        }

        Stamina = Mathf.Clamp(Stamina, 0, 100);
        StaminaSliderUI.value = Stamina / MaxStamina;  // 0 ~ 1;//

        // 3. 이동하기
        // transform.position += speed * dir * Time.deltaTime;
        _characterController.Move(dir * speed * Time.deltaTime);

    }
}
