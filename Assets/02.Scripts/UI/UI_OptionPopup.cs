using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_OptionPopup : MonoBehaviour
{
    public void Open()
    {
        // 사운드 효과음
        // 초기화 함수

        gameObject.SetActive(true);
    }
    public void Close()
    {
        gameObject.SetActive(false);
    }
    private void Awake()
    {
        gameObject.SetActive(false);
    }
    public void OnClickContinueButton()
    {
        Gamemanager.Instance.Continue();
        Close();
    }
    public void OnClickedRetryButton()
    {

        SceneManager.LoadScene("SampleScene");
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        Gamemanager.Instance.Continue();

    }
    public void OnClickedQuitButton()
    {
        
        Application.Quit(); // 빌드 후 실행했을 경우 종료하는 방법

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // 유니티 에디터에서 실행했을 경우
#endif

    }
}
