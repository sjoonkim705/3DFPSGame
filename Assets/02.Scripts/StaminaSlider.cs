using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class StaminaSlider : MonoBehaviour
{
    public float Stamina;
    public float MaxStamina;
    public GameObject Player;
    private PlayerMove _playerMove;

    [Header ("Slider")]
    private Slider StaminaSliderUI;

    void Start()
    {
        _playerMove = Player.GetComponent<PlayerMove>();
        StaminaSliderUI = GetComponent<Slider>();
        Stamina = _playerMove.Stamina;
        MaxStamina = PlayerMove.MaxStamina;
        

    }

    void Update()
    {
        Stamina = _playerMove.Stamina;
        StaminaSliderUI.value = Stamina/MaxStamina;
        //Debug.Log($"{});
    }
}
