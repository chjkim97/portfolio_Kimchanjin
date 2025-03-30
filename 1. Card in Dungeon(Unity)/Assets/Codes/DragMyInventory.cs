using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragMyInventory : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    private RectTransform rectTransform;
    private Vector2 offset;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>(); // 패널의 RectTransform을 가져옴
    }

    // 마우스를 눌렀을 때 호출되는 함수
    public void OnPointerDown(PointerEventData eventData)
    {
        // 패널의 위치와 마우스 클릭 위치 차이를 계산
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, eventData.position, eventData.pressEventCamera, out offset);
    }

    // 마우스를 드래그할 때 호출되는 함수
    public void OnDrag(PointerEventData eventData)
    {
        Vector2 mousePos;
        // 현재 마우스 위치를 로컬 좌표로 변환
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform.parent as RectTransform, eventData.position, eventData.pressEventCamera, out mousePos))
        {
            rectTransform.localPosition = mousePos - offset; // 패널 이동
        }
    }
}
