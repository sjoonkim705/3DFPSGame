using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotate : MonoBehaviour
{
    public float RotationSpeed = 200;
    private float _mx = 0;

    // 순서
    // 1. 마우스 입력(drag) 받는다.
    // 2. 마우스 입력 값만큼 
    void Start()
    {
           
    }
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        _mx += mouseX * RotationSpeed * Time.deltaTime;
        transform.eulerAngles= new Vector3(0f, _mx, 0);



    }
}
