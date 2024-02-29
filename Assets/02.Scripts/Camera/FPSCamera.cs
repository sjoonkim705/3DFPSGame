using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.FilePathAttribute;

// 1인칭 슈팅(First Person Shooter)
public class FPSCamera : MonoBehaviour
{

    public static FPSCamera Instance;
    public GameObject GameCamera;
    private void Awake()
    {
        Instance = this; 
    }

    // ** 카메라 회전 **
    // 목표: 마우스를 조작하면 카메라를 그 방향으로 회전시키고 싶다.
    // 필요 속성:
    // - 회전 속도
    public float RotationSpeed = 200; // 초당 200도까지 회전 가능한 속도
    // 누적할 x각도와 y각도
    private float _mx = 0;
    private float _my = 0;



    /** 카메라 이동 **/
    // 목표: 카메라를 캐릭터의 눈으로 이동시키고 싶다.
    // 필요 속성:
    // - 캐릭터의 눈 위치
    public Transform Target;
    // 구현 순서:
    // 1. 캐릭터의 눈 위치로 카메라를 이동시킨다.


    private void Start()
    {
        // 마우스 커서 없애고, 고정
        // Cursor.visible = false;
        // Cursor.lockState = CursorLockMode.Locked;
    }

    // 순서:
    // 1. 마우스 입력(drag) 받는다.
    // 2. 마우스 입력 값을 이용해 회전 방향을 구한다.
    // 3. 회전 방향 회전한다.

    private void LateUpdate()
    {
        if (Gamemanager.Instance.State != GameState.Go)
        {
            return;
        }

        if (CameraManager.Instance.Mode == CameraMode.FPS)
        {
            // 1. 캐릭터의 눈 위치로 카메라를 이동시킨다.
            transform.localPosition = Target.position;
        }
        // 1. 마우스 입력(drag) 받는다.
        
        float mouseX = Input.GetAxis("Mouse X"); // 방향에 따라 -1 ~ 1 사이의 값 반환 
        float mouseY = Input.GetAxis("Mouse Y");
        //Debug.Log($"GetAxis: {mouseX},{mouseY}");
        //Vector2 mousePosition = Input.mousePosition; // 진짜 마우스 좌표값
        //Debug.Log($"mousePosition: {mousePosition.x}, {mousePosition.y}");

        // 2. 마우스 입력 값을 이용해 회전 방향을 구한다.
        Vector3 rotationDir = new Vector3(mouseX, mouseY, 0);
        // rotationDir.Normalize(); // 정규화

        // 3. 회전 방향 회전한다.
        // 새로운 위치 = 이전 위치 + 방향 * 속도 * 시간
        // 새로운 회전 = 이전 회전 + 방향 * 속도 * 시간
        // 3-1 회전 방향에 따라 마우스 입력 값 만큼 미리 누적시킨다.

            _mx += rotationDir.x * RotationSpeed * Time.deltaTime;
            _my += rotationDir.y * RotationSpeed * Time.deltaTime;
        

        // 4. 시선의 상하 제한을 -90 ~ 90로 사이로 제한하고 싶다.
        _my = Mathf.Clamp(_my, -90f, 90f);
        //_mx = Mathf.Clamp(_mx, -270f, 270f);

        if (CameraManager.Instance.Mode == CameraMode.FPS)
        {
            transform.eulerAngles = new Vector3(-_my, _mx, 0);
        }

        // 오일러 각도의 단점
        // 1. 짐벌락 현상
        // 2. 0보다 작아지면 -1이 아닌 359(360-1)가 된다. (유니티 내부에서 이렇게 자동 연산)
        // 위 2번 문제 해결을 위해서 우리가 미리 연산을 해줘야 한다.
    }

}
