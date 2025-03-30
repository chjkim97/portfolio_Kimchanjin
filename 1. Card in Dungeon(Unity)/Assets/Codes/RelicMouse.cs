using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class RelicMouse : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject backgroundSprite; // 배경 스프라이트
    public TextMeshProUGUI descriptionText; // 유물 설명 텍스트

    void Start()
    {
        backgroundSprite.SetActive(false);
        descriptionText.gameObject.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // 마우스가 유물 UI에 올라가면 배경과 텍스트를 활성화
        backgroundSprite.SetActive(true);
        descriptionText.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // 마우스가 유물 UI에서 나가면 배경과 텍스트를 비활성화
        backgroundSprite.SetActive(false);
        descriptionText.gameObject.SetActive(false);
    }
}
