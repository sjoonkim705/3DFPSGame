using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class Monster : MonoBehaviour, IHitable
{

    public int Health;
    public int MaxHealth = 100;

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
    }
    public void Init()
    {
        Health = MaxHealth;
    }
    public void DieMonster()
    {
        int spawnItemRandomFactor = Random.Range(0, 10);
        if (spawnItemRandomFactor >= 8)
        {
            ItemManager.Instance.SpawnItem(ItemType.Health, transform);
;       }
        else if (spawnItemRandomFactor >= 6)
        {
            ItemManager.Instance.SpawnItem(ItemType.Stamina, transform);
        }
        else if (spawnItemRandomFactor >= 5)
        {
            ItemManager.Instance.SpawnItem(ItemType.Bullet, transform);
        }

        Destroy(gameObject);
    }
    void Start()
    {
        Init();
        _healthSliderFillAreaImage = _healthSliderFillAreaImage.GetComponent<Image>();
    }
    private void Update()
    {
        _healthSlider.value = (float)Health / MaxHealth;
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
