using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class StaminaSlider : MonoBehaviour
{
    public float stamina;
    public float MaxStamina;
    public GameObject Player;
    private PlayerMove _playerMove;
    private Slider mySlider;

    void Start()
    {
        _playerMove = Player.GetComponent<PlayerMove>();
        mySlider = GetComponent<Slider>();
        stamina = _playerMove.Stamina;
        MaxStamina = PlayerMove.MAX_STAMINA;
        

    }

    void Update()
    {
        stamina = _playerMove.Stamina;
        mySlider.value = stamina/MaxStamina;
        Debug.Log(mySlider.value);
    }
}
