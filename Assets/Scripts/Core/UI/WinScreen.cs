using UnityEngine;

public class WinScreen : MonoBehaviour
{
    public void MainMenu()
    {
        SceneController.Instance.LoadLevel("MainMenu");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
