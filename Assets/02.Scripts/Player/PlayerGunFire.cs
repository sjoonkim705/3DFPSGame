using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class PlayerGunFire : MonoBehaviour
{

    // 목표 : 마우스 왼쪽 버튼을 누르면 시선이 바라보는 방향으로 총을 발사하고 싶다.
    // 필요 속성
    private GunType _gtype;
    public Gun CurrentGun;

    // - 총알 튀는 이펙트 프리팹
    public ParticleSystem HitEffect;

    private Coroutine _reloadingCoroutine;
    private bool _isReloading;
    private float _shotTimer = 0;

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

    void SetGunActive(GunType gunType)
    {
        int index = (int)gunType;

        CurrentGun = SelectableGuns[index].GetComponent<Gun>();
        SelectableGuns[index].SetActive(true);
        for (int i=0;i< SelectableGuns.Count;i++)
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

    private void Start()
    {
        _reloadingTimeSlider.gameObject.SetActive(false);
        _reloadingCoroutine = null;
        _reloadingMsg.text = string.Empty;
        CurrentGun.BulletLeft = CurrentGun.MagazineCapacity;
        FpsCamera = CameraManager.Instance.GetComponent<FPSCamera>();
        SetGunActive(GunType.Rifle);
        //RefreshGun();
        RefreshTextUI();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SetGunActive(GunType.Rifle);
            RefreshTextUI();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        { 
            SetGunActive(GunType.Sniper);
            RefreshTextUI();
        }        
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SetGunActive(GunType.Pistol);
            RefreshTextUI();
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
        RefreshTextUI();

    }

    private IEnumerator Reload_Coroutine()
    {
        _reloadingTimeSlider.value = 0f;
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
        RefreshTextUI();
    }

    private void RefreshTextUI()
    {
        _magazineUI.text = $"Bullet {CurrentGun.BulletLeft:D2} / {CurrentGun.TotalBulletLeft}";
    }
   
}
