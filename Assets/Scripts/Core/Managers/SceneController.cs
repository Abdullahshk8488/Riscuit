using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    [SerializeField] private Animator fadeInOutBlack_Anim;

    public static SceneController Instance;
    private SceneController _sceneController;

    public void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        _sceneController = GetComponent<SceneController>();

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void LoadLevel(string sceneName)
    {
        //StartCoroutine(FadeInOutTransition(sceneName));
        SceneManager.LoadScene(sceneName);
    }

    public void LoadLevel(int sceneIndex)
    {
        StartCoroutine(FadeInOutTransition(sceneIndex));
    }

    IEnumerator FadeInOutTransition(string sceneName)
    {
        fadeInOutBlack_Anim.SetBool("FadeOut", true);
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(sceneName);
    }

    IEnumerator FadeInOutTransition(int sceneIndex)
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(sceneIndex);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {

    }

    public void FadeInAnimation()
    {
        fadeInOutBlack_Anim.SetBool("FadeOut", true);
    }

    public void FadeOutAnimation()
    {
        fadeInOutBlack_Anim.SetBool("FadeOut", false);
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
