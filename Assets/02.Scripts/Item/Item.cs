using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Health,
    Stamina,
    Bullet
}
public class Item
{
    public ItemType ItemType;
    public int Count;
    public PlayerMoveAbility PlayerInfo;
    public PlayerGunFireAbility GunInfo;

    public Item(ItemType type, int count)
    {
        ItemType = type;
        Count = count;
    }

    public bool TryUse()
    {
        if (Count == 0)
        {
            return false;
        }
        else
        {
            Count--;
        }

        switch (ItemType)
        {
            case ItemType.Health:
            {
                PlayerInfo = ItemManager.Instance.Player.GetComponent<PlayerMoveAbility>();
                if (PlayerInfo != null)
                {
                    PlayerInfo.Health = PlayerInfo.MaxHealth;
                }
                break; // Todo: 플레이어 체력 꽉차기
            }
            case ItemType.Stamina:
            {
                PlayerInfo = ItemManager.Instance.Player.GetComponent<PlayerMoveAbility>();
                if (PlayerInfo != null)
                {
                    PlayerInfo.Stamina = PlayerMoveAbility.MaxStamina;
                }
                break;
            }
            case ItemType.Bullet:
            {
                GunInfo = ItemManager.Instance.Player.GetComponent<PlayerGunFireAbility>();
                GunInfo.CurrentGun.TotalBulletLeft += 30;
                GunInfo.RefreshUI();

                break;
            }
            default:
                break;


        }
        return true;
    }
}
