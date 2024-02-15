using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotate : MonoBehaviour
{
    // 목표: 마우스를 조작하면 플레이어를 좌우방향으로 회전 시키고 싶다.
    // 필요 속성:
    // - 회전 속도
    public float RotationSpeed = 200; // 초당 200도까지 회전 가능한 속도
    // 누적할 x각도
    private float _mx = 0;

    void Update()
    {
        // 1. 마우스 입력(drag) 받는다.
        float mouseX = Input.GetAxis("Mouse X");

        // 2. 마우스 입력 값만큼 x값을 누적한다.
        _mx += mouseX * RotationSpeed * Time.deltaTime;
        //_mx = Mathf.Clamp(_mx, -270f, 270f);

        // 3. 누적한 값에 따라 회전한다.
        transform.eulerAngles = new Vector3(0f, _mx, 0);
    }
}
