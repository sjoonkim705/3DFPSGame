using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_ItemInventory : MonoBehaviour
{
    public TextMeshProUGUI HealthItemCountTextUI;
    public TextMeshProUGUI StaminaCountTextUI;
    public TextMeshProUGUI BulletCountTextUI;

    public void Start()
    {
        Refresh();
       // ItemManager.Instance.Subscribe(Refresh);

    }

    public void Refresh()
    {
        HealthItemCountTextUI.text = $"x{ItemManager.Instance.GetItemCount(ItemType.Health)}";
        StaminaCountTextUI.text = $"x{ItemManager.Instance.GetItemCount(ItemType.Stamina)}";
        BulletCountTextUI.text = $"x{ItemManager.Instance.GetItemCount(ItemType.Bullet)}";
    }

}
