using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum GunType
{
    Rifle = 0,   //
    Sniper = 1,
    Pistol = 2,

}

public class Gun : MonoBehaviour
{
    public GunType Gtype;
    // 공격력
    public int Damage = 1;

    // 발사 쿨타임
    public float CoolTime;


    // 총알 갯수
    public int MagazineCapacity = 30;
    public int BulletLeft;
    public int TotalBulletLeft = 150;

    // 재장전 시간
    public float ReloadingTime = 1.5f;

    private void Start()
    {
/*        switch(Gtype)
        {
            case GunType.Rifle:
                Damage = 10;
                CoolTime = 0.3f;
                TotalBulletLeft = 150;
                MagazineCapacity = 30;
                ReloadingTime = 1.5f;
                break;
            case GunType.Sniper:
                Damage = 30;
                CoolTime = 0.8f;
                TotalBulletLeft = 150;
                MagazineCapacity = 30;
                ReloadingTime = 1.5f;
                break;
            case GunType.Pistol:
                break;


        }*/
        BulletLeft = MagazineCapacity;

    }
}
