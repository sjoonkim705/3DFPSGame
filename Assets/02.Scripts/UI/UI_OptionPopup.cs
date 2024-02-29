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
        Gamemanager.Instance.Continue();

    }
    public void OnClickedQuitButton()
    {
        UnityEditor.EditorApplication.isPlaying = false;
    }
}
