using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void StartButton()
    {
        SceneController.Instance.LoadLevel("LV_0");
    }

    public void Credits()
    {

    }

    public void Quit()
    {
        Application.Quit();
    }
}
