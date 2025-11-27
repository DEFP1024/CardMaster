using System.Collections.Generic;
using UnityEngine;


public enum NodeType
{
    Start,
    Enemy,
    Shop,
    Event,
    Rest,
    Boss
}

public class MapGenerator : Singleton<MapGenerator>
{
    public int floorCount = 6; // 총 층 수
    public int nodeMinCount = 1; // 층별 노드 최소 개수
    public int nodeMaxCount = 4; //  층별 노드 최대 개수

    public GameObject nodePrefab;
    public GameObject linePrefab;
    public GameObject parentObj;
    
    public float floorX = 3f; // 층 간 간격
    public float nodedistance = 2f; // 노드간 간격

    public List<List<StageNode>> floors = new List<List<StageNode>>();

    // 현재 플레이어가 있는 노드
    public StageNode currentNode;

    private void Start()
    {
        if (floors == null || floors.Count == 0)
        {
            GenerateMap();
            DrawMap();

            SelectNode(floors[0][0]);
        }
    }

    public void GenerateMap()
    {
        CreateNodes();
        ConnectNode();
    }

    // 노드 생성
    // 층별로 랜덤하게 노드를 생성
    private void CreateNodes()
    {
        floors.Clear();
        // 층수 만큼 반복
        for (int i = 0; i < floorCount; i++)
        {
            floors.Add(new List<StageNode>());

            int count; // 층수별 생성될 노드 수

            if (i == 0 || i == floorCount - 1)
                count = 1; // 1층과 보스는 항상 1개를 보장
            else
                count = Random.Range(nodeMinCount, nodeMaxCount + 1); // 최대와 최저 사이에서 랜덤으로 뽑는다 

            // 생성된 노드의 타입을 결정
            for (int j = 0; j < count; j++)
            {
                StageNode node = new StageNode();
                node.floor = i;

                // 노드간 거리 입니다
                float x = i * floorX;
                float y = (j - (count - 1)/ 2f) * -nodedistance;

                node.position = new Vector2 (x, y);
               
                // 노드가 1층과 마지막 층이라면 각각 시작과 보스를 부여 그외에는 무작위
                if (i == 0)
                    node.type = NodeType.Start;
                else if (i == floorCount - 1)
                    node.type = NodeType.Boss;
                else if (i == floorCount - 2) // 보스 전 방은 항상 휴식장소
                    node.type = NodeType.Rest;
                else
                    node.type = GetRandomNodeType();

                node.nextNodes =  new List<StageNode>();
                floors[i].Add(node);
            }
        }
    }

    //랜덤으로 타입지정
    private NodeType GetRandomNodeType()
    {
        int rnd = Random.Range(0, 10);
        switch (rnd)
        {
            case 0: return NodeType.Shop;
            case 1: return NodeType.Event;
            case 2: return NodeType.Rest;
            default: return NodeType.Enemy;
        }
    }

    // 노드간 연결
    private void ConnectNode()
    {
        StageNode start = floors[0][0];
        StageNode boss = floors[floorCount - 1][0];
        // 0층 1층 전체 연결
        foreach (StageNode n in floors[1])
        {
            AddWay(start, n);
        }

        // 1층 ~ 보스 전 까지
        for (int i = 1; i < floorCount - 2; i++)
        {
            List<StageNode> currentFloor = floors[i];
            List<StageNode> nextFloor = floors[i + 1];

            // 같은 인덱스끼리 1:1 연결
            int pairCount = Mathf.Min(currentFloor.Count, nextFloor.Count);
            for (int k = 0; k < pairCount; k++)
            {
                AddWay(currentFloor[k], nextFloor[k]);
            }

            // 연결이 하나도 없는 노드에게 무작위로 다음 노드 연결
            foreach (StageNode node in currentFloor)
            {
                if (node.nextNodes.Count == 0)
                {
                    StageNode rnd = nextFloor[Random.Range(0, nextFloor.Count)];
                    AddWay(node, rnd);
                }
            }

            // 다음 층에서 아무도 자신과 연결하지 않으면 강제로 연결
            foreach (StageNode next in nextFloor)
            {
                bool hasIncoming = false;

                foreach (StageNode prev in currentFloor)
                {
                    if (prev.nextNodes.Contains(next))
                    {
                        hasIncoming = true;
                        break;
                    }
                }

                if (hasIncoming == false)
                {
                    StageNode source = currentFloor[Random.Range(0, currentFloor.Count)];
                    AddWay(source, next);
                }
            }

            // 여분의 랜덤 연결 다양성 증가
            int extraLine = Random.Range(1, 3);
            for (int t = 0; t < extraLine;  t++)
            {
                StageNode p = currentFloor[Random.Range(0, currentFloor.Count)];
                StageNode n = nextFloor[Random.Range(0, nextFloor.Count)];
                AddWay(p, n);
            }
        }

        
        // 마지막 전 보스 전체 연결
        List<StageNode> lastBeforeBoss = floors[floorCount - 2];
        foreach (StageNode n in lastBeforeBoss)
        {
            AddWay(n, boss);
        }
    }

    // from 에서 to 연결
    private void AddWay(StageNode from, StageNode to)
    {
        if (from.nextNodes.Contains(to) == false)
            from.nextNodes.Add(to);
    }

    // 화면에 맵을 그려주는 매서드
    private void DrawMap()
    {
        foreach (List<StageNode> floor in floors)
        {
            foreach (StageNode node in floor)
            {
                GameObject obj = Instantiate(nodePrefab, parentObj.transform);
                RectTransform rect = obj.GetComponent<RectTransform>();
                rect.anchoredPosition = node.position;
                node.uiObj = obj;
                obj.name = node.type.ToString();

                MapNodeUI ui = obj.GetComponent<MapNodeUI>();
                ui.Setup(this, node);

            }
        }
        // 길 그리기
        foreach (List<StageNode> floor in floors)
        {
            foreach(StageNode node in floor)
            {
                if (node.nextNodes == null) continue;

                RectTransform rectA = node.uiObj.GetComponent<RectTransform>();
                Vector2 posA = rectA.anchoredPosition;

                foreach(StageNode next in node.nextNodes)
                {
                    RectTransform rectB = next.uiObj.GetComponent<RectTransform>();
                    Vector2 posB = rectB.anchoredPosition;

                    GameObject lineObj = Instantiate(linePrefab, parentObj.transform.GetChild(0).transform);
                    RectTransform lineRect = lineObj.GetComponent<RectTransform>();

                    Vector2 center = (posA + posB) * 0.5f; // 두 노드 중간에 배치
                    lineRect.anchoredPosition = center;

                    float length = Vector2.Distance(posA, posB); // 길이 계산
                    Vector2 size = lineRect.sizeDelta;
                    size.x = length;
                    lineRect.sizeDelta = size;

                    Vector2 dir = (posB - posA).normalized; // 방향
                    float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                    lineRect.localRotation = Quaternion.Euler(0, 0, angle); // 각도
                }
            }
        }
    }

    // 특정 노드를 선택했을 때 UI
    public void SelectNode(StageNode node)
    {
        currentNode = node;

        foreach (List<StageNode> floor in floors)
        {
            foreach (StageNode next in floor)
            {
                MapNodeUI ui = next.uiObj.GetComponent<MapNodeUI>();

                if (next ==   node)
                {
                    ui.SetState(NodeState.Current); // 현재 위치
                }
                else if ( node.nextNodes.Contains(next))
                {
                    ui.SetState(NodeState.Selectable); // 이동 가능
                }
                else
                {
                    ui.SetState(NodeState.Locked); // 잠김
                }
            }
        }
    }

    public void MapClear()
    {
        floors.Clear(); // 맵 데이터 초기화
    }
}
