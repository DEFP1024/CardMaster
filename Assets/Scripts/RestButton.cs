using UnityEngine;

public class RestButton : MonoBehaviour
{
    [SerializeField] private int heal = 20;

    public void OnclickRest()
    {
        var player = GameManager.Instance.Player;
        player.Heal(heal);
    }
}
