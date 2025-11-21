using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneChanger : Singleton<SceneChanger>
{
    public void OnTitle() => SceneManager.LoadScene("TitleScene");

    public void OnStartGame() => SceneManager.LoadScene("FirstCardChoiceScene");

    public void OnOption() => SceneManager.LoadScene("OptionScene");
}
