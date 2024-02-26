using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 역할: 게임 관리자
// -> 게임 전체의 상태를 알리고, 시작과 끝을 텍스트로 나타낸다.
public enum GameState
{
    Ready, // 대기
    Go, // 시작
    Over,  // 게임오버


}
public class Gamemanager : MonoBehaviour
{
    // 게임의 상태는 처음에 "준비"상태
    public static Gamemanager Instance { get; private set; }
    public GameState State { get; private set; } = GameState.Ready;
    public InputField InputField;
    public Text StateTextUI;

    // 게임 상태
    // 1. 게임 준비 상태
    // 2. 1.6초 후에 게임 시작 상태
    // 3. 0.4초 후에 텍스트 사라지고
    // 4. 플레이중
    // 5. 체력 0이되면 게임 오버
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);

        }

    }
    private void Start()
    {
        StartCoroutine(Start_Coroutine());

    }

    private IEnumerator Start_Coroutine()
    {
        State = GameState.Ready;
        Refresh();
        StateTextUI.gameObject.SetActive(true);
        yield return new WaitForSeconds(1.6f);
        State = GameState.Go;
        Refresh();
        yield return new WaitForSeconds(0.4f);
        StateTextUI.gameObject.SetActive(false);

    }
    public PlayerMoveAbility Player;

    public void GameOver()
    {
         State = GameState.Over;
         StateTextUI.gameObject.SetActive(true);
         Refresh();
    }
    

   
    public void Refresh()
    {
        switch (State)
        {
            case GameState.Ready:
            {
                StateTextUI.color = new Color(130f/255f, 100/255f, 0);
                StateTextUI.text = "Ready";
                break;
            }
            case GameState.Go:
            {
                StateTextUI.color = new Color(5f/255f, 150f/255f, 0);
                StateTextUI.text = "Go!";
                break;
            }
            case GameState.Over:
            {
                StateTextUI.text = "Game Over";
                break;
            }
        }
    }


}
