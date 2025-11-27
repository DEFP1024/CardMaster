using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneChanger : Singleton<SceneChanger>
{
    MapGenerator map;
    CardStockManager cardStockManager;
    CardDeck cardDeck;

    public void OnTitle()
    {
        SceneManager.LoadScene("TitleScene");

        map = MapGenerator.Instance;
        cardStockManager = CardStockManager.Instance;
        cardDeck = CardDeck.Instance;

        var player = FindFirstObjectByType<Player>();
        Destroy(player.gameObject);

        if (map != null)
        {
            Destroy(map.gameObject);
            Destroy(cardStockManager.gameObject);
            Destroy(cardDeck.gameObject);
            
            CardDeck.Instance.ClearCards();
        }
    }

    public void OnStage()
    {
        var a = SceneManager.LoadSceneAsync("StageScene");
        a.completed += (_) =>
        {
            if (CardDeck.Instance.StartComplete ==  false)
            {
                CardDeck.Instance.StartGame();
                CardDeck.Instance.StartComplete =  true;
            }

            map = MapGenerator.Instance;

            if (map != null)
            {
                map.gameObject.SetActive(true);
            }
        };
    }

    public void OnOption() => SceneManager.LoadScene("OptionScene");

    public void OnStageScene(NodeType nodeType)
    {
        switch (nodeType)
        {
            case NodeType.Shop:
                SceneManager.LoadScene($"{nodeType.ToString()}Scene");
                break;
            case NodeType.Enemy:
                SceneManager.LoadScene($"{nodeType.ToString()}Scene");
                break;
            case NodeType.Rest:
                SceneManager.LoadScene($"{nodeType.ToString()}Scene");
                break;
            case NodeType.Event:
                SceneManager.LoadScene($"{nodeType.ToString()}Scene");
                break;
            case NodeType.Boss:
                SceneManager.LoadScene($"{nodeType.ToString()}Scene");
                break;
        }
    }
        

    public void OnVictory() => SceneManager.LoadScene("VictoryScene");

    public void OnDefeate() => SceneManager.LoadScene("DefeatScene");
}
