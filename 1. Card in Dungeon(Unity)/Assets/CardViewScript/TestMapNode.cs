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
            // ��尡 �����ϰ�, �ش� ����� layer�� ���� ����� layer�� ���ų� �� ���� ���
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

        // �������� �� ��ü ����
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
            mapPrefabInstance.Transform.localScale *= 15f; // scale ����
        }
    }

    private void Update()
    {
        // isClicked ���°� ����� ��쿡�� ó��
        if (isClicked != previousClickState)
        { 
            if (isClicked)
            {
                mapPrefabInstance?.GreenSignOn(); // Ŭ���� ������ ��
            }
            else
            {
                UpdateSameLayerNodes(); // isClicked�� false�� ����� ���
                mapPrefabInstance?.RedSignOn(); // Ŭ�� ���� ����
            }

            // ���� ���¸� ���
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
        // ������ LineRenderer�� �߰��Ǿ� �ִ� ���� ������Ʈ���� ���� ���� (�ߺ� ����)
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        lineRenderers.Clear();

        // ������ ����ŭ LineRenderer�� ���� ������Ʈ�� ����
        for (int i = 0; i < num; i++)
        {
            // ���� ������Ʈ�� �����ϰ� �θ� ���� ������Ʈ�� ����
            GameObject lineObject = new GameObject("LineRenderer_" + i);
            lineObject.transform.SetParent(this.transform);

            // ���� ������Ʈ�� LineRenderer �߰�
            LineRenderer lineRenderer = lineObject.AddComponent<LineRenderer>();

            // LineRenderer �⺻ ���� (����, �ʺ� ��)
            lineRenderer.startColor = Color.white;
            lineRenderer.endColor = Color.white;
            lineRenderer.startWidth = 2f;
            lineRenderer.endWidth = 2f;
            lineRenderer.positionCount = 2; // �� ���� ���
            lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
            lineRenderer.sortingLayerName = "MapObj";
            // LineRenderer�� ����Ʈ�� �߰�
            lineRenderers.Add(lineRenderer);
            Debug.Log("LineRenderer added to child: " + lineRenderer);
        }
    }

    public LineRenderer FindLineRenderer(Vector3 startPoint, Vector3 endPoint)
    {
        foreach (LineRenderer lineRenderer in lineRenderers)
        {
            // �������� ������ ���Ͽ� �ش� LineRenderer ã��
            Vector3 lineStart = lineRenderer.GetPosition(0);
            Vector3 lineEnd = lineRenderer.GetPosition(1);

            // �Ÿ� �񱳸� ���� ��ġ ���� Ȯ�� (0.01f�� ��� ����)
            if (Vector3.Distance(lineStart, startPoint) < 3f &&
                Vector3.Distance(lineEnd, endPoint) < 3f)
            {
                return lineRenderer; // ��ġ�ϴ� LineRenderer ��ȯ
            }
        }

        return null; // ã�� ���� ��� null ��ȯ
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
