using UnityEngine;
using UnityEngine.UI;

public class EndTurnButton : MonoBehaviour
{
    private void Awake()
    {
        Button btn = GetComponent<Button>();

        btn.onClick.AddListener(OnClickEndTurn);
    }

    private void OnClickEndTurn()
    {
        GameManager.Instance.OnClickEndTurn();
    }
}
