using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Order : MonoBehaviour
{
    [SerializeField] Renderer[] characterRenderers; // 추가된 부분: characterRenderers 변수 추가
    [SerializeField] Renderer[] backRenderers;
    [SerializeField] Renderer[] middleRenderers;
    //[SerializeField] Renderer[] topRenderers;
    [SerializeField] string sortingLayerName;
    int originOrder;

    private void Start()
    { 
        SetOrder(0);
    }


    public void SetOriginOrder(int originOrder)
    {
        this.originOrder = originOrder;
        SetOrder(originOrder);
    }

    public void SetMostFrontOrder(bool isMostFront)
    {
        SetOrder(isMostFront ? 100 : originOrder);
    }
    public void SetOrder(int order)
    {
        int mulOrder = order * 10;

        // 추가된 부분: 아래 foreach문 하나 추가(characterRenderers)
        foreach (var renderer in characterRenderers)
        {
            renderer.sortingLayerName = sortingLayerName;
            renderer.sortingOrder = mulOrder - 1;
        }

        foreach (var  renderer in backRenderers)
        {
            renderer.sortingLayerName = sortingLayerName;
            renderer.sortingOrder = mulOrder;
        }
        foreach(var renderer in middleRenderers)
        {
            renderer.sortingLayerName = sortingLayerName;
            renderer.sortingOrder = mulOrder + 4 ;  // 추가된 부분: 카드 여러개 생성시 layer 겹칠까바 +1에서 +4로 변경
        }

    }

}
