using System.Collections;
using System.Collections.Generic;
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
            Destroy(gameObject);
        }
    }
    public void Init()
    {
        Health = MaxHealth;
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
