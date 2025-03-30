using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMapNode : MonoBehaviour
{

    public int layer;
    public Vector3 position;
    public bool isClicked = false;
    public bool isPressed = false;
    public bool isMinRoute = false;
    public List<TestMapNode> previousNodes;
    public List<TestMapNode> nextNodes;
    public GameObject[] MapPrefabs;
    TestRMGene functions;

    public IMapObject mapPrefabInstance;

    private bool previousClickState = false;
    private bool previousPressState = false;

    public LineRenderer connectlineRenderer;
    public List<LineRenderer> lineRenderers;

    public int nodeNumber;
    public int changecount = 0;

    public void AddNextNode(TestMapNode nextNode)
    {
        if( nextNode != null)
        {
            nextNodes.Add(nextNode);
        }
    }
    public void AddpPreviousNode(TestMapNode previousNode)
    {
        if (previousNodes != null)
        {
            previousNodes.Add(previousNode);
        }
    }

    void UpdateSameLayerNodes()
    {
        foreach (GameObject node in functions.nodes)
        {
            TestMapNode testNode = node.GetComponent<TestMapNode>();
            // 노드가 존재하고, 해당 노드의 layer가 현재 노드의 layer와 같거나 더 낮은 경우
            if (testNode != null && testNode.layer <= this.layer)
            {
                testNode.isClicked = false;
            }
        }
    }

    void Awake()
    {
        nextNodes = new List<TestMapNode>();
        previousNodes = new List<TestMapNode>();
        functions = FindObjectOfType<TestRMGene>();
        lineRenderers = new List<LineRenderer>();
    }

    private void Start()
    {

        position = transform.position;

        // 랜덤으로 맵 객체 생성
        int prefabIndex = functions.SpawnProbabilty();
        if(layer == 0) 
        {
            GameObject prefabInstance = Instantiate(MapPrefabs[0], position, Quaternion.identity, transform);
            mapPrefabInstance = prefabInstance.GetComponent<IMapObject>();
            mapPrefabInstance.SetParentNode(this);
            mapPrefabInstance.RedSignOn(); // ?
            mapPrefabInstance.Transform.localScale *= 15f; 
        }
        else if(layer == 13)
        {
            GameObject prefabInstance = Instantiate(MapPrefabs[5], position, Quaternion.identity, transform);
            mapPrefabInstance = prefabInstance.GetComponent<IMapObject>();
            mapPrefabInstance.SetParentNode(this);
            mapPrefabInstance.RedSignOn(); // ?
            mapPrefabInstance.Transform.localScale *= 15f; 
        }
        else
        {
            GameObject prefabInstance = Instantiate(MapPrefabs[prefabIndex], position, Quaternion.identity, transform);
            mapPrefabInstance = prefabInstance.GetComponent<IMapObject>();
            mapPrefabInstance.SetParentNode(this);
            mapPrefabInstance.RedSignOn();
            mapPrefabInstance.Transform.localScale *= 15f; // scale 조정
        }
    }

    private void Update()
    {
        // isClicked 상태가 변경된 경우에만 처리
        if (isClicked != previousClickState)
        { 
            if (isClicked)
            {
                mapPrefabInstance?.GreenSignOn(); // 클릭된 상태일 때
            }
            else
            {
                UpdateSameLayerNodes(); // isClicked가 false로 변경된 경우
                mapPrefabInstance?.RedSignOn(); // 클릭 해제 상태
            }

            // 이전 상태를 기록
            previousClickState = isClicked;
        }
        if(isPressed != previousPressState)
        {
            
           foreach (LineRenderer lineRenderer in lineRenderers)
           {
               lineRenderer.startColor = Color.green;
               lineRenderer.endColor = Color.green;
           }
           previousPressState = isPressed;
           
        }
        if(functions.chooseNode.Count() >0 && changecount == 1) 
        {
            
            if (functions.chooseNode.GetElements(0) == this && functions.chooseNode.Count() == 2)
            {
                GreenLineOff();
            }
            //changecount++;
        }

        if(isMinRoute == true)
        {
            mapPrefabInstance?.MinSignOn();
        }
        else
        {
            mapPrefabInstance?.MinSignOff();
        }
    }
    public void MaKeLine(int num)
    {
        // 기존의 LineRenderer가 추가되어 있는 서브 오브젝트들을 먼저 제거 (중복 방지)
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        lineRenderers.Clear();

        // 지정된 수만큼 LineRenderer를 서브 오브젝트에 생성
        for (int i = 0; i < num; i++)
        {
            // 서브 오브젝트를 생성하고 부모를 현재 오브젝트로 설정
            GameObject lineObject = new GameObject("LineRenderer_" + i);
            lineObject.transform.SetParent(this.transform);

            // 서브 오브젝트에 LineRenderer 추가
            LineRenderer lineRenderer = lineObject.AddComponent<LineRenderer>();

            // LineRenderer 기본 설정 (색상, 너비 등)
            lineRenderer.startColor = Color.white;
            lineRenderer.endColor = Color.white;
            lineRenderer.startWidth = 2f;
            lineRenderer.endWidth = 2f;
            lineRenderer.positionCount = 2; // 두 점을 사용
            lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
            lineRenderer.sortingLayerName = "MapObj";
            // LineRenderer를 리스트에 추가
            lineRenderers.Add(lineRenderer);
            Debug.Log("LineRenderer added to child: " + lineRenderer);
        }
    }

    public LineRenderer FindLineRenderer(Vector3 startPoint, Vector3 endPoint)
    {
        foreach (LineRenderer lineRenderer in lineRenderers)
        {
            // 시작점과 끝점을 비교하여 해당 LineRenderer 찾기
            Vector3 lineStart = lineRenderer.GetPosition(0);
            Vector3 lineEnd = lineRenderer.GetPosition(1);

            // 거리 비교를 통해 일치 여부 확인 (0.01f는 허용 오차)
            if (Vector3.Distance(lineStart, startPoint) < 3f &&
                Vector3.Distance(lineEnd, endPoint) < 3f)
            {
                return lineRenderer; // 일치하는 LineRenderer 반환
            }
        }

        return null; // 찾지 못한 경우 null 반환
    }

    public void GreenLineOff()
    {
        Vector3 startPoint = functions.chooseNode.GetElements(0).position;
        Vector3 endPoint = functions.chooseNode.GetElements(1).position;

        LineRenderer lineRendererToKeep = FindLineRenderer(startPoint, endPoint);

        foreach (LineRenderer lineRenderer in lineRenderers)
        {
            if (lineRenderer != lineRendererToKeep)
            {
                lineRenderer.startColor = Color.white;
                lineRenderer.endColor = Color.white;
            }
        }
    }
}
