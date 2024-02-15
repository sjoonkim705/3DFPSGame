using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCamera : MonoBehaviour
{
    public Transform Target;
    public float Ydistance = 20;
    private Vector3 _initialEulerAngles;

    private void Start()
    {
        _initialEulerAngles = transform.eulerAngles;
    }
    void LateUpdate()
    {
        Vector3 targetPosition = Target.position;

        transform.position = targetPosition;
        targetPosition.y = Ydistance;
        transform.position = targetPosition;


        Vector3 targetEulerAngles = Target.eulerAngles;
        targetEulerAngles.x = _initialEulerAngles.x;
        targetEulerAngles.z = _initialEulerAngles.z;
        transform.eulerAngles = targetEulerAngles;

/*        Quaternion targetRotation = Target.rotation;
        Quaternion fixedRotation = Quaternion.Euler(90, targetRotation.eulerAngles.y, 0f);
        //targetRotation.x = 90;
        //targetRotation.z = 0;
        transform.rotation = fixedRotation;*/


    }
}
