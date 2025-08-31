using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private AudioClip UiButtonPressAudioClip;

    public void StartButton()
    {
        SoundFXManager.Instance.PlaySoundClip(UiButtonPressAudioClip, transform);
        GameManager.Instance.UnPauseGame();
        SceneController.Instance.LoadLevel("LV_0");
    }

    public void Credits()
    {
        SoundFXManager.Instance.PlaySoundClip(UiButtonPressAudioClip, transform);
    }

    public void Quit()
    {
        SoundFXManager.Instance.PlaySoundClip(UiButtonPressAudioClip, transform);
        Application.Quit();
    }
}
