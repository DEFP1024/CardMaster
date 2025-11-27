using UnityEngine;

public class EnemyStarter : MonoBehaviour
{
    [SerializeField] private NodeType nodeType = NodeType.Enemy;
    void Start()
    {
        GameManager.Instance.StartBattle(nodeType);
    }

}
