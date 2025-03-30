using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapObjShop : MonoBehaviour, IMapObject
{
    public SpriteRenderer redSign;
    private TestMapNode parentNode; // 부모참조
    public TestRMGene testRMGene;

    public Transform Transform => this.transform;
    // 부모 노드 설정 참조
    public void SetParentNode(TestMapNode node)
    {
        parentNode = node;
    }

    private void Start()
    {
        testRMGene = FindObjectOfType<TestRMGene>();
    }


    public void RedSignOn()
    {
        Transform colorObject = transform.Find("RedSign");
        if (colorObject != null)
        {
            redSign = colorObject.GetComponent<SpriteRenderer>();
            if (redSign != null)
            {
                redSign.color = Color.red;
            }
            else
            {
                Debug.LogWarning("SpriteRenderer not found on Color object!");
            }
        }
        else
        {
            Debug.LogWarning("Color object not found!");
        }
    }
    public void GreenSignOn()
    {
        Transform colorObject = transform.Find("RedSign");
        if (colorObject != null)
        {
            redSign = colorObject.GetComponent<SpriteRenderer>();
            if (redSign != null)
            {
                redSign.color = Color.green;
            }
            else
            {
                Debug.LogWarning("SpriteRenderer not found on Color object!");
            }
        }
        else
        {
            Debug.LogWarning("Color object not found!");
        }
    }
    public void MinSignOn()
    {
        Transform minObject = transform.Find("MinRoute");
        if (minObject != null)
        {
            minObject.gameObject.SetActive(true);
        }
        else
        {
            Debug.LogWarning("MinRoute 없음");
        }
    }
    public void MinSignOff()
    {
        Transform minObject = transform.Find("MinRoute");
        if (minObject != null)
        {
            minObject.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogWarning("MinRoute 없음");
        }
    }
    public void OnClicked()
    {
        OnMouseDown();
    }

    public void OnUnclicked()
    {

    }
    void OnMouseDown()
    {
        if (parentNode != null && parentNode.isClicked)
        {
            testRMGene.chooseNode.Enqueue(parentNode);
            parentNode.isClicked = false;
            parentNode.isPressed = true;
            parentNode.changecount++;
            foreach (TestMapNode nextNode in parentNode.nextNodes)
            {
                if (nextNode != null)
                {
                    nextNode.isClicked = true;
                }
            }
            if (testRMGene != null)
            {
                testRMGene.minRoute.FindShortestPathToEnd(testRMGene.nodesList, parentNode);
                testRMGene.HideNodes(); 
            }
            SceneManager.LoadScene("Shop");
        }
        
    }
}
