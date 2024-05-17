using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{
    public Button pauseButton;
    private bool isPaused = false;

    void Start()
    {
        // 버튼에 클릭 리스너 추가
        pauseButton.onClick.AddListener(TogglePause);
    }

    void TogglePause()
    {
        if (isPaused)
        {
            // 게임을 다시 시작
            GameManager.instance.Stop();
        }
        else
        {
            // 게임을 멈춤
            GameManager.instance.Resume();
        }
        isPaused = !isPaused;
    }
}