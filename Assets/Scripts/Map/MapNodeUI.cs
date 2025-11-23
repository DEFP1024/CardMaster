using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum NodeVisualState
{
    Locked,
    Selectable,
    Current
}

public class MapNodeUI : MonoBehaviour
{
    [SerializeField] private Image background; // 노드 배경 이미지
    [SerializeField] private TextMeshPro typeText;    // 타입 표시용 텍스트
    [SerializeField] private Button button;    // 클릭 버튼

    private MapGenerator map;
    private StageNode node;
    private NodeVisualState state;

    void Awake()
    {
        if (button == null)
            button = GetComponent<Button>();
    }

    public void Setup(MapGenerator map, StageNode node)
    {
        this.map = map;
        this.node = node;

        if (typeText != null)
            typeText.text = node.type.ToString();

        // 기본 상태 (나중에 전체 갱신하면서 바뀜)
        SetState(NodeVisualState.Locked);
    }

    public void SetState(NodeVisualState newState)
    {
        state = newState;

        switch (state)
        {
            case NodeVisualState.Locked:
                if (background != null)
                    background.color = Color.gray;
                if (button != null)
                    button.interactable = false;
                break;

            case NodeVisualState.Selectable:
                if (background != null)
                    background.color = Color.white;
                if (button != null)
                    button.interactable = true;
                break;

            case NodeVisualState.Current:
                if (background != null)
                    background.color = Color.yellow;
                if (button != null)
                    button.interactable = true;
                break;
        }
    }
}