using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private AudioClip UiButtonPressAudioClip;

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
        SoundFXManager.Instance.PlaySoundClip(UiButtonPressAudioClip, transform);
        GameManager.Instance.UnPauseGame();
        HidePauseScreen();
    }

    public void MainMenu()
    {
        SoundFXManager.Instance.PlaySoundClip(UiButtonPressAudioClip, transform);
        SceneController.Instance.LoadLevel("MainMenu");
    }

    public void Quit()
    {
        SoundFXManager.Instance.PlaySoundClip(UiButtonPressAudioClip, transform);
        Application.Quit();
    }

    private void OnDisable()
    {
        GameManager.Instance.GamePaused -= DisplayPauseScreen;
        GameManager.Instance.GameUnPaused -= HidePauseScreen;
    }
}
