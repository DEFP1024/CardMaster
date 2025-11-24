using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneChanger : Singleton<SceneChanger>
{
    MapGenerator map;

    public void OnTitle()
    {
        SceneManager.LoadScene("TitleScene");

        map = MapGenerator.Instance;

        if (map != null)
        {
            Destroy(map.gameObject);
        }
    }

    public void OnStage()
    {
        var a = SceneManager.LoadSceneAsync("StageScene");
        a.completed += (_) =>
        {
            map = MapGenerator.Instance;

            if (map != null)
            {
                map.gameObject.SetActive(true);
            }
        };
    }

    public void OnOption() => SceneManager.LoadScene("OptionScene");

    public void OnStageScene(NodeType nodeType) => SceneManager.LoadScene($"{nodeType.ToString()}Scene");
}
