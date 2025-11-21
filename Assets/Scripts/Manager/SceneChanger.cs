using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneChanger : Singleton<SceneChanger>
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void OnTitle() => SceneManager.LoadScene(0);

    public void OnStartGame() => SceneManager.LoadScene(1);

    public void OnOption() => SceneManager.LoadScene(2);

}
