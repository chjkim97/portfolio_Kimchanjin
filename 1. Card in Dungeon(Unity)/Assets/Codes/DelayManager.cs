using UnityEngine;
using System.Collections;

public class DelayManager : MonoBehaviour
{
    // 0.5초 딜레이를 주는 함수를 호출할 때 실행할 동작을 매개변수로 전달
    public void DelayAction(System.Action action)
    {
        StartCoroutine(DelayCoroutine(action));
    }
    private IEnumerator DelayCoroutine(System.Action action)
    {
        yield return new WaitForSeconds(1.0f);

        action?.Invoke();
    }
}
