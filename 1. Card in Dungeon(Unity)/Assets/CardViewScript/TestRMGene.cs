using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
//using static UnityEditor.Experimental.GraphView.GraphView;

public class TestRMGene : MonoBehaviour
{
    public GameObject nodePrefab;
    public GameObject NodeObj;
    //public GameObject edgePrefab;
    public TestMapNode root;
    public TestMapNode endNode;
    public List<GameObject> nodes;
    public List<Vector3> generatedPositions;
    public List<GameObject> Path;
    int totalNodeCount = 11;
    int doubleConnect = 3;

    public float yRangePivot = 150;

    public List<TestMapNode> edgeList;

    private int nodeCounter = 0;

    private static bool isFirstEntry = true;

    public Camera MainCamera;
    public Vector3 currentPos= new Vector3(0,0,-10);

    public List<TestMapNode> nodesList;

    public class LimitedQueue<TestMapNode>
    {
        private List<TestMapNode> elements = new List<TestMapNode>();
        private int maxSize = 2; 

        public void Enqueue(TestMapNode item)
        {
            if (elements.Count >= maxSize)
            {
                elements.RemoveAt(0);
            }
            elements.Add(item);
        }

        public int Count()
        {
            return elements.Count;
        }
        public TestMapNode GetElements(int i)
        {
            return elements[i]; 
           
        }
    }

    public class PathFinder
    {
        public List<TestMapNode> FindShortestPathToEnd(List<TestMapNode> nodes, TestMapNode startNode)
        {
           
            foreach (var node in nodes)
            {
                node.isMinRoute = false; 
            }
            var distance = new Dictionary<TestMapNode, int>();
            var previous = new Dictionary<TestMapNode, TestMapNode>();

            foreach (var node in nodes)
            {
                distance[node] = int.MaxValue;
                previous[node] = null;
            }
            distance[startNode] = 0;

            var stack = TopologicalSort(nodes);

            while (stack.Count > 0)
            {
                var node = stack.Pop();

                if (distance[node] == int.MaxValue) continue;

                foreach (var nextNode in node.nextNodes)
                {
                    int newDist = distance[node] + 1;
                    if (newDist < distance[nextNode])
                    {
                        distance[nextNode] = newDist;
                        previous[nextNode] = node;
                    }
                }
            }

            TestMapNode endNode = nodes.First(n => n.layer == 13);
            var path = new List<TestMapNode>();
            for (TestMapNode at = endNode; at != null; at = previous[at])
            {
                path.Add(at);
            }
            path.Reverse();

            foreach (var node in path)
            {
                node.isMinRoute = true;
            }

            return path;
        }

        private Stack<TestMapNode> TopologicalSort(List<TestMapNode> nodes)
        {
            var stack = new Stack<TestMapNode>();
            var visited = new HashSet<TestMapNode>();

            foreach (var node in nodes)
            {
                if (!visited.Contains(node))
                {
                    DFS(node, visited, stack);
                }
            }

            return stack;
        }

        private void DFS(TestMapNode node, HashSet<TestMapNode> visited, Stack<TestMapNode> stack)
        {
            visited.Add(node);
            foreach (var nextNode in node.nextNodes)
            {
                if (!visited.Contains(nextNode))
                {
                    DFS(nextNode, visited, stack);
                }
            }
            stack.Push(node);
        }
    }

    public LimitedQueue<TestMapNode> chooseNode;
    public PathFinder minRoute;

    private void Awake()
    {
        GameObject existingNodeObj = GameObject.Find("RandomMapGene");

        if (existingNodeObj != null && existingNodeObj != NodeObj)
        {
            Destroy(NodeObj);
        }
        else
        {
            NodeObj.name = "RandomMapGene"; 
            DontDestroyOnLoad(NodeObj);
            Debug.Log("노드들 생성" + NodeObj.name);
        }

    }

    private void Start()
    {
        chooseNode = new LimitedQueue<TestMapNode>();
        minRoute = new PathFinder();
        if (isFirstEntry)
        {
            GenerateMap();
            
            RemoveNodesWithNoPreviousNodes();
            isFirstEntry = false; 
        }
        MainCamera = Camera.main;
        MakeNodeList(nodes);
    }

    private void Update()
    {
        if( MainCamera != null)
        {

            // Debug.Log("카메라 연결");
            currentPos.x = MainCamera.transform.position.x;
            currentPos.y = MainCamera.transform.position.y;

            // Debug.Log(currentPos);

        }
    }
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    private void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode)
    {
        Debug.Log("OnEnable called.");

        if (NodeObj != null)
        {
            Debug.Log("NodeObj is active: " + NodeObj.activeSelf);
        }
        else
        {
            Debug.Log("NodeObj is null.");
        }

        Debug.Log("isFirstEntry: " + isFirstEntry);
        if (SceneManager.GetActiveScene().name == "TestRandomMap")
        {
            MainCamera = Camera.main;
            if (MainCamera != null)
            {
                Debug.Log(" 카메라 위치" + currentPos);
                MainCamera.transform.position = currentPos;
            }
            else
            {
                Debug.LogWarning("MainCamera is not found!");
            }
           // Debug.Log(" 카메라 위치" + currentPos);
           // MainCamera.transform.position = currentPos;

            if (!isFirstEntry)
            {
                foreach (GameObject node in nodes)
                {
                    if (node != null)
                    {
                        Debug.Log("Activating node: " + node.name);
                        node.SetActive(true);
                    }
                    else
                    {
                        Debug.Log("Node is null.");
                    }
                }
            }
        }
    }

    public void MakeNodeList(List<GameObject> nodes)
    {
        nodesList = nodes.Select(go => go.GetComponent<TestMapNode>()).ToList();
    }
    public void HideNodes()
    {
        foreach (GameObject node in nodes)
        {
            node.SetActive(false); 
        }
    }
    public void GenerateMap()
    {
       
        GameObject root = CreateNode(new Vector3(0, 0, 0),0);
        GameObject end = CreateNode(new Vector3(0, -2850, 0), 13);

        nodes.Add(root);
        nodes.Add(end);

        float yPosition = -100;
        generatedPositions = new List<Vector3>();
        for (int  layer = 1; layer < 13; layer++) 
        {
            int nodeCount = Random.Range(2, 5);
            if( layer !=1)
            {
                yPosition -= 220;
            }
            for (int i = 0; i < nodeCount; i++) 
            {
                float yPos = Random.Range(yPosition - 75, yPosition);
                Vector3 position = GenerateValidPosition(yPos, generatedPositions);
                GameObject middleNode = CreateNode(position,layer);
                nodes.Add(middleNode);
                generatedPositions.Add(position);
            }
        }
        MakePath(nodes);
        ConnectEdge(nodes);
    }

    public void MakePath(List<GameObject> nodes)
    {
        GameObject root = nodes.Find(node => node.GetComponent<TestMapNode>().layer == 0);
        GameObject end = nodes.Find(node => node.GetComponent<TestMapNode>().layer == 13);
        
        if (root != null && end != null)
        {
            TestMapNode rootComponent = root.GetComponent<TestMapNode>();
            TestMapNode endComponent = end.GetComponent<TestMapNode>();
            // nodes 리스트에서 layer == 1인 모든 노드를 찾음
            List<GameObject> layer1Nodes = nodes.FindAll(node => node.GetComponent<TestMapNode>().layer == 1);
            List<GameObject> layer12Nodes = nodes.FindAll(node => node.GetComponent<TestMapNode>().layer == 12);

            // layer == 1인 노드를 root의 nextNodes에 추가하고, previousNode를 root로 설정
            foreach (GameObject layer1Node in layer1Nodes)
            {
                TestMapNode layer1NodeComponent = layer1Node.GetComponent<TestMapNode>();

                // root의 nextNodes에 layer1Node 추가
                rootComponent.AddNextNode(layer1NodeComponent);

                // layer1Node의 previousNode를 root로 설정
                layer1NodeComponent.AddpPreviousNode(rootComponent);
            }
            foreach (TestMapNode nextNode in rootComponent.nextNodes)
            {
                Debug.Log($"Next Node Position: {nextNode.position}");
            }
            foreach (GameObject layer12Node in layer12Nodes)
            {
                TestMapNode layer12NodeComponent = layer12Node.GetComponent<TestMapNode>();
                layer12NodeComponent.AddNextNode(endComponent);
                endComponent.AddpPreviousNode(layer12NodeComponent);
            }
        }

        foreach (GameObject node in nodes)
        {
            TestMapNode nodeComponent = node.GetComponent<TestMapNode>();

            if (nodeComponent.layer >= 1 && nodeComponent.layer <= 10 && nodeComponent.previousNodes.Count != 0)
            {
                int currentLayer = nodeComponent.layer;

                // 다음 층의 노드들 찾기
                int nextLayer1 = currentLayer + 1; // 다음 층
                int nextLayer2 = currentLayer + 2; // 다음 층의 다음 층

                // 다음 층의 노드들 찾기
                List<GameObject> nextLayerNodes = nodes.FindAll(n =>
                    n.GetComponent<TestMapNode>().layer == nextLayer1 || (doubleConnect >= 0 && n.GetComponent<TestMapNode>().layer == nextLayer2)); //(doubleConnect > 0 && n.GetComponent<TestMapNode>().layer == nextLayer2)

                // 현재 노드와 랜덤으로 연결할 다음 노드 수
                int randomCount = Random.Range(1, 4); // 1 또는 3

                // 다음 층의 노드 중 랜덤으로 선택하여 연결
                List<TestMapNode> selectedNextNodes = new List<TestMapNode>();
                for (int i = 0; i < randomCount; i++)
                {
                    if (nextLayerNodes.Count == 0) break; // 다음 층의 노드가 없으면 중단

                    int randomIndex = Random.Range(0, nextLayerNodes.Count);
                    TestMapNode nextNodeComponent = nextLayerNodes[randomIndex].GetComponent<TestMapNode>();
                    selectedNextNodes.Add(nextNodeComponent);
                    nodeComponent.AddNextNode(nextNodeComponent); // 현재 노드에 다음 노드 연결

                    // 연결된 노드에서 이전 노드 설정
                    nextNodeComponent.AddpPreviousNode(nodeComponent);
                   
                    // 세번만 길게2층 연결 허용
                    
                    if (nextNodeComponent.layer == nextLayer2)
                    {
                        doubleConnect--; 
                    }
                    
                   
                    // 선택된 노드 리스트에서 제거하여 중복 선택 방지
                    nextLayerNodes.RemoveAt(randomIndex);

                }
            }
            else if (nodeComponent.layer == 11)
            {
                List<GameObject> layer12Nodes = nodes.FindAll(n => n.GetComponent<TestMapNode>().layer == 12);

                int randomCount = Random.Range(1, 3);
                List<TestMapNode> selectedNextNodes = new List<TestMapNode>();
                for (int i = 0; i < randomCount; i++)
                {
                    if (layer12Nodes.Count == 0) break; // layer 12 노드가 없으면 중단

                    int randomIndex = Random.Range(0, layer12Nodes.Count);
                    TestMapNode nextNodeComponent = layer12Nodes[randomIndex].GetComponent<TestMapNode>();
                    selectedNextNodes.Add(nextNodeComponent);
                    nodeComponent.AddNextNode(nextNodeComponent); // 현재 노드에 다음 노드 연결

                    nextNodeComponent.AddpPreviousNode(nodeComponent);

                    layer12Nodes.RemoveAt(randomIndex);
                }
            }
        }
    }

    public void ConnectEdge(List<GameObject> nodes)
    {
        GameObject root = nodes.Find(node => node.GetComponent<TestMapNode>().layer == 0);
        TestMapNode rootComponent = root.GetComponent<TestMapNode>();

        // rootComponent의 LineRenderer를 설정합니다.
        rootComponent.MaKeLine(rootComponent.nextNodes.Count); // root의 nextNodes 수만큼 LineRenderer 생성
        Debug.Log("Next nodes count: " + rootComponent.nextNodes.Count);
        Debug.Log("Line renderers count: " + rootComponent.lineRenderers.Count);

        // root의 다음 노드와 선 연결
        for (int i = 0; i < rootComponent.nextNodes.Count; i++)
        {
            TestMapNode nextComponent = rootComponent.nextNodes[i];
            DrawLineBetween(rootComponent.lineRenderers[i], rootComponent.gameObject, nextComponent.gameObject);
        }

        // edgeList 초기화
        List<TestMapNode> edgeList = new List<TestMapNode>(rootComponent.nextNodes);
        
        while (edgeList.Count > 0)
        {
            TestMapNode edgeNode = edgeList[0]; // edgeList의 첫 번째 노드를 가져옵니다.

            // edgeNode의 LineRenderer를 설정합니다.
            edgeNode.MaKeLine(edgeNode.nextNodes.Count); // edgeNode의 nextNodes 수만큼 LineRenderer 생성

            for (int i = 0; i < edgeNode.nextNodes.Count; i++)
            {
                TestMapNode nextComponent = edgeNode.nextNodes[i];
                DrawLineBetween(edgeNode.lineRenderers[i], edgeNode.gameObject, nextComponent.gameObject); // edgeNode와 다음 노드 간의 선을 그리기 위한 함수 호출
                edgeList.Add(nextComponent); // 새로운 nextComponent를 edgeList에 추가합니다.
            }

            // 사용이 끝난 edgeNode를 edgeList에서 제거합니다.
            edgeList.RemoveAt(0);
        
        }
        
    }

    private void DrawLineBetween(LineRenderer lineRenderer, GameObject nodeA, GameObject nodeB)
    {
        if (lineRenderer == null || nodeA == null || nodeB == null)
        {
            Debug.LogError("LineRenderer or one of the nodes is null!");
            return;
        }

        TestMapNode nodeAComponent = nodeA.GetComponent<TestMapNode>();
        TestMapNode nodeBComponent = nodeB.GetComponent<TestMapNode>();

        if (nodeAComponent == null || nodeBComponent == null)
        {
            Debug.LogError("One of the nodes is missing the TestMapNode component!");
            return;
        }

        // nodeA와 nodeB의 nodeNumber를 출력합니다.
        Debug.Log($"Drawing line between Node {nodeAComponent.nodeNumber} and Node {nodeBComponent.nodeNumber}");
        Debug.Log($"NodeA (Node {nodeAComponent.nodeNumber}) Position: {nodeA.transform.position}, NodeB (Node {nodeBComponent.nodeNumber}) Position: {nodeB.transform.position}");

        // LineRenderer를 사용하여 nodeA와 nodeB 사이에 선을 그립니다.
        lineRenderer.SetPosition(0, nodeA.transform.position); // 시작점
        lineRenderer.SetPosition(1, nodeB.transform.position); // 끝점
    }
    public GameObject CreateNode(Vector3 position, int layer)
    {
        GameObject node = Instantiate(nodePrefab, position, Quaternion.identity, NodeObj.transform);
        //GameObject node = Instantiate(nodePrefab,position,Quaternion.identity);
        /*
        GameObject parentObject = GameObject.Find("Nodes"); 
        if (parentObject != null)
        {
            node.transform.SetParent(parentObject.transform); 
        }
        */
        TestMapNode nodeComponent = node.GetComponent<TestMapNode>();
        nodeComponent.layer = layer;
        nodeComponent.position = position;

        nodeComponent.nodeNumber = nodeCounter++;
        if (nodeComponent.layer == 0) { nodeComponent.isClicked = true; }
        return node;
    }
    private Vector3 GenerateValidPosition(float yPosition, List<Vector3> generatedPositions)
    {
        Vector3 position;
        bool validPosition;

        do
        {
            float xPosition = Random.Range(-400f, 400f); // x 좌표 랜덤 생성
            position = new Vector3(xPosition, yPosition, 0);

            validPosition = true;

            // 이미 생성된 위치들과 거리가 30 이상 되는지 확인
            foreach (Vector3 generatedPosition in generatedPositions)
            {
                if (Vector3.Distance(position, generatedPosition) < 120f)
                {
                    validPosition = false;
                    break;
                }
            }

        } while (!validPosition);

        return position;
    }

    public int SpawnProbabilty()
    {
        float randomValue = Random.Range(0f, 100f);
       
        if (randomValue < 40f)
        {
            return 0;
        }
        else if (randomValue < 60f)
        {
            return 1;
        }
        else if (randomValue < 80f)
        {
            return 2;
        }
        else if(randomValue < 90f)
        {
            return 3;
        }
        else 
        {
            return 4;
            
        }
    }


    public void RemoveNodesWithNoPreviousNodes()
    {
        List<GameObject> nodesToRemove = nodes.FindAll(node =>
         node.GetComponent<TestMapNode>().previousNodes.Count == 0 &&
         node.GetComponent<TestMapNode>().layer != 0
     );

        // 해당 노드를 nodes 리스트에서 제거하고, 게임 오브젝트도 삭제합니다.
        foreach (GameObject node in nodesToRemove)
        {
            nodes.Remove(node);
            Destroy(node);
        }
    }
}
