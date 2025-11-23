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

public class MapGenerator : MonoBehaviour
{
    public int floorCount = 6;
    public int nodeMaxCount = 4;
    public GameObject nodePrefab;
    public GameObject linePrefab;

    public float floorX = 3f;
    public float nodedistance = 2f;

    public List<List<StageNode>> floors = new List<List<StageNode>>();

    // 현재 플레이어가 있는 노드
    public StageNode currentNode;

    private void Start()
    {
        GenerateMap();
        DrawMap();
    }

    public void GenerateMap()
    {
        CreateNodes();
        ConnectNode();
    }

    private void CreateNodes()
    {
        floors.Clear();

        for (int i = 0; i < floorCount; i++)
        {
            floors.Add(new List<StageNode>());

            int count;
            if (i == 0 || i == floorCount - 1)
                count = 1;
            else
                count = Random.Range(1, nodeMaxCount + 1);

            for (int j = 0; j < count; j++)
            {
                StageNode node = new StageNode();
                node.floor = i;

                float x = i * floorX;
                float y = (j - (count - 1)/ 2f) * -nodedistance;
                node.position = new Vector2 (x, y);

                if (i == 0)
                    node.type = NodeType.Start;
                else if (i == floorCount - 1)
                    node.type = NodeType.Boss;
                else if (i == floorCount - 2)
                    node.type = NodeType.Rest;
                else
                    node.type = GetRandomNodeType();

                node.nextNodes =  new List<StageNode>();
                floors[i].Add(node);
            }
        }
    }

    private NodeType GetRandomNodeType()
    {
        int rnd = Random.Range(0, 4);
        switch (rnd)
        {
            case 0: return NodeType.Enemy;
            case 1: return NodeType.Shop;
            case 2: return NodeType.Event;
            case 3: return NodeType.Rest;
            default: return NodeType.Enemy;
        }
    }

    private void ConnectNode()
    {
        StageNode start = floors[0][0];
        StageNode boss = floors[floorCount - 1][0];
        // 0층 1층 연결
        foreach (StageNode n in floors[1])
        {
            AddWay(start, n);
        }

        // 1층 ~ 보스 전 까지
        for (int i = 1; i < floorCount - 2; i++)
        {
            List<StageNode> currentFloor = floors[i];
            List<StageNode> nextFloor = floors[i + 1];

            int pairCount = Mathf.Min(currentFloor.Count, nextFloor.Count);
            for (int k = 0; k < pairCount; k++)
            {
                AddWay(currentFloor[k], nextFloor[k]);
            }

            foreach (StageNode node in currentFloor)
            {
                if (node.nextNodes.Count == 0)
                {
                    StageNode rnd = nextFloor[Random.Range(0, nextFloor.Count)];
                    AddWay(node, rnd);
                }
            }

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
        }

        

        List<StageNode> lastBeforeBoss = floors[floorCount - 2];
        foreach (StageNode n in lastBeforeBoss)
        {
            AddWay(n, boss);
        }
    }

    private void AddWay(StageNode from, StageNode to)
    {
        if (from.nextNodes.Contains(to) == false)
            from.nextNodes.Add(to);
    }

    private void DrawMap()
    {
        foreach (List<StageNode> floor in floors)
        {
            foreach (StageNode node in floor)
            {
                GameObject obj = Instantiate(nodePrefab, transform);
                RectTransform rect = obj.GetComponent<RectTransform>();
                rect.anchoredPosition = node.position;
                node.uiObj = obj;

                MapNodeUI ui = obj.GetComponent<MapNodeUI>();
                ui.Setup(this, node);

            }
        }

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

                    GameObject lineObj = Instantiate(linePrefab, transform);
                    RectTransform lineRect = lineObj.GetComponent<RectTransform>();

                    Vector2 center = (posA + posB) * 0.5f;
                    lineRect.anchoredPosition = center;

                    float length = Vector2.Distance(posA, posB);
                    Vector2 size = lineRect.sizeDelta;
                    size.x = length;
                    lineRect.sizeDelta = size;

                    Vector2 dir = (posB - posA).normalized;
                    float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                    lineRect.localRotation = Quaternion.Euler(0, 0, angle);
                }
            }
        }
    }
}
