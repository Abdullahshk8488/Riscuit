using UnityEngine;

public class LoseScreen : MonoBehaviour
{
    public void Retry()
    {
        SceneController.Instance.LoadLevel("LV_0");
    }

    public void MainMenu()
    {
        SceneController.Instance.LoadLevel("MainMenu");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
