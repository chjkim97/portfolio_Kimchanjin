using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapRandomMaker : MonoBehaviour
{

    public GameObject nodePrefab;
    public GameObject[] MapPrefabs;
    public int gridSizeX = 12;
    public int gridSizeY = 6;

    public float xspacing = 5.0f;
    public float yspacing = 1.0f;
    public int nodeNum = 0;
    private Node[,] nodes;
    public List<Node> path;

    void GenerateGrid()
    {
        nodes = new Node[gridSizeX, gridSizeY];
        int nodeNum = 0;
        for (int x = 0; x < gridSizeX; x++) 
        {
            HashSet<int> randomIndices = GenerateRandomIndices(gridSizeY, 3);
            for (int y = 0; y < gridSizeY; y++) 
            {
                Vector2 position = new Vector2(x*xspacing, y*yspacing);
                GameObject nodeObject = Instantiate(nodePrefab, position, Quaternion.identity, transform);
                Node node = nodeObject.GetComponent<Node>();
                nodes[x, y] = node;
                node.nodeNum = nodeNum;

                Debug.Log($"Node created at index ({x},{y}) with nodeNum: {nodeNum}");

                if (randomIndices.Contains(y))
                {
                    Instantiate(MapPrefabs[SpawnProbabilty()], position, Quaternion.identity, transform);
                    node.isMap = true;
                    Debug.Log($"MapIcon maked with Index Node ({x},{y}) with nodeNum: {nodeNum}");
                }
                nodeNum++;
            }


        }

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
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
                if (x < gridSizeX - 1)
                {
                    Node rightNeighbor = nodes[x + 1, y];
                    node.AddNeighbor(rightNeighbor);
                    rightNeighbor.AddNeighbor(node);
                }
                if (y < gridSizeY - 1)
                {
                    Node topNeighbor = nodes[x, y + 1];
                    node.AddNeighbor(topNeighbor);
                    topNeighbor.AddNeighbor(node);
                }
            }
        }
    }

   

    HashSet<int> GenerateRandomIndices(int range, int count)
    {
        HashSet<int> indices = new HashSet<int>();
        System.Random rand = new System.Random();
        while (indices.Count < count)
        {
            indices.Add(rand.Next(range));
        }
        return indices;
    }

int SpawnProbabilty()
    {
        float randomValue = Random.Range(0f, 100f);

        if (randomValue < 50f)
        {
            return 0; 
        }
        else if (randomValue < 70f)
        {
            return 1; 
        }
        else if (randomValue < 80f)
        {
            return 2; 
        }
        else if (randomValue < 90f)
        {
            return 3; 
        }
        else
            return 4;
    }


    









    void Start()
    {
        GenerateGrid();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
