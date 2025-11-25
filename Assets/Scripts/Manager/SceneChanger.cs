using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneChanger : Singleton<SceneChanger>
{
    MapGenerator map;
    CardStockManager cardStockManager;

    public void OnTitle()
    {
        SceneManager.LoadScene("TitleScene");

        map = MapGenerator.Instance;
        cardStockManager = CardStockManager.Instance;

        if (map != null)
        {
            Destroy(map.gameObject);
            Destroy(cardStockManager.gameObject);
            CardDeck.Instance.ClearCards();
        }
    }

    public void OnStage()
    {
        var a = SceneManager.LoadSceneAsync("StageScene");
        a.completed += (_) =>
        {
            CardDeck.Instance.StartGame();

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
