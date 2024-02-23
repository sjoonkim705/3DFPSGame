using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public enum CameraMode
{
    FPS,
    TPS,
    Top,
    Back,
}

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance { get; private set; }

    public FPSCamera FPSCamera;
    public TPSCamera TPSCamera;
    public CameraShake CameraShake;


    public CameraMode Mode = CameraMode.FPS;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }

        FPSCamera = GetComponent<FPSCamera>();
        TPSCamera = GetComponent<TPSCamera>();


        SetCameraMode(CameraMode.FPS);
    }

    public void SetCameraMode(CameraMode mode)
    {
        Mode = mode;

        FPSCamera.enabled = (mode == CameraMode.FPS);
        TPSCamera.enabled = (mode == CameraMode.TPS);
    }



}
