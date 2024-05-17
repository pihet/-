using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{
    public Button togglePauseButton;
    private bool isPaused = false;

    void Start()
    {
        togglePauseButton.onClick.AddListener(TogglePause);
        UpdateButtonText();
    }

    void Update()
    {
        // ESC 키 입력 처리
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    void TogglePause()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0f : 1f;
        UpdateButtonText();
    }

    void UpdateButtonText()
    {
        Text buttonText = togglePauseButton.GetComponentInChildren<Text>();
        buttonText.text = isPaused ? "||" : "▶";
    }
}