using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBombFireAbility : MonoBehaviour
{        // 목표 : 마우스 오른쪽 버튼을 누르면 시선이 바라보는 방향으로 수류탄을 던지고 싶다.
         // 필요 속성
         // - 수류탄 프리팹
         // - 수류탄 던지는 위치
         // 구현 순서:
         // 1. 마우스 오른쪽 버튼을 감지
         // 2. 수류탄 던지는 위치에다가 수류탄 생성
         // 3. 시선이 바라보는 방향으로 수류탄 투척
    public GameObject BombPrefeb;
    public Transform FirePosition;
    public float ThrowPower = 30;
    public int BombLeft;
    public int MaxBombNumber = 3;
    public PlayerGunFireAbility PlayerGunInfo;

    public List<GameObject> BombPool;

    [SerializeField]
    private Text _bombLeftUI;

    void Start()
    {

        BombLeft = MaxBombNumber;
        BombPool = new List<GameObject>();
        for (int i = 0;  i<MaxBombNumber ;i++ )
        {
            GameObject bomb = GameObject.Instantiate(BombPrefeb);
            BombPool.Add(bomb);
            bomb.SetActive(false);

        }
        RefreshUI();
    }
    private void RefreshUI()
    {
        _bombLeftUI.text = $"{BombLeft} / {MaxBombNumber}";
    }


    void Update()
    {
        if (Gamemanager.Instance.State != GameState.Go)
        {
            return;
        }

        if (Input.GetMouseButtonDown(1) && BombLeft > 0 && !PlayerGunInfo.GetZoomMode())
        {
            BombFire();
            BombLeft--;
            RefreshUI();
        }
    }
    void BombFire()
    {
        // 2. 수류탄 던지는 위치에다가 수류탄 생성

            // 1. 꺼져 있는 총알을 찾아 꺼낸다.
            GameObject selectedBomb = null;
            foreach (GameObject b in BombPool)
            {
                if (b.activeInHierarchy == false)
                {
                    selectedBomb = b;
                    break; // 찾았기 때문에 그 뒤까지 찾을 필요가 없다.
                }
            }
        selectedBomb.SetActive(true);
        Rigidbody bombRigid = selectedBomb.GetComponent<Rigidbody>();
        bombRigid.velocity = Vector3.zero;
        bombRigid.AddForce(Camera.main.transform.forward * 30, ForceMode.Impulse);
        selectedBomb.transform.position = FirePosition.position;

    }
 
}
