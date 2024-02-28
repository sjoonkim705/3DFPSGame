using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMoveAbility : MonoBehaviour , IHitable
{
    // 목표: 키보드 방향키(wasd)를 누르면 캐릭터를 바라보는 방향 기준으로 이동시키고 싶다. 
    // 속성:
    // - 이동속도
    public float MoveSpeed = 5;     // 일반 속도
    public float RunSpeed = 10;    // 뛰는 속도
    [Range(0,100)]
    public float Stamina = 100;             // 스태미나
    public const float MaxStamina = 100;    // 스태미나 최대량
    public float StaminaConsumeSpeed = 33f; // 초당 스태미나 소모량
    public float StaminaChargeSpeed = 50f;  // 초당 스태미나 충전량
    public float ClimbingWallConsumeSpeed = 50f;
    private bool _isJumping = false;
    private int JumpMaxCount = 2;
    private int JumpRemainCount;
    private bool _isFalling = false;
    private bool _isRunning = false;
    public Image HitEffectImageUI;
    private Vector3 _dir;

    [Header("스태미나 슬라이더 UI")]
    public Slider StaminaSliderUI;
    private CharacterController _characterController;

    private Animator _animator;



    // 목표 : 스페이스바를 누르면 캐릭터르 점프하고 싶다.
    // 필요 속성:
    // - 점프 파워 값
    public float JumpPower = 3f;

    // 점프 구현
    // 1. 만약에 [SpaceBar]를 누르면..
    // 2. 플레이어에게 y축에 있어 점프 파워를 적용한다.
    // 목표 : 캐릭터에 중력을 적용하고 싶다.

    // 필요 속성:
    // 중력 값
    private float _gravity = -20;
    // 누적할 중력 변수
    private float _yVelocity = 0f;
    // 구현 순서
    // 1. 중력 가속도가 누적된다.
    // 2. 플레이어에게 y축에 있어 중력을 적용한다.

    // 목표: 벽에 닿아 있는 상태에서 스페이스바를 누르면 벽타기를 하고 싶다.
    // 필요 속성:
    public float ClimbingPower;
    public bool _isClimbing = false;
    // 구현 순서
    [Header("플레이어 체력 슬라이더 UI")]
    public int Health;
    public int MaxHealth = 100;
    [SerializeField]
    private Slider _healthSlider;
    [SerializeField]
    private Image _healthSliderFillArea;


    public void Hit(int damage)
    {
        Health -= damage;
        StartCoroutine(HitEffect_Coroutine(0.2f));
        CameraManager.Instance.CameraShake.Shake(0.025f, 0.2f);
        _animator.SetLayerWeight(1, 1 - Health/(float)MaxHealth);


        if(Health <0)
        {
            Gamemanager.Instance.GameOver();
        }

 
    }


    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponentInChildren<Animator>();

    }
    private void Start()
    {
        Health = MaxHealth;
        Stamina = MaxStamina;
        _characterController.minMoveDistance = 0;
        _healthSliderFillArea = _healthSliderFillArea.GetComponent<Image>();

    }

    // 구현 순서
    // 1. 키 입력 받기
    // 2. '캐릭터가 바라보는 방향'을 기준으로 방향구하기
    // 3. 이동하기
    void ClimbWall()
    {
        _yVelocity = 0f;
        _yVelocity = ClimbingPower;
        Stamina -= ClimbingWallConsumeSpeed * Time.deltaTime;

    }
    void Update()
    {
        if (Gamemanager.Instance.State != GameState.Go)
        {
            return;
        }
        _healthSlider.value = (float)Health / MaxHealth;
        float sliderValue = _healthSlider.value;
        if (sliderValue > 0.7)
        {
            _healthSliderFillArea.color = Color.green;
        }
        else if (sliderValue > 0.3)
        {
            _healthSliderFillArea.color = Color.yellow;
        }
        else
        {
            _healthSliderFillArea.color = Color.red;
        }

   

        if (Input.GetKey(KeyCode.Space) && (Stamina > 0) && (_characterController.collisionFlags == CollisionFlags.Sides) && !_isFalling)
        {
            _isClimbing = true;
        }
        else
        {
            _isClimbing = false;
        }

        if (_isClimbing)
        {
            ClimbWall();

        }

        // 3. 벽을 타겠다.
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

            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");
            _dir = new Vector3(h, 0, v);             // 로컬 좌표꼐 (나만의 동서남북) 
            float unNormalizedDir = _dir.magnitude;
            _dir.Normalize();
            _dir = Camera.main.transform.TransformDirection(_dir); // 글로벌 좌표계 (세상의 동서남북)
        
        // 1. 키 입력 받기

        // 2. '캐릭터가 바라보는 방향'을 기준으로 방향구하기

        // 실습 과제 1. Shift 누르고 있으면 빨리 뛰기
        float speed = MoveSpeed; // 5

        if (Input.GetKey(KeyCode.LeftShift))
        {
            // - Shift 누른 동안에는 스태미나가 서서히 소모된다. (3초)
            Stamina -= StaminaConsumeSpeed * Time.deltaTime; // 초당 33씩 소모
            if (Stamina > 0)
            {
                speed = RunSpeed;
            }
            _isRunning = true;
        }
        else
        {
            _isRunning = false;
        }

        if (_characterController.isGrounded && !_isRunning)
        {
            // - 아니면 스태미나가 소모 되는 속도보다 빠른 속도로 충전된다 (2초)
            Stamina += StaminaChargeSpeed * Time.deltaTime; // 초당 50씩 충전
        }

        Stamina = Mathf.Clamp(Stamina, 0, 100);
        StaminaSliderUI.value = Stamina / MaxStamina;  

        if (_characterController.isGrounded)
        {
            if (_yVelocity < -30)
            {
                Hit(10 * (int)(_yVelocity / -10f));
            }
            JumpRemainCount = JumpMaxCount;
            _isJumping = false;
            _isFalling = false;
            _yVelocity = 0;
        }

        // Stamina -= StaminaConsumeSpeed * (_isClimbing ? 1.5f : 1) * Time.deltaTime;



        if (!_characterController.isGrounded && !_isJumping)
        {
            _isFalling = true;
        }

        if (Input.GetKeyDown(KeyCode.Space) && (JumpRemainCount > 0) && _isFalling == false)
            {
                _yVelocity = JumpPower;
                _isJumping = true;
                JumpRemainCount--;
            }

        // 3-1. 중력 가속도 계산
        // 1. 중력 가속도가 누적된다.

        _yVelocity += _gravity * Time.deltaTime;

        // 2. 플레이어에게 y축에 있어 중력을 적용한다.
        _dir.y = _yVelocity;

        // 3. 이동하기
        // transform.position += speed * dir * Time.deltaTime;
        _characterController.Move(_dir * speed * Time.deltaTime);
        _animator.SetFloat("Move", unNormalizedDir);
    }
    private IEnumerator HitEffect_Coroutine(float delay)
    {
        HitEffectImageUI.gameObject.SetActive(true);
        yield return new WaitForSeconds(delay);
        HitEffectImageUI.gameObject.SetActive(false);
    }

}
