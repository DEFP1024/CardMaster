using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum NodeState
{
    Locked,
    Selectable,
    Current
}

public class MapNodeUI : MonoBehaviour
{
    [SerializeField] private Image background; // 노드 배경 이미지
    [SerializeField] private TextMeshProUGUI typeText;    // 타입 표시용 텍스트
    [SerializeField] private Button button;    // 클릭 버튼

    private MapGenerator map;
    private StageNode node;
    private NodeState state;

    void Awake()
    {
        if (button == null)
            button = GetComponent<Button>();

        button.onClick.AddListener(OnNodeClick);
    }

    public void Setup(MapGenerator map, StageNode node)
    {
        this.map = map;
        this.node = node;

        if (typeText != null)
            typeText.text = node.type.ToString();

        // 기본 상태
        SetState(NodeState.Locked);
    }

    public void SetState(NodeState newState)
    {
        state = newState;

        switch (state)
        {
            case NodeState.Locked:
                if (background != null)
                    background.color = Color.gray;
                if (button != null)
                    button.interactable = false;
                break;

            case NodeState.Selectable:
                if (background != null)
                    background.color = Color.white;
                if (button != null)
                    button.interactable = true;
                break;

            case NodeState.Current:
                if (background != null)
                    background.color = Color.yellow;
                if (button != null)
                    button.interactable = true;
                break;
        }
    }
    private void OnNodeClick()
    {
        // 현재 노드 or 이동 가능한 노드만 선택 허용
        if (state == NodeState.Selectable || node.floor == 0)
        {
            map.SelectNode(node);
        }

        switch (node.type)
        {
            case NodeType.Enemy:
                map.gameObject.SetActive(false);
                SceneChanger.Instance.OnStageScene(node.type);
                break;
            case NodeType.Shop:
                map.gameObject.SetActive(false);
                SceneChanger.Instance.OnStageScene(node.type);
                break;
            case NodeType.Rest:
                map.gameObject.SetActive(false);
                SceneChanger.Instance.OnStageScene(node.type);
                break;
            case NodeType.Boss:
                map.gameObject.SetActive(false);
                SceneChanger.Instance.OnStageScene(node.type);
                break;
            case NodeType.Event:
                map.gameObject.SetActive(false);
                SceneChanger.Instance.OnStageScene(node.type);
                break;
        }
    }

    
}