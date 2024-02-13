using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.FilePathAttribute;

// 1인칭 슈팅(First Person Shooter)
public class FPSCamera : MonoBehaviour
{
    // 마우스를 조작하면 카메라를 그 방향을 회전시키고 싶다.
    // 필요 속성 : 최전 속도
    public float RotationSpeed = 200; // 초당 200도까지 회전
    private float _mx = 0;
    private float _my = 0;
    // 순서
    // 1. 마우스 입력을 받는다.
    // 2. 마우스 입력값을 이용해 회전방향을 구한다.
    // 3. 회전 방향으로 회전한다.
    public Transform target;

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void Update()
    {
        transform.position = target.position;
        // 1. 마우스 입력을 받는다.
        float mouseX = Input.GetAxis("Mouse X"); // 방향에 따라 -1 ~ 1 사이의 값 반환
        float mouseY = Input.GetAxis("Mouse Y");
        //Vector2 mousePos = Input.mousePosition;

        // Debug.Log($"{mouseX} {mouseY}");
        //Debug.Log($"mousePosition: {mousePos.x}:{mousePos.y}");

        // 2. 마우스 입력값을 이용해 회전방향을 구한다.
        Vector3 rotationDir = new Vector3(mouseX, mouseY, 0);
        // rotationDir.Normalize();


        // 3. 회전 방향으로 회전한다.
        // 새로운 위치 = 이전 위치 + 방향 * 속도 * 시간
        // 새로운 회전 = 이전 회전 + 방향 * 속도 * 시간

        //transform.eulerAngles += rotationDir * RotationSpeed * Time.deltaTime;

/*        Vector3 rotation = transform.eulerAngles;
        rotation.x = Mathf.Clamp(rotation.x, -90f, 90f);
        // rotation.y = Mathf.Clamp(rotation.y, -200f, 200f);
        transform.eulerAngles = rotation;*/

        // 오일러 각도의 단점
        // 1. 짐벌락현상
        // 2. 0보다 작아지면 -1이 아닌 359도가 된다. (유니티 내부에서 자동 연산)
        // 위 문제 해결을 위해서 우리가 미리 연산을 해줘야 한다.

        // 3-1  회전 방향에 따라 마우스 입력값 만큼 누적시킨다.
        _mx += rotationDir.x * RotationSpeed * Time.deltaTime;
        _my += rotationDir.y * RotationSpeed * Time.deltaTime;

        // 4. 시선의 상하제한
        _my = Mathf.Clamp(_my, -90f, 90f);
        // _mx = Mathf.Clamp(_mx, -270f, 270f);
        // _my = Mathf.Clamp(_mx, -200f, 200f);
        transform.eulerAngles = new Vector3(_my, _mx, 0);

        // 카메라 이동
        // 목표: 카메라를 캐릭터의 눈으로 이동시키고 싶다.

        // 필요 속성
        // 캐릭터의 눈 위치



        // 구현 순서
        // 1. 캐릭터의 눈 위치로 카메라를 이동시킨다.


    }
}
