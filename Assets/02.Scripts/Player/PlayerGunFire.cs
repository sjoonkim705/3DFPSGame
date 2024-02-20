using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class PlayerGunFire : MonoBehaviour
{

    // 목표 : 마우스 왼쪽 버튼을 누르면 시선이 바라보는 방향으로 총을 발사하고 싶다.
    // 필요 속성
    private GunType _gtype;
    public Gun CurrentGun;
    private int _currentGunIndex;

    private bool _isZoomMode = false;
    private const int DEFAULT_FOV = 60;
    private const int ZOOM_FOV = 20;
    private float _zoomTransition = 0;
    private float duration = 0.3f;
    // - 총알 튀는 이펙트 프리팹
    public ParticleSystem HitEffect;

    private Coroutine _reloadingCoroutine;
    private bool _isReloading;
    private float _shotTimer = 0;
    public Image GunUIImage;
    public GameObject CrossHairUI;
    public GameObject CrossHairZoomMode;

    public List<GameObject> SelectableGuns;

    // 구현 순서
    // 1. 만약에 마우스 왼쪽 버튼을 누르면
    // 2. Ray를 생성하고, 위치와 방향을 설정한다.
    // 3. Ray를 발사한다
    // 4. Ray가 부딛힌 대상의 정보를 받아온다.
    // 5. 부딛힌 위치에 총알이 튀는 이펙트를 생성한다.


    public FPSCamera FpsCamera;
    [SerializeField]
    private Text _magazineUI;
    [SerializeField]
    private Text _reloadingMsg;
    [SerializeField]
    private Slider _reloadingTimeSlider;

    public bool GetZoomMode()
    {
        bool returnValue;
        returnValue = _isZoomMode ? true : false;
        return returnValue;
    }
    void SetGunActive(GunType gunType)
    {
        int index = (int)gunType;
        CurrentGun = SelectableGuns[index].GetComponent<Gun>();
        SelectableGuns[index].SetActive(true);
        for (int i = 0; i < SelectableGuns.Count; i++)
        {
            if (i == index)
            {
                continue;
            }
            else
            {
                SelectableGuns[i].SetActive(false);
            }
        }
    }
    void SetZoomModeUI(bool isZoomMode)
    { 
        CrossHairZoomMode.SetActive(isZoomMode);
        CrossHairUI.SetActive(!isZoomMode);

    }


    private void Start()
    {
        _reloadingTimeSlider.gameObject.SetActive(false);
        _reloadingCoroutine = null;
        _reloadingMsg.text = string.Empty;
        CurrentGun.BulletLeft = CurrentGun.MagazineCapacity;
        FpsCamera = CameraManager.Instance.GetComponent<FPSCamera>();
        SetGunActive(GunType.Rifle);
        RefreshUI();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(2) && CurrentGun.Gtype == GunType.Sniper)
        {
            _isZoomMode = !_isZoomMode;
        }
        else if (CurrentGun.Gtype != GunType.Sniper)
        { 
            _isZoomMode = false;
        }

        if (_isZoomMode)
        {
            SetZoomModeUI(true);
            _zoomTransition -= Time.deltaTime/duration;
            _zoomTransition = Mathf.Clamp(_zoomTransition, 0, 1);
            Camera.main.fieldOfView = Mathf.Lerp(ZOOM_FOV,DEFAULT_FOV, _zoomTransition);
        }
        else
        {
            SetZoomModeUI(false);
            _zoomTransition += Time.deltaTime;
            _zoomTransition = Mathf.Clamp(_zoomTransition, 0, 1);
            Camera.main.fieldOfView = Mathf.Lerp(ZOOM_FOV, DEFAULT_FOV, _zoomTransition);
        }


        if (Input.GetKeyDown(KeyCode.LeftBracket))
        {
            _currentGunIndex--;
            if (_currentGunIndex < 0)
            {
                _currentGunIndex = SelectableGuns.Count - 1;
            }
            SetGunActive((GunType)_currentGunIndex);
            RefreshUI();
        }
        else if (Input.GetKeyDown(KeyCode.RightBracket))
        {
            _currentGunIndex++;
            if (_currentGunIndex > SelectableGuns.Count - 1)
            {
                _currentGunIndex = 0;
            }
            SetGunActive((GunType)_currentGunIndex);
            RefreshUI();
        }


        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SetGunActive(GunType.Rifle);
            RefreshUI();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        { 
            SetGunActive(GunType.Sniper);
            RefreshUI();
        }        
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SetGunActive(GunType.Pistol);
            RefreshUI();
        }

        // 구현 순서
        // 1. 만약에 마우스 왼쪽 버튼을 누르면
        _shotTimer += Time.deltaTime;
        if (Input.GetMouseButton(0) && _shotTimer >= CurrentGun.CoolTime && CurrentGun.BulletLeft > 0)
        {
            if (_reloadingCoroutine != null) // fire중이면 reloading Cancel
            {
                StopCoroutine(_reloadingCoroutine);
                _reloadingMsg.text = string.Empty;
                _isReloading = false;

            }
            Fire();
        }
        if(_isReloading)
        {
            _reloadingTimeSlider.value += Time.deltaTime;
        }
        else
        {
            _reloadingMsg.text = string.Empty;
            _reloadingTimeSlider.gameObject.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.R) && !_isReloading)
        {
            _reloadingTimeSlider.value = 0f;
            _reloadingMsg.text = "Reloading..";
            _reloadingTimeSlider.gameObject.SetActive(true);
            _reloadingCoroutine = StartCoroutine(Reload_Coroutine());
        }
    }
    private void Fire()
    {
        FpsCamera.Shake();

        // 2. Ray를 생성하고, 위치와 방향을 설정한다.
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        // 3. Ray를 발사한다.
        RaycastHit hitInfo;
        bool IsHit = Physics.Raycast(ray, out hitInfo);
        if (IsHit)
        {
            // 실습과제18. 레이저를 몬스터에게 맞출 시 몬스터 체력 닳는 기능 구현
            IHitable hitObject = hitInfo.collider.GetComponent<IHitable>();
            if (hitObject != null)
            {
                hitObject.Hit(CurrentGun.Damage);
            }

            HitEffect.gameObject.transform.position = hitInfo.point;
            HitEffect.gameObject.transform.forward = hitInfo.normal;
            HitEffect.Play();
        }


        // Debug.Log(hitInfo.point);
        // 4. Ray가 부딛힌 대상의 정보를 받아온다.
        // 5. 부딛힌 위치에 총알이 튀는 이펙트를 생성한다. 
        _shotTimer = 0f;
        CurrentGun.BulletLeft--;
        RefreshUI();

    }

    private IEnumerator Reload_Coroutine()
    {
        _reloadingTimeSlider.value = 0f;
        _reloadingTimeSlider.maxValue = CurrentGun.ReloadingTime;
        _reloadingMsg.text = "Reloading...";
        _isReloading = true;
        yield return new WaitForSeconds(CurrentGun.ReloadingTime);
        FillMagazine();
        _isReloading = false;
        _reloadingMsg.text = string.Empty;
    }
    private void FillMagazine()
    {
        CurrentGun.TotalBulletLeft -= CurrentGun.MagazineCapacity - CurrentGun.BulletLeft;
        CurrentGun.BulletLeft = CurrentGun.MagazineCapacity;
        RefreshUI();
    }

    private void RefreshUI()
    {
        GunUIImage.sprite = CurrentGun.ProfileImage;
        _magazineUI.text = $"{CurrentGun.BulletLeft:D2} / {CurrentGun.TotalBulletLeft}";
    }
   
}
