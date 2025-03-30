using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MapGraphTwo : MonoBehaviour
{
    public GameObject nodePrefab;
    public GameObject[] obstaclePrefabs;
    public int gridSize = 19;
    public float spacing = 1.0f;
    public int nodeNum = 0;
    private Node[,] nodes;
    public List<Node> path;


    void GenerateGrid()
    {
        nodes = new Node[gridSize, gridSize];
        int nodeNum = 0;
        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                Vector3 position = new Vector3(x * spacing, -1, y * spacing);
                GameObject nodeObject = Instantiate(nodePrefab, position, Quaternion.identity, transform);
                Node node = nodeObject.GetComponent<Node>();
                nodes[x, y] = node;
                node.nodeNum = nodeNum;

                Debug.Log($"Node created at index ({x},{y}) with nodeNum: {nodeNum}");

                if (ObstacleSpawn(position, nodeNum))
                {
                    node.isObstacle = true;
                }
                nodeNum++;
            }
        }

        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                Node node = nodes[x, y];
                if (x > 0)
                {
                    Node leftNeighbor = nodes[x - 1, y];
                    node.AddNeighbor(leftNeighbor);
                    leftNeighbor.AddNeighbor(node);
                }
                if (y > 0)
                {
                    Node bottomNeighbor = nodes[x, y - 1];
                    node.AddNeighbor(bottomNeighbor);
                    bottomNeighbor.AddNeighbor(node);
                }
                if (x < gridSize - 1)
                {
                    Node rightNeighbor = nodes[x + 1, y];
                    node.AddNeighbor(rightNeighbor);
                    rightNeighbor.AddNeighbor(node);
                }
                if (y < gridSize - 1)
                {
                    Node topNeighbor = nodes[x, y + 1];
                    node.AddNeighbor(topNeighbor);
                    topNeighbor.AddNeighbor(node);
                }
            }
        }

    }


    bool ObstacleSpawn(Vector3 position, int number)
    {
        switch (number)
        {
            case 26:
            case 27:
            case 28:
            case 29:
            case 30:
            case 31:
            case 32:
            case 33:
            case 34:
            case 35:

            case 54:

            case 62:
            case 73:
            
            case 81:
            case 92:

            case 100:
            case 111:

            case 119:
            case 130:

            case 138:
            case 149:

            case 157:
            case 168:

            case 176:
            case 187:

            case 195:
            case 206:

            case 225:
            case 244:

            case 263:
            case 262:
            case 261:
            case 260:
            case 259:
            case 258:
            case 257:
            case 256:
            case 255:
            case 254:
            case 253:
                position.y = 0;
                Instantiate(obstaclePrefabs[Random.Range(0, 5)], position, Quaternion.identity, transform);
                return true;
        }
        return false;
    }
    
    void FindPath(int startNodeIndex, int endNodeIndex)
    {
        int startX = startNodeIndex / gridSize;
        int startY = startNodeIndex % gridSize;
        int endX = endNodeIndex / gridSize;
        int endY = endNodeIndex % gridSize;

        Node startNode = nodes[startX, startY];
        Node endNode = nodes[endX, endY];

        Debug.Log($"FindPath :  startNodeIndex: {startNodeIndex}, endNodeIndex: {endNodeIndex}");
        Debug.Log($"startNode: {startNode.nodeNum}, endNode: {endNode.nodeNum}");

        path = BFS(startNode, endNode);

        if (path != null)
        {
            foreach (Node node in path)
            {
                Debug.Log($"Path Node: {node.transform.position}, NodeNum: {node.nodeNum}");
            }
        }
        else
        {
            Debug.Log("NO PATH");
        }
    }


    List<Node> BFS(Node start, Node goal)
    {
        Queue<Node> frontq = new Queue<Node>();
        frontq.Enqueue(start);

        Dictionary<Node, Node> cameFrom = new Dictionary<Node, Node>();
        cameFrom[start] = null;

        while (frontq.Count > 0)
        {
            Node current = frontq.Dequeue();

            if (current == goal)
            {
                return MakePath(cameFrom, current);
            }

            foreach (Node neighbor in current.neighbors)
            {
                if (!neighbor.isObstacle && !cameFrom.ContainsKey(neighbor))
                {
                    frontq.Enqueue(neighbor);
                    cameFrom[neighbor] = current;
                }
            }
        }

        return null;
    }

    List<Node> MakePath(Dictionary<Node, Node> cameFrom, Node current)
    {
        List<Node> totalPath = new List<Node> { current };

        while (cameFrom[current] != null)
        {
            current = cameFrom[current];
            totalPath.Add(current);
        }

        totalPath.Reverse();
        return totalPath;
    }


    void Start()
    {
        GenerateGrid();
        FindPath(24, 300);
    }
}

    

