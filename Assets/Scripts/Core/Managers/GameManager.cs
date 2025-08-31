using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public event Action GamePaused;
    public event Action GameUnPaused;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void PauseGame()
    {
        Time.timeScale = 0.0f;
        GamePaused?.Invoke();
    }

    public void UnPauseGame()
    {
        Time.timeScale = 1.0f;
        GameUnPaused?.Invoke();
    }

    public void TogglePause()
    {
        if (Time.timeScale > 0.0f)
        {
            PauseGame();
        }
        else
        {
            UnPauseGame();
        }
    }
}
