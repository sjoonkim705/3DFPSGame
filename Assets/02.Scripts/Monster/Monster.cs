using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public enum MonsterState        // 몬스터 상태
{
    Idle,
    Trace,
    Attack,
    Comeback,
    Damaged,
    Die
}

public class Monster : MonoBehaviour, IHitable
{
    [Header("Monster FSM Values")]
    private MonsterState _currentState = MonsterState.Idle;
    private Transform _target;
    public float FindDistance = 5f;
    public float AttackDistance = 2f;
    public Vector3 StartPos;
    public float MoveableDistance = 10f;


    public int Health;
    public int MaxHealth = 100;
    public int Damage = 10;
    private CharacterController _charController;
    public float MovingSpeed = 3.0f;
    private float _rotationSpeed = 50f;

    [Header("Monster Attack Components")]
    private float _attackTimer;
    private const float _ATTACK_COOLTIME = 1f;
    private const float KNOCKBACK_DURATION= 0.5f;
    private float _knockbackProgress = 0f;
    public float KnockbackPower = 0.5f;
    public Vector3 _knockbackStartPosition;
    public Vector3 _knockbackEndPosition;


    [Header("Monster Health Slider Components")]
    [SerializeField]
    private Slider _healthSlider;
    [SerializeField]
    private Image _healthSliderFillAreaImage;

    public void Hit(int damage)
    {
        Health -= damage;
        if (Health <= 0)
        {
            DieMonster();
        }
        else
        {
            _currentState = MonsterState.Damaged;
            Debug.Log($"{this.gameObject.name} : Any -> Damaged");
        }
    }
    public void Init()
    {
        Health = MaxHealth;
        StartPos = transform.position;
        StartPos.y = 1;
        
    }
    public void DieMonster()
    {
        ItemObjectFactory.Instance.MakebyProbability(transform.position);
        Destroy(gameObject);
    }
    void Start()
    {
        Init();
        _charController = GetComponent<CharacterController>();
        _target = GameObject.FindGameObjectWithTag("Player").transform;
        _healthSliderFillAreaImage = _healthSliderFillAreaImage.GetComponent<Image>();
    }
    private void Update()
    {
        _healthSlider.value = (float)Health / MaxHealth;
        // 상태패턴 : 상태에 따라 행동을 다르게 하는 패턴
        // 1. 몬스터가 가질 수 있는 행동에 따라 상태를 나눈다.
        // 2. 상태들이 조건에 따라 자연스럽게 전환되게 설계한다.

        switch (_currentState)
        {
            case MonsterState.Idle:
                Idle();
                break;
            case MonsterState.Trace:
                Trace();
                break;
            case MonsterState.Attack:
                Attack();
                break;
            case MonsterState.Comeback:
                Comeback();
                break;
            case MonsterState.Damaged:
                Damaged();
                break;

        }
        SetSliderColor();
    }

    private void Idle()
    {
        // todo: 몬스터의 Idle애니메이션 재생

        if (Vector3.Distance(_target.position, transform.position) <= FindDistance)
        {
            _currentState = MonsterState.Trace;
            Debug.Log($"{gameObject.name} Idle -> Trace");
        }
    }
    private void Trace()
    {
        Vector3 dir = _target.position - transform.position;
        dir.y = 0;
        dir.Normalize();

        float targetAngle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
        float angle = Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetAngle, _rotationSpeed * Time.deltaTime);
        transform.eulerAngles = new Vector3(0, angle, 0);

        _charController.Move(dir * MovingSpeed * Time.deltaTime);

        if (Vector3.Distance(_target.position, transform.position) <= AttackDistance)
        { 
            _currentState = MonsterState.Attack;
            Debug.Log($"{gameObject.name} Trace -> Attack");
            _attackTimer = 0;
        }
        if (Vector3.Distance(transform.position, StartPos) >= MoveableDistance)
        {
            _currentState = MonsterState.Comeback;
            Debug.Log($"{gameObject.name} Trace -> Comeback");
        }
    }
    private void Attack()
    {
        // 전이 사건 : 플레이어와 거리가 공격 범위보다 멀어지면 다시 Trace
        if (Vector3.Distance(_target.position, transform.position) > AttackDistance)
        {
            _currentState = MonsterState.Trace;
            Debug.Log($"{gameObject.name} Attack -> Trace");
        }
        IHitable playerHitable = _target.GetComponent<IHitable>();
        if (playerHitable != null)
        {
            _attackTimer += Time.deltaTime;
            if ( _attackTimer > _ATTACK_COOLTIME)
            {
                playerHitable.Hit(Damage);
                _attackTimer = 0;
            }
        }
    }

    private void Comeback()
    {
        Vector3 dir = StartPos - transform.position;
        dir.y = 0;
        dir.Normalize();

        float targetAngle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
        float angle = Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetAngle, _rotationSpeed * Time.deltaTime);

        transform.eulerAngles = new Vector3(0, angle, 0);

        _charController.Move(dir * MovingSpeed * Time.deltaTime);
        if (Vector3.Distance(StartPos, transform.position) <= 0.1f)
        {
            _currentState = MonsterState.Idle;
            Debug.Log($"{gameObject.name} Comeback -> Idle");
        }
    }
    private void Damaged()
    {
        // 1. Damage animation 실행
        // 2. 넉백(lerp -> 0.5초)
        // 2-1. 넉백 최종 위치를 구한다.
        // 2-2. Lerp를 이용해 넉백
        if (_knockbackProgress == 0)
        {
            _knockbackStartPosition = transform.position;

            Vector3 dir = transform.position - _target.position;
            dir.y = 0;
            dir.Normalize();
            _knockbackEndPosition = transform.position + dir * KnockbackPower;
        }
        _knockbackProgress += Time.deltaTime / KNOCKBACK_DURATION;
        transform.position = Vector3.Lerp(_knockbackStartPosition, _knockbackEndPosition, _knockbackProgress);

        if (_knockbackProgress > 1)
        {
            _knockbackProgress = 0;
            Debug.Log($"{gameObject.name} Damaged -> Trace");
            _currentState = MonsterState.Trace;
        }


    }
    


    private void SetSliderColor()
    {

        if (_healthSlider.value > 0.7)
        {
            _healthSliderFillAreaImage.color = Color.green;

        }
        else if (_healthSlider.value > 0.3)
        {
            _healthSliderFillAreaImage.color = Color.yellow;
        }
        else
        {
            _healthSliderFillAreaImage.color = Color.red;
        }
    }

}
