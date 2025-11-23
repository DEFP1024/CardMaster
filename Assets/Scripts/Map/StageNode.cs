using System.Collections.Generic;
using UnityEngine;

public class StageNode
{
    public int floor;
    public Vector2 position;
    public NodeType type;
    public List<StageNode> nextNodes;

    public GameObject uiObj;
}
