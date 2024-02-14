using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPSCamera : MonoBehaviour
{
    // ** 카메라 회전 **
    // 목표: 마우스를 조작하면 카메라를 캐릭터 중심에 따라 그 방향으로 회전시키고 싶다.

    // 필요 속성:
    // - 회전 속도
    public float RotationSpeed = 200;
    // - 타겟(캐릭터)
    public Transform Target;
    public Vector3 Offset = new Vector3(0, 3f, -3f);
    // - 누적할 x각도와 y각도
    private float _mx = 0;
    private float _my = 0;

    private void LateUpdate()
    {
        if (CameraManager.Instance.Mode == CameraMode.TPS)
        {
            // 구현 순서:
            // 1. 카메라를 타겟(플레이어에서 조금 떨어진 거리)으로 이동시킨다.(따라다니게 한다.)
            transform.position = Target.position + Offset;

            // 2. 플레이어를 쳐다보게 한다.
            // LookAt: Rotates the transform so the forward vector points at target's current position
            transform.LookAt(Target);
        }

        // 3. 마우스 입력을 받는다.
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // 4. 마우스 입력에 따라 회전 방향을 누적한다.
        _mx += mouseX * RotationSpeed * Time.deltaTime;
        _my += mouseY * RotationSpeed * Time.deltaTime;
        //_my = Mathf.Clamp(_my, -20f, 10f);

        if (CameraManager.Instance.Mode == CameraMode.TPS)
        {
            // 5. 타겟 중심으로 회전 방향에 맞게 회전한다.
            transform.RotateAround(Target.position, Vector3.up, _mx);
            transform.RotateAround(Target.position, transform.right, -_my);
        }


        //transform.position = Target.position - transform.forward * Offset.magnitude + Vector3.up * Offset.y;
    }
}
