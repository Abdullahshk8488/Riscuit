using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseScreen;

    private void Awake()
    {
        GameManager.Instance.GamePaused += DisplayPauseScreen;
        GameManager.Instance.GameUnPaused += HidePauseScreen;
    }

    public void DisplayPauseScreen()
    {
        pauseScreen.SetActive(true);
    }

    public void HidePauseScreen()
    {
        pauseScreen.SetActive(false);
    }

    public void Resume()
    {
        GameManager.Instance.UnPauseGame();
        HidePauseScreen();
    }

    public void MainMenu()
    {
        SceneController.Instance.LoadLevel("MainMenu");
    }

    public void Quit()
    {
        Application.Quit();
    }

    private void OnDisable()
    {
        GameManager.Instance.GamePaused -= DisplayPauseScreen;
        GameManager.Instance.GameUnPaused -= HidePauseScreen;
    }
}
