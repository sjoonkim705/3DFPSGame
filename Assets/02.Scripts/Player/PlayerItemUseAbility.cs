using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemUseAbility : MonoBehaviour
{
    private CharacterController _playerController;
    void Start()
    {
        _playerController = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T)) // HealthItem
        {
            if(ItemManager.Instance.TryUseItem(ItemType.Health))
            {
                Debug.Log(ItemManager.Instance.GetItemCount(ItemType.Health));
                // ItemManager.Instance.OnDataChanged();
            }

        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            if(ItemManager.Instance.TryUseItem(ItemType.Stamina))
            {
                //ItemManager.Instance.RefreshUI();
            }
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            if (ItemManager.Instance.TryUseItem(ItemType.Bullet))
            {
               // ItemManager.Instance.RefreshUI();
               
            }
        }

    }
}
