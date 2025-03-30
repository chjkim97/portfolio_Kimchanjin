using System.Collections;
using UnityEngine;

public class PlayerDefense : MonoBehaviour
{
    public Transform spriteTransform; // 스프라이트의 Transform
    float moveDistance = 20f; // 이동 거리
    float moveDuration = 0.3f;   // 이동에 걸리는 시간

    private Vector3 originalPosition; // 원래 자리

    void Start()
    {
        // 스프라이트의 원래 위치 저장
        originalPosition = spriteTransform.localPosition;
    }

    void Update()
    {
        // 테스트용으로 스페이스바를 누르면 이동하도록 설정
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // 이동 코루틴 시작
            StartCoroutine(MoveSprite());
        }
    }

    IEnumerator MoveSprite()
    {
        // 이동할 목표 위치 (-100 만큼 이동)
        Vector3 targetPosition = new Vector3(originalPosition.x - moveDistance, originalPosition.y, originalPosition.z);

        // 이동할 시간에 따라 자연스럽게 이동
        float elapsedTime = 0f;
        while (elapsedTime < moveDuration)
        {
            spriteTransform.localPosition = Vector3.Lerp(originalPosition, targetPosition, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime;
            yield return null; // 다음 프레임으로 넘어감
        }
        // 정확한 목표 위치로 설정
        spriteTransform.localPosition = targetPosition;

        // 다시 원래 자리로 돌아가기
        elapsedTime = 0f;
        while (elapsedTime < moveDuration)
        {
            spriteTransform.localPosition = Vector3.Lerp(targetPosition, originalPosition, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        // 원래 자리로 정확히 복구
        spriteTransform.localPosition = originalPosition;
    }
}
